using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Mustang.SqlBuilder;

namespace Mustang.DataAccess
{
    public class Database
    {
        internal string ConnectionString { get; set; }

        internal DbProviderFactory DbProviderFactory { get; set; }

        internal Database(string connectionString, string providerName)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(nameof(connectionString));

            ConnectionString = connectionString;
            DbProviderFactory = ProviderFactory.GetDbProviderFactory(providerName);
        }

        internal DbCommand CreateCommand(string commandText, List<SqlParameter>? sqlParameters = null)
        {
            if (string.IsNullOrWhiteSpace(commandText))
                throw new ArgumentException("commandText");

            var command = DbProviderFactory.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = commandText;

            if (sqlParameters != null && sqlParameters.Any())
            {
                foreach (var databaseParameter in sqlParameters)
                {
                    AddInParameter(command, databaseParameter.ColumnName, databaseParameter.DbType, databaseParameter.Value);
                }
            }

            return command;
        }


        internal virtual void AddInParameter(DbCommand command, string name, DbType dbType, object value)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var parameter = DbProviderFactory.CreateParameter();
            parameter.ParameterName = "@" + name;
            parameter.DbType = dbType;
            parameter.Size = 0;
            parameter.Value = value ?? DBNull.Value;
            parameter.Direction = ParameterDirection.Input;
            parameter.IsNullable = false;
            parameter.SourceColumn = string.Empty;
            parameter.SourceVersion = DataRowVersion.Default;
            command.Parameters.Add(parameter);
        }

        internal object? ExecuteScalar(DbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            using var wrapper = GetOpenConnection();
            PrepareCommand(command, wrapper.Connection);
            return command.ExecuteScalar();
        }

        internal async Task<object?> ExecuteScalarAsync(DbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            using var wrapper = GetOpenConnection();
            PrepareCommand(command, wrapper.Connection);
            var result = await command.ExecuteScalarAsync();

            return result;
        }

        internal int ExecuteNonQuery(DbCommand command)
        {
            using var wrapper = GetOpenConnection();
            PrepareCommand(command, wrapper.Connection);
            return command.ExecuteNonQuery();
        }

        internal Task<int> ExecuteNonQueryAsync(DbCommand command)
        {
            using var wrapper = GetOpenConnection();
            PrepareCommand(command, wrapper.Connection);
            return command.ExecuteNonQueryAsync();
        }

        internal DataSet ExecuteDataSet(DbCommand command)
        {
            var dataSet = new DataSet { Locale = CultureInfo.InvariantCulture };

            using var wrapper = GetOpenConnection();

            using var adapter = DbProviderFactory.CreateDataAdapter();

            PrepareCommand(command, wrapper.Connection);

            if (adapter == null)
                return dataSet;

            adapter.SelectCommand = command;

            adapter.Fill(dataSet);

            return dataSet;
        }

        internal IDataReader ExecuteReader(DbCommand command)
        {
            using var wrapper = GetOpenConnection();

            PrepareCommand(command, wrapper.Connection);

            var cmdBehavior = Transaction.Current == null ? CommandBehavior.CloseConnection : CommandBehavior.Default;

            IDataReader realReader = command.ExecuteReader(cmdBehavior);

            return new RefCountingDataReader(wrapper, realReader);
        }

        internal void PrepareCommand(DbCommand command, DbConnection connection)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (connection == null)
                throw new ArgumentNullException(nameof(connection));

            command.Connection = connection;
        }

        #region Connection

        private DatabaseConnectionWrapper GetOpenConnection()
        {
            var connection = TransactionScopeConnections.GetConnection(this);

            return connection ?? GetWrappedConnection();

        }

        private DatabaseConnectionWrapper GetWrappedConnection()
        {
            return new DatabaseConnectionWrapper(GetNewOpenConnection());
        }

        internal DbConnection GetNewOpenConnection()
        {
            DbConnection connection = null;
            try
            {
                connection = CreateConnection();
                connection.Open();
            }
            catch
            {
                connection?.Close();
                throw;
            }

            return connection;
        }

        private DbConnection CreateConnection()
        {
            var connection = DbProviderFactory.CreateConnection();
            if (connection == null)
                throw new ArgumentNullException("");

            connection.ConnectionString = ConnectionString;

            return connection;
        }


        #endregion

    }
}
