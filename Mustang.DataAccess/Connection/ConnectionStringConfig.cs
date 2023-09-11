using System;
using System.Collections.Generic;
using System.Text;

namespace Mustang.DataAccess.Connection
{
    public sealed class ConnectionStringConfig
    {
        public string ConnectionName { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

        public ConnectionStringConfig(string connectionName, string connectionString, string providerName)
        {
            ConnectionName = connectionName;
            ConnectionString = connectionString;
            ProviderName = providerName;
        }
    }
}
