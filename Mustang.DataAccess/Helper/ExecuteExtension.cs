using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Mustang.DataAccess
{
    public static class ExecuteExtension
    {
        public static T ToEntity<T>(this IDataReader dataReader) where T : class, new()
        {
            var entity = new T();

            var entityType = typeof(T);

            while (dataReader.Read())
            {
                for (var i = 0; i < dataReader.FieldCount; i++)
                {
                    var data = dataReader[i];
                    if (data == null || data is DBNull)
                        continue;

                    var propertyInfo = entityType.GetProperty(dataReader.GetName(i), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                    propertyInfo.SetValue(entity, CheckType(dataReader[i], propertyInfo.PropertyType), null);
                }
            }
            return entity;
        }

        public static List<T> ToEntityList<T>(this IDataReader dataReader) where T : class, new()
        {
            var result = new List<T>();

            var entityType = typeof(T);

            while (dataReader.Read())
            {
                var entity = new T();

                for (var i = 0; i < dataReader.FieldCount; i++)
                {
                    var data = dataReader[i];
                    if (data == null || data is DBNull)
                        continue;

                    var propertyInfo = entityType.GetProperty(dataReader.GetName(i), BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                    propertyInfo.SetValue(entity, CheckType(data, propertyInfo.PropertyType), null);
                }
            }

            return result;
        }

        private static object CheckType(object value, Type conversionType)
        {
            if (!conversionType.IsGenericType || conversionType.GetGenericTypeDefinition() != typeof(Nullable<>))
                return Convert.ChangeType(value, conversionType);

            if (value == null)
                return null;

            var nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
            conversionType = nullableConverter.UnderlyingType;
            return Convert.ChangeType(value, conversionType);
        }

        public static List<T> ToEntityList<T>(this DataTable dataTable) where T : class, new()
        {
            var result = new List<T>();

            var entityType = typeof(T);

            var propertys = entityType.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            foreach (DataRow row in dataTable.Rows)
            {
                var entity = new T();

                foreach (var propertyInfo in propertys)
                {
                    if (dataTable.Columns.Contains(propertyInfo.Name))
                    {
                        if (!propertyInfo.CanWrite) continue;
                        var value = row[propertyInfo.Name];
                        if (value != DBNull.Value)
                            propertyInfo.SetValue(entity, value, null);
                    }
                }
            }


            return result;
        }
    }
}
