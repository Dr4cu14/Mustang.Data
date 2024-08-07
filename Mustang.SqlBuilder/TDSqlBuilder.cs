﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mustang.SqlBuilder
{
    public class TDSqlBuilder<T> : SqlBuilder<T> where T : class, new()
    {
        private readonly string _dynamicTableName;

        public TDSqlBuilder(string dynamicTableName)
        {
            if (string.IsNullOrWhiteSpace(dynamicTableName))
                throw new ArgumentNullException(nameof(dynamicTableName));

            _dynamicTableName = dynamicTableName;
        }

        public override SqlBuilder<T> Insert(T entity)
        {
            EntityContext = EntityHelper.GetEntityContext(entity);
            EntityContext.TableName = EntityContext.TableName + "_" + _dynamicTableName;
            EntityContext.FullNameTableName = EntityContext.FullNameTableName + "_" + _dynamicTableName;

            var columnNames = new List<string>();
            var values = new List<string>();

            foreach (var propertyValue in EntityContext.PropertyValues)
            {
                if (propertyValue.PropertyValue == null)
                    continue;

                columnNames.Add(propertyValue.PropertyName);

                var valueType = Transfer2DbType.TransferToTDDataType(propertyValue.PropertyValue);

                if (valueType == System.Data.DbType.String)
                {
                    if (propertyValue.PropertyName == "ts" && propertyValue.PropertyValue.ToString() == "NOW()")
                    {
                        values.Add($"{propertyValue.PropertyValue}");
                    }
                    else
                    {
                        values.Add($"'{propertyValue.PropertyValue}'");
                    }
                }



                if (valueType == System.Data.DbType.Int32)
                    values.Add($"{propertyValue.PropertyValue}");

                SqlParameters.Add(new SqlParameter(propertyValue.PropertyName, propertyValue.PropertyValue));
            }

            Statement.Append($"INSERT INTO {EntityContext.FullNameTableName} ({string.Join(",", columnNames)}) VALUES({string.Join(",", values)})");

            return this;
        }
    }
}
