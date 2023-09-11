using System;
using System.Collections.Generic;
using System.Text;

using Mustang.Map;

namespace Mustang.Demo.Entity
{
    [TableName("account", "Mustang.Account")]
    public class Account
    {

        public int? Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime? InDate { get; set; }
    }
}
