using System;
using System.Collections.Generic;
using System.Text;
using Mustang.DataAccess.Connection;

namespace Mustang.DataAccess.Config
{
    public class DataAccessConfig
    {
        public List<ConnectionStringConfig> ConnectionStringConfigs { get; set; }

        public bool EnableTransaction { get; set; }
    }
}
