using System;
using System.Collections.Generic;
using System.Data;

namespace Mustang.SqlBuilder
{
    /// <summary>
    /// 转换SQL参数类型为DBType
    /// </summary>
    public class Transfer2DbType
    {
        public static T[] Transfer<T>(IList<T> value)
        {
            T[] arr = null;
            if (value != null)
            {
                if (value.Count > 0)
                {
                    arr = new T[value.Count];
                    value.CopyTo(arr, 0);
                }
            }
            return arr;
        }


        public static DbType TransferToDataType(Object value)
        {
            //字符及字符串
            if (value is Char)
                return DbType.String;

            if (value is String)
                return DbType.String;

            //整型
            if (value is Int16)
                return DbType.Int16;

            if (value is Int32)
                return DbType.Int32;

            if (value is Int64)
                return DbType.Int64;

            if (value is UInt16)
                return DbType.UInt16;

            if (value is UInt32)
                return DbType.UInt32;

            if (value is UInt64)
                return DbType.UInt64;

            //浮点
            if (value is Single)
                return DbType.Single;

            if (value is Double)
                return DbType.Double;

            if (value is Decimal)
                return DbType.Decimal;

            //布尔
            if (value is  Boolean)
                return DbType.Boolean;

            //时间
            if (value is DateTime)
                return DbType.DateTime;

            //枚举
            if (value is Enum)
                return DbType.Int32;

            //Guid
            if (value is Guid)
                return DbType.Guid;

            throw new ApplicationException(@"Database Unsopported Value type " + value.GetType().Name);
        }
    }
}
