using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using Mustang.DataAccess.Static;

namespace Mustang.DataAccess;

public class ConnectionStringManager
{
    private static readonly List<ConnectionStringConfig> ConnectionStringConfigs = new();

    static ConnectionStringManager()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true)
            .Build();


        if (builder == null)
            throw new FileNotFoundException("appstrings.json cannot be found");


        var defaultSection1 = builder.GetSection("Mustang:DBConnectionStrings").Get<List<ConnectionStringConfig>>();

        var defaultSection = builder.GetSection(GlobalSettings.DefaultSection);
        if (defaultSection == null)
            throw new KeyNotFoundException("this default section cannot be found");

        foreach (var item in GlobalSettings.DefaultConnectionStringKey)
        {
            var connectionStrList = defaultSection.GetSection(item).Get<List<ConnectionStringConfig>>();

            ConnectionStringConfigs.AddRange(connectionStrList);
        }


        if (ConnectionStringConfigs == null || !ConnectionStringConfigs.Any() || ConnectionStringConfigs.Count == 0)
            throw new KeyNotFoundException("this ConnectionStrings cannot be found");

    }

    public static ConnectionStringConfig GetConnectionStringConfig(string connectionName = "common")
    {
        var connectionStringConfig = ConnectionStringConfigs.FirstOrDefault(w => w.ConnectionName == connectionName);

        if (connectionStringConfig == null)
            throw new ArgumentException("This connection Name cannot be found");

        return connectionStringConfig;
    }

}