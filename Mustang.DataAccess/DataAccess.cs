using System.Collections.Generic;
using Mustang.SqlBuilder;
using System.Threading.Tasks;
using System.Data;
using System;
using Mustang.Common;

namespace Mustang.DataAccess
{
    public static class DataAccess
    {
        public static T ExecuteScalar<T>(ISqlBuilder sqlBuilder, string connectionName = null!)
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql, sqlBuilder.SqlParameters);

            try
            {
                var result = database.ExecuteScalar(command);
                return result.ToProgramType<T>();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, connectionConfig.MasterConnectionString, command.CommandText, command.Parameters);
            }
        }

        public static T ExecuteScalar<T>(string sql, List<SqlParameter> sqlParameters = null!, string connectionName = null!)
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sql, sqlParameters);

            try
            {
                var result = database.ExecuteScalar(command);
                return result.ToProgramType<T>();
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, connectionConfig.MasterConnectionString, command.CommandText, command.Parameters);
            }
        }

        public static int ExecuteNonQuery(ISqlBuilder sqlBuilder, string connectionName = null!)
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql, sqlBuilder.SqlParameters);

            try
            {
                return database.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, connectionConfig.MasterConnectionString, command.CommandText, command.Parameters);
            }
        }

        public static int ExecuteNonQuery(string sql, List<SqlParameter> sqlParameters = null!, string connectionName = null!)
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sql, sqlParameters);

            try
            {
                return database.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, connectionConfig.MasterConnectionString, command.CommandText, command.Parameters);
            }
        }

        public static T ExecuteReader<T>(ISqlBuilder sqlBuilder, string connectionName = null!) where T : class, new()
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql, sqlBuilder.SqlParameters);

            IDataReader? dataReader = null;

            try
            {
                dataReader = database.ExecuteReader(command);

                var entity = dataReader.ToEntity<T>();

                dataReader.Close();

                return entity;
            }
            catch (Exception ex)
            {
                dataReader?.Close();

                throw new DataAccessException(ex, connectionConfig.MasterConnectionString, command.CommandText, command.Parameters);
            }
        }

        public static T ExecuteReader<T>(string sql, List<SqlParameter> sqlParameters = null!, string connectionName = null!) where T : class, new()
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sql, sqlParameters);

            IDataReader? dataReader = null;

            try
            {
                dataReader = database.ExecuteReader(command);
                var entity = dataReader.ToEntity<T>();

                dataReader.Close();

                return entity;
            }
            catch (Exception ex)
            {
                dataReader?.Close();

                throw new DataAccessException(ex, connectionConfig.MasterConnectionString, command.CommandText, command.Parameters);
            }
        }

        public static List<T> ExecuteReaderList<T>(ISqlBuilder sqlBuilder, string connectionName = null!) where T : class, new()
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql, sqlBuilder.SqlParameters);

            IDataReader? dataReader = null;

            try
            {
                dataReader = database.ExecuteReader(command);

                var entity = dataReader.ToEntityList<T>();

                dataReader.Close();

                return entity;
            }
            catch (Exception ex)
            {
                dataReader?.Close();

                throw new DataAccessException(ex, connectionConfig.MasterConnectionString, command.CommandText, command.Parameters);
            }
        }

        public static List<T> ExecuteReaderList<T>(string sql, List<SqlParameter> sqlParameters = null!, string connectionName = null!) where T : class, new()
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sql, sqlParameters);

            IDataReader? dataReader = null;
            try
            {
                dataReader = database.ExecuteReader(command);

                var entity = dataReader.ToEntityList<T>();

                dataReader.Close();

                return entity;
            }
            catch (Exception ex)
            {
                dataReader?.Close();

                throw new DataAccessException(ex, connectionConfig.MasterConnectionString, command.CommandText, command.Parameters);
            }
        }

        public static DataSet ExecuteDataSet(ISqlBuilder sqlBuilder, string connectionName = null!)
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql, sqlBuilder.SqlParameters);

            try
            {
                return database.ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, connectionConfig.MasterConnectionString, command.CommandText, command.Parameters);
            }
        }

        public static DataSet ExecuteDataSet(string sql, List<SqlParameter> sqlParameters = null!, string connectionName = null!)
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sql, sqlParameters);

            try
            {
                return database.ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(ex, connectionConfig.MasterConnectionString, command.CommandText, command.Parameters);
            }
        }

        public static List<T> ExecuteReaderColumnList<T>(ISqlBuilder sqlBuilder, string connectionName = null!) where T : struct
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql, sqlBuilder.SqlParameters);

            IDataReader? dataReader = null;

            try
            {
                dataReader = database.ExecuteReader(command);

                var result = dataReader.ToColumnList<T>();

                dataReader.Close();

                return result;
            }
            catch (Exception ex)
            {
                dataReader?.Close();

                throw new DataAccessException(ex, connectionConfig.MasterConnectionString, command.CommandText, command.Parameters);
            }
        }

        public static List<T> ExecuteReaderColumnList<T>(string sql, List<SqlParameter> sqlParameters = null!, string connectionName = null!) where T : struct
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sql, sqlParameters);

            IDataReader? dataReader = null;

            try
            {
                dataReader = database.ExecuteReader(command);

                var result = dataReader.ToColumnList<T>();

                dataReader.Close();

                return result;
            }
            catch (Exception ex)
            {
                dataReader?.Close();

                throw new DataAccessException(ex, connectionConfig.MasterConnectionString, command.CommandText, command.Parameters);
            }
        }
    }
}
