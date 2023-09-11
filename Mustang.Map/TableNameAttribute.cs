using System;

namespace Mustang.Map
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class TableNameAttribute : Attribute
    {
        public string TableName { get; private set; }

        public string FullNameTableName { get; private set; }

        public TableNameAttribute(string tableName, string fullTableName)
        {
            TableName = tableName;
            FullNameTableName = fullTableName;
        }
    }
}
