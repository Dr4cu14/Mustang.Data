using Mustang.DataAccess.Connection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using Mustang.SqlBuilder;

namespace Mustang.DataAccess
{
    public class MustangDataAccess
    {
        private static ConnectionStringConfig GetConnectionConfig(string connectionStringName)
        {
            var connectionConfig = !string.IsNullOrWhiteSpace(connectionStringName)
                ? ConnectionStringConfiguration.GetConnectionStringConfig(connectionStringName)
                : ConnectionStringConfiguration.GetConnectionStringConfig();

            if (connectionConfig == null)
                throw new ArgumentNullException(nameof(connectionStringName));

            return connectionConfig;
        }

        private static Database GetDatabase(ConnectionStringConfig connectionStringConfig)
        {
            if (connectionStringConfig == null)
                throw new ArgumentNullException(nameof(connectionStringConfig));

            return new Database(connectionStringConfig.ConnectionString, connectionStringConfig.ProviderName);
        }

        public static int ExecuteScalar<T>(SqlBuilder<T> sqlBuilder, string connectionName = null) where T : class, new()
        {
            var connectionConfig = GetConnectionConfig(connectionName);

            var database = GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql);

            foreach (var databaseParameter in sqlBuilder.SqlParameterList)
            {
                database.AddInParameter(command, databaseParameter.ColumnName, databaseParameter.DbType, databaseParameter.Value);
            }

            return Convert.ToInt32(database.ExecuteScalar(command));
        }

        public static int ExecuteNonQuery<T>(SqlBuilder<T> sqlBuilder, string connectionName = null) where T : class, new()
        {
            var connectionConfig = GetConnectionConfig(connectionName);

            var database = GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql);

            foreach (var databaseParameter in sqlBuilder.SqlParameterList)
            {
                database.AddInParameter(command, databaseParameter.ColumnName, databaseParameter.DbType, databaseParameter.Value);
            }

            return database.ExecuteNonQuery(command);
        }

        public static T ExecuteReader<T>(SqlBuilder<T> sqlBuilder, string connectionName = null) where T : class, new()
        {
            var connectionConfig = GetConnectionConfig(connectionName);

            var database = GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql);

            foreach (var databaseParameter in sqlBuilder.SqlParameterList)
            {
                database.AddInParameter(command, databaseParameter.ColumnName, databaseParameter.DbType, databaseParameter.Value);
            }

            var reader = database.ExecuteReader(command);


            return reader.ToEntity<T>();
        }

        public static List<T> ExecuteReaderList<T>(SqlBuilder<T> sqlBuilder, string connectionName = null) where T : class, new()
        {
            var connectionConfig = GetConnectionConfig(connectionName);

            var database = GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql);

            foreach (var databaseParameter in sqlBuilder.SqlParameterList)
            {
                database.AddInParameter(command, databaseParameter.ColumnName, databaseParameter.DbType, databaseParameter.Value);
            }

            var reader = database.ExecuteReader(command);

            return reader.ToEntityList<T>();
        }

        public static DataSet ExecuteDataSet<T>(SqlBuilder<T> sqlBuilder, string connectionName = null) where T : class, new()
        {
            var connectionConfig = GetConnectionConfig(connectionName);

            var database = GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql);

            foreach (var databaseParameter in sqlBuilder.SqlParameterList)
            {
                database.AddInParameter(command, databaseParameter.ColumnName, databaseParameter.DbType, databaseParameter.Value);
            }

            return database.ExecuteDataSet(command);
        }

        //public static List<TResult> QueryPaging<TEntity, TResult>(SqlBuilder<TEntity> sqlBuilder, out int totalCount, string connectionName = null) where TEntity : class, new() where TResult : class, new()
        //{
        //    totalCount = 0;

        //    var connectionConfig = GetConnectionConfig(connectionName);

        //    var database = GetDatabase(connectionConfig);

        //    var command = database.CreateCommand(sqlBuilder.Sql);

        //    foreach (var databaseParameter in sqlBuilder.SqlParameterList)
        //    {
        //        database.AddInParameter(command, databaseParameter.ColumnName, databaseParameter.DbType, databaseParameter.Value);
        //    }

        //    var dataSet = database.ExecuteDataSet(command);
        //    if (dataSet != null && dataSet.Tables.Count == 2)
        //    {
        //        totalCount = Convert.ToInt32(dataSet.Tables[0].Rows[0][0].ToString());
        //        return dataSet.Tables[0].ToEntityList<TResult>();
        //    }

        //    return null;
        //}
    }
}
