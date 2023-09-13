using Mustang.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mustang.Demo.Entity
{
    [TableName("role", "Mustang.Role")]
    public class RoleAccount
    {
        public int? Id { get; set; }

        public int AccountId { get; set; }

        public string RoleName { get; set; }

        public DateTime? InDate { get; set; }
    }
}
