using System.Collections.Generic;

namespace Mustang.SqlBuilder
{
    public class EntityContext
    {
        public string TableName { get; set; }

        public string FullNameTableName { get; set; }

        public List<EntityPropertyValue> PropertyValues { get; set; }

    }
}
