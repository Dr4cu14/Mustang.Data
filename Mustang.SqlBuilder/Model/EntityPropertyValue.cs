using System;
using System.Data;

namespace Mustang.SqlBuilder
{
    public class EntityPropertyValue
    {
        public string PropertyName { get; set; }

        public object PropertyValue { get; set; }

        public DbType DbType { get; set; }

        public EntityPropertyValue(string propertyName, object propertyValue)
        {
            PropertyName = propertyName;
            PropertyValue = propertyValue;

            if ((propertyValue != null) && (propertyValue != DBNull.Value))
            {
                if (propertyValue is Enum)
                {
                    propertyValue = Convert.ToInt16(propertyValue);
                    DbType = DbType.Int16;
                    return;
                }

                DbType = Transfer2DbType.TransferToDataType(propertyValue);
            }
        }
    }
}
