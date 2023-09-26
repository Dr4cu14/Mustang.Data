using System.Collections.Generic;

namespace Mustang.DataAccess;

public class ConnectionStringConfig
{
    public string ConnectionName { get; set; }

    public string ProviderName { get; set; }

    public string MasterConnectionString { get; set; } = string.Empty;

    public List<string> SalveConnectionStrings { get; set; } = new();
}