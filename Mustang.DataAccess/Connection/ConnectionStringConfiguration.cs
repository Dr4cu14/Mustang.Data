using Microsoft.Extensions.Configuration;
using Mustang.DataAccess.Config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mustang.DataAccess.Connection
{
    public static class ConnectionStringConfiguration
    {
        private static ConcurrentDictionary<string, ConnectionStringConfig> ConnectionStringDictionary = new ConcurrentDictionary<string, ConnectionStringConfig>();

        public static ConnectionStringConfig GetConnectionStringConfig(string connectionName = "Common")
        {
            if (ConnectionStringDictionary.TryGetValue(connectionName, out ConnectionStringConfig connectionStringConfig))
                return connectionStringConfig;

            var connectionStringConfigs = DataAccessConfiguration.DataAccessConfig.ConnectionStringConfigs;
            if (connectionStringConfigs != null && connectionStringConfigs.Any())
                connectionStringConfig = connectionStringConfigs.FirstOrDefault(w => w.ConnectionName == connectionName);

            if (connectionStringConfig == null)
                throw new ArgumentException("This connection Name cannot be found");

            ConnectionStringDictionary.TryAdd(connectionName, connectionStringConfig);

            return connectionStringConfig;
        }
    }
}
