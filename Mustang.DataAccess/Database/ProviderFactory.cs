using System;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Mustang.DataAccess
{
    internal sealed class ProviderFactory
    {
        internal static DbProviderFactory GetDbProviderFactory(string providerName)
        {
            DbProviderFactory factory;

            switch (providerName)
            {
                case "SqlServer":
                    factory = SqlClientFactory.Instance;
                    break;


                case "MySql":
                    factory = MySqlClientFactory.Instance;
                    break;

                //case "TDEngine":
                //    factory = MySqlClientFactory.Instance;
                //    break;


                default:
                    throw new ArgumentException(@"Can not recognize name:" + providerName);
            }
            return factory;
        }
    }
}
