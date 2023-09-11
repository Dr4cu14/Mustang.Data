using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mustang.DataAccess.Config
{
    public sealed class DataAccessConfiguration
    {
        public static DataAccessConfig DataAccessConfig;

        static DataAccessConfiguration()
        {
            var configuration = new ConfigurationBuilder();
            configuration.AddJsonFile(Directory.GetCurrentDirectory() + "dataAccess.json", optional: true, reloadOnChange: true);
            var configurationRoot = configuration.Build();

            DataAccessConfig = configurationRoot.Get<DataAccessConfig>();
        }
    }
}
