﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mustang.SqlBuilder
{
    public class MsSqlBuilder<T> : SqlBuilder<T> where T : class, new()
    {
        public MsSqlBuilder(T entity) : base(entity)
        {

        }

        public override SqlBuilder<T> ReturnId()
        {
            SqlStatement.AppendLine("SELECT SCOPE_IDENTITY();");
            return this;
        }

        //public SqlBuilder<T> WithNoLock()
        //{
        //    SqlStatement.AppendLine(" ");
        //    return this;
        //}
        
        //public override SqlBuilder<T> Query(int pageIndex, int pageSize)
        //{
        //    var columnNames = new List<string>();

        //    foreach (var propertyValue in EntityContext.PropertyValues)
        //    {
        //        columnNames.Add(EntityContext.TableName + "." + propertyValue.PropertyName);
        //    }

        //    SqlStatement.AppendLine($@"SELECT COUNT(0) FROM {EntityContext.FullNameTableName} {EntityContext.TableName} #WHERE#");

        //    SqlStatement.AppendLine($@"SELECT {string.Join(",", columnNames)} FROM {EntityContext.FullNameTableName} {EntityContext.TableName} #WHERE#");

        //    SqlStatement.AppendLine($"ORDER BY #Sort# OFFSET @StartNumber ROWS FETCH NEXT (@EndNumber - @StartNumber) ROWS ONLY");

        //    SqlParameterList.Add(new SqlParameter("@StartNumber", pageIndex * pageSize));
        //    SqlParameterList.Add(new SqlParameter("@EndNumber", pageSize + (pageIndex * pageSize)));

        //    return this;
        //}

        //public override SqlBuilder<T> Query(int pageIndex, int pageSize, List<string> selectColumnList)
        //{

        //    SqlStatement.AppendLine($@"SELECT COUNT(0) FROM {EntityContext.FullNameTableName} {EntityContext.TableName} #WHERE#");

        //    SqlStatement.AppendLine($@"SELECT {string.Join(",", selectColumnList)} FROM {EntityContext.FullNameTableName} {EntityContext.TableName} #WHERE#");

        //    SqlStatement.AppendLine($"ORDER BY #Sort# OFFSET @StartNumber ROWS FETCH NEXT (@EndNumber - @StartNumber) ROWS ONLY");

        //    SqlParameterList.Add(new SqlParameter("@StartNumber", pageIndex * pageSize));
        //    SqlParameterList.Add(new SqlParameter("@EndNumber", pageSize + (pageIndex * pageSize)));

        //    return this;
        //}
    }
}