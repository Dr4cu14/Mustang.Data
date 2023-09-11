using System;
using System.Data;

namespace Mustang.SqlBuilder
{
    public class SqlParameter
    {
        public string ColumnName { get; set; }

        public object Value { get; set; }

        public DbType DbType { get; set; }

        public SqlParameter(string columnName, object value)
        {
            ColumnName = columnName;
            Value = value;

            if ((value != null) && (value != DBNull.Value))
            {
                if (Value is Enum)
                {
                    Value = Convert.ToInt16(value);
                    DbType = DbType.Int16;
                    return;
                }

                DbType = Transfer2DbType.TransferToDataType(value);
            }
        }
    }
}
