using System.Collections.Generic;
using System.Text.RegularExpressions;
using Mustang.Map;

namespace Mustang.SqlBuilder
{
     public class EntityHelper
    {

        /// <summary>
        /// 获取实体代表的表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TableNameAttribute GetTableName<T>()
        {
            var tablename = typeof(T).GetCustomAttributes(typeof(TableNameAttribute), true);
            return (TableNameAttribute)tablename[0];
        }

        /// <summary>
        /// 获取DTO字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EntityPropertyValue> GetEntityFields<T>(T entity)
        {
            var properties = typeof(T).GetProperties();

            var propertyValues = new List<EntityPropertyValue>();

            foreach (var propertie in properties)
            {
                var columnName = FilterField(propertie.Name);

                object value = null;
                if (entity != null)
                    value = propertie.GetValue(entity);

                propertyValues.Add(new EntityPropertyValue(columnName, value));
            }

            return propertyValues;
        }

        /// <summary>
        /// 从Enity中反射获取自定义特性上的数据库表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static EntityContext GetEntityContext<T>(T entity)
        {

            var tableNames = GetTableName<T>();
            var entityContext = new EntityContext
            {
                TableName = tableNames.TableName.ToLower(),
                FullNameTableName = tableNames.FullNameTableName,
                PropertyValues = GetEntityFields(entity)
            };
            return entityContext;
        }

        private static string FilterField(string fieldName)
        {
            var match = Regex.Match(fieldName, "<(.*?)>");
            return match.Success ? match.Groups[1].Value : fieldName;
        }

        ///// <summary>
        ///// 获取Entity实体中的字段
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="isFullName">true：字段名前面包含表名</param>
        ///// <returns></returns>
        //public static List<string> GetTableNameconnectFields<T>(bool isFullName)
        //{
        //    var fields = typeof(T).GetProperties();
        //    var result = new List<string>();
        //    if (isFullName)
        //    {
        //        var tablename = EntityHelper.GetTableName<T>();
        //        result.AddRange(fields.Select(i => tablename + "." + i.Name));
        //        return result;
        //    }
        //    result.AddRange(fields.Select(i => i.Name));
        //    return result;
        //}

        ///// <summary>
        ///// 获取实体中的字段，包括表名，使用","连接
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public static string GetFiledString<T>()
        //{
        //    var list = GetFields<T>(true).ToArray();
        //    var result = string.Join(",", list);
        //    return result;
        //}



        //public static string GetTableName(Type entityType)
        //{
        //    try
        //    {
        //        var tablename = entityType.GetCustomAttributes(typeof(TableNameAttribute), true);
        //        return ((TableNameAttribute)tablename[0]).TableName;
        //    }
        //    catch
        //    {
        //        throw new Exception("没有配置TableName特性！");
        //    }

        //}

        ///// <summary>
        ///// 获取实体主键名称
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public static string GetPrimaryKey<T>()
        //{
        //    var primary = typeof(T).GetCustomAttributes(typeof(PrimaryAttribute), true);
        //    var pri = typeof(T).GetProperties();
        //    foreach (var i in pri)
        //    {
        //        var pris = i.GetCustomAttributes(typeof(PrimaryAttribute), true);
        //        if (pris.Any())
        //        {
        //            return i.Name;
        //        }
        //    }
        //    return "";
        //}
    }
}
