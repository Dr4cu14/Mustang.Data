using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Transactions;

namespace Mustang.DataAccess
{
    public class Database
    {
        internal string ConnectionString { get; set; }

        internal DbProviderFactory DbProviderFactory { get; set; }

        internal Database(string connectionString, string provdierName)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException(nameof(connectionString));

            ConnectionString = connectionString;
            DbProviderFactory = ProviderFactory.GetDbProviderFactory(provdierName);
        }

        public DbCommand CreateCommand(string commandText)
        {
            if (string.IsNullOrWhiteSpace(commandText))
                throw new ArgumentException("SQL");

            var command = DbProviderFactory.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = commandText;

            return command;
        }
        public virtual void AddInParameter(DbCommand command, string name, DbType dbType, object value)
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

        public object ExecuteScalar(DbCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            using (var wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                return command.ExecuteScalar();
            }
        }

        public int ExecuteNonQuery(DbCommand command)
        {
            using (var wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                return command.ExecuteNonQuery();
            }
        }

        public DataSet ExecuteDataSet(DbCommand command)
        {
            var dataSet = new DataSet {Locale = CultureInfo.InvariantCulture};
            using (var wrapper = GetOpenConnection())
            using (var adapter = DbProviderFactory.CreateDataAdapter())
            {
                PrepareCommand(command, wrapper.Connection);
                if (adapter != null)
                {
                    adapter.SelectCommand = command;

                    adapter.Fill(dataSet);
                }
            }
            return dataSet;
        }

        public IDataReader ExecuteReader(DbCommand command)
        {
            using (var wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                var cmdBehavior = Transaction.Current == null ? CommandBehavior.CloseConnection : CommandBehavior.Default;
                IDataReader realReader = command.ExecuteReader(cmdBehavior);
                return new RefCountingDataReader(wrapper, realReader);
            }
        }

        private void PrepareCommand(DbCommand command, DbConnection connection)
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
