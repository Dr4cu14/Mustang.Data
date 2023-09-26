using System.Collections.Concurrent;

namespace Mustang.DataAccess;

public class DatabaseManager
{
    private static readonly ConcurrentDictionary<string, Database> Databases = new();

    public static Database GetDatabase(ConnectionStringConfig connectionStringConfig)
    {
        if (Databases.TryGetValue(connectionStringConfig.ConnectionName, out var database))
            return database;

        database = new Database(connectionStringConfig.MasterConnectionString, connectionStringConfig.ProviderName);

        Databases.TryAdd(connectionStringConfig.ConnectionName, database);

        return database;
    }
}