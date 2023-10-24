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


        public static DbType TransferToDataType(object value)
        {
            //字符及字符串
            if (value is char)
                return DbType.String;

            if (value is string)
                return DbType.String;

            //整型
            if (value is short)
                return DbType.Int16;

            if (value is int)
                return DbType.Int32;

            if (value is long)
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

            if (value is decimal)
                return DbType.Decimal;

            //布尔
            if (value is bool)
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

        public static DbType TransferToTDDataType(object value)
        {
            //字符及字符串
            //if (value is char)
            //    return DbType.String;

            if (value is string)
                return DbType.String;

            //整型
            if (value is short)
                return DbType.Int32;

            if (value is int)
                return DbType.Int32;

            if (value is long)
                return DbType.Int64;

            //if (value is UInt16)
            //    return DbType.Int32;

            //if (value is UInt32)
            //    return DbType.Int32;

            //if (value is UInt64)
            //    return DbType.Int32;

            ////浮点
            //if (value is Single)
            //    return DbType.Int32;

            //if (value is Double)
            //    return DbType.Int32;

            if (value is decimal)
                return DbType.Decimal;

            //布尔
            if (value is bool)
                return DbType.Int32;

            //时间
            if (value is DateTime)
                return DbType.String;

            //枚举
            if (value is Enum)
                return DbType.Int32;

            //Guid
            if (value is Guid)
                return DbType.String;

            throw new ApplicationException(@"Database Unsopported Value type " + value.GetType().Name);
        }
    }
}
