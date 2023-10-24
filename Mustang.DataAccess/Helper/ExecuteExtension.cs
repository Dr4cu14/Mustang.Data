using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Mustang.DataAccess
{
    public static class ExecuteExtension
    {

        public static T ToProgramType<T>(this object? value)
        {
            if (value == null || value == DBNull.Value)
                return default;

            if (value is T newValue)
                return newValue;

            return default;
        }

        public static T ToEntity<T>(this IDataReader dataReader) where T : class, new()
        {
            var entity = new T();

            var entityType = typeof(T);

            var propertys = entityType.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            while (dataReader.Read())
            {
                for (var i = 0; i < dataReader.FieldCount; i++)
                {
                    var columnName = dataReader.GetName(i);
                    var data = dataReader[columnName];
                    if (data == null || data is DBNull)
                        continue;

                    var propertyInfo = propertys.FirstOrDefault(w => w.Name == columnName);
                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(entity, CheckType(data, propertyInfo.PropertyType), null);
                    }
                }
            }
            return entity;
        }

        public static List<T> ToEntityList<T>(this IDataReader dataReader) where T : class, new()
        {
            var result = new List<T>();

            var entityType = typeof(T);

            var propertys = entityType.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            while (dataReader.Read())
            {
                var entity = new T();

                for (var i = 0; i < dataReader.FieldCount; i++)
                {
                    var columnName = dataReader.GetName(i);
                    var data = dataReader[columnName];
                    if (data == null || data is DBNull)
                        continue;

                    var propertyInfo = propertys.FirstOrDefault(w => w.Name == columnName);
                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(entity, CheckType(data, propertyInfo.PropertyType), null);
                    }
                }

                result.Add(entity);
            }

            return result;
        }

        public static List<T> ToColumnList<T>(this IDataReader dataReader) where T : struct
        {
            var result = new List<T>();

            while (dataReader.Read())
            {
                var data = dataReader[0];
                if (data == null || data is DBNull)
                    continue;

                result.Add((T)data);
            }

            return result;
        }


        public static T ToEntity<T>(this DataTable dataTable) where T : class, new()
        {
            var entityType = typeof(T);

            var propertys = entityType.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            var row = dataTable.Rows[0];

            var entity = new T();

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var columnName = dataTable.Columns[i].ColumnName;
                var data = row[columnName];
                if (data == null || data is DBNull)
                    continue;

                var propertyInfo = propertys.FirstOrDefault(w => w.Name == columnName);
                if (propertyInfo != null && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(entity, CheckType(data, propertyInfo.PropertyType), null);
                }
            }

            return entity;
        }

        public static List<T> ToEntityList<T>(this DataTable dataTable) where T : class, new()
        {
            var result = new List<T>();

            var entityType = typeof(T);

            var propertys = entityType.GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            foreach (DataRow row in dataTable.Rows)
            {
                var entity = new T();

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    var columnName = dataTable.Columns[i].ColumnName;
                    var data = row[columnName];
                    if (data == null || data is DBNull)
                        continue;

                    var propertyInfo = propertys.FirstOrDefault(w => w.Name == columnName);
                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(entity, CheckType(data, propertyInfo.PropertyType), null);
                    }
                }

                result.Add(entity);
            }


            return result;
        }


        private static object CheckType(object value, Type conversionType)
        {
            var type = Nullable.GetUnderlyingType(conversionType);
            if (type != null)
            {
                if (conversionType.IsGenericType)
                {
                    var definition = conversionType.GetGenericTypeDefinition();
                    if (definition != null && definition == typeof(Nullable<>))
                    {
                        var nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                        conversionType = nullableConverter.UnderlyingType;

                        if (type.IsEnum)
                        {
                            return Enum.Parse(conversionType, value.ToString());
                        }
                        else
                        {
                            return Convert.ChangeType(value, conversionType);
                        }


                    }
                }
            }


            if (conversionType.IsEnum)
            {
                if (Enum.IsDefined(conversionType, value))
                    return Enum.Parse(conversionType, value.ToString());
                else
                    throw new InvalidCastException($"Can not cast enum，enum:{conversionType}, value:{value}");
            }
            else
            {
                return Convert.ChangeType(value, conversionType);
            }
        }
    }
}
