using System.Collections.Generic;
using Mustang.SqlBuilder;
using System.Threading.Tasks;

namespace Mustang.DataAccess
{
    public static class DataAccess
    {
        public static TResult ExecuteScalar<TResult>(ISqlBuilder sqlBuilder, string connectionName = null!)
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql, sqlBuilder.SqlParameters);

            var result = database.ExecuteScalar<TResult>(command);

            return result.ToProgramType<TResult>();
        }

        public static TResult ExecuteScalar<TResult>(string sql, List<SqlParameter> sqlParameters = null!, string connectionName = null!)
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sql, sqlParameters);

            var result = database.ExecuteScalar<TResult>(command);

            return result.ToProgramType<TResult>();
        }

        public static int ExecuteNonQuery(ISqlBuilder sqlBuilder, string? connectionName = null)
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql, sqlBuilder.SqlParameters);

            return database.ExecuteNonQuery(command);
        }

        public static int ExecuteNonQuery(string sql, List<SqlParameter> sqlParameters = null!, string connectionName = null!)
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sql, sqlParameters);

            return database.ExecuteNonQuery(command);
        }

        public static Task<int> ExecuteNonQueryAsync(ISqlBuilder sqlBuilder, string? connectionName = null)
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql, sqlBuilder.SqlParameters);

            return database.ExecuteNonQueryAsync(command);
        }

        public static TResult ExecuteReader<TResult>(ISqlBuilder sqlBuilder, string connectionName = null!) where TResult : class, new()
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql, sqlBuilder.SqlParameters);

            var reader = database.ExecuteReader(command);

            return reader.ToEntity<TResult>();
        }

        public static List<T> ExecuteReaderList<T>(ISqlBuilder sqlBuilder, string connectionName = null!) where T : class, new()
        {
            var connectionConfig = ConnectionStringManager.GetConnectionStringConfig(connectionName);

            var database = DatabaseManager.GetDatabase(connectionConfig);

            var command = database.CreateCommand(sqlBuilder.Sql, sqlBuilder.SqlParameters);

            var reader = database.ExecuteReader(command);

            return reader.ToEntityList<T>();
        }
    }
}
