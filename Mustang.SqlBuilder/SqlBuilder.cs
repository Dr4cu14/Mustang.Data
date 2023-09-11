using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Mustang.BuilderSql;

namespace Mustang.SqlBuilder
{
    public abstract class SqlBuilder<T> where T : class, new()
    {
        public string Sql => SqlStatement.ToString();

        protected readonly EntityContext EntityContext;

        protected static string ParameterToken => "@";

        protected readonly StringBuilder SqlStatement = new StringBuilder();

        public readonly List<SqlParameter> SqlParameterList = new List<SqlParameter>();

        protected SqlBuilder(T entity = null)
        {
            EntityContext = EntityCache.GetEntityContext(entity);
        }

        public abstract SqlBuilder<T> ReturnId();

        public SqlBuilder<T> Insert()
        {
            var columnNames = new List<string>();
            var values = new List<string>();

            foreach (var propertyValue in EntityContext.PropertyValues)
            {
                if (propertyValue.PropertyValue == null)
                    continue;

                columnNames.Add(propertyValue.PropertyName);
                values.Add($"{ParameterToken}{propertyValue.PropertyName}");

                SqlParameterList.Add(new SqlParameter(propertyValue.PropertyName, propertyValue.PropertyValue));
            }

            SqlStatement.AppendLine($@"INSERT INTO {EntityContext.FullNameTableName} ({string.Join(",", columnNames)}) VALUES({string.Join(",", values)});");

            return this;
        }

        

        public SqlBuilder<T> Update()
        {
            var columnNames = new List<string>();

            foreach (var propertyValue in EntityContext.PropertyValues)
            {
                if (propertyValue.PropertyValue == null)
                    continue;

                columnNames.Add($"{propertyValue.PropertyName} = {ParameterToken}{propertyValue.PropertyName}");

                SqlParameterList.Add(new SqlParameter(propertyValue.PropertyName, propertyValue.PropertyValue));
            }

            SqlStatement.AppendLine($@"UPDATE {EntityContext.FullNameTableName} SET {string.Join(",", columnNames)}");

            return this;
        }

        public SqlBuilder<T> Delete()
        {
            SqlStatement.AppendLine($"DELETE FROM {EntityContext.FullNameTableName}");

            return this;
        }

        public SqlBuilder<T> Query()
        {
            var columnNames = new List<string>();

            foreach (var propertyValue in EntityContext.PropertyValues)
            {
                columnNames.Add(EntityContext.TableName + "." + propertyValue.PropertyName);
            }
            SqlStatement.AppendLine($@"SELECT {string.Join(",", columnNames)} FROM {EntityContext.FullNameTableName} {EntityContext.TableName}");

            return this;
        }

        public SqlBuilder<T> Query(List<string> selectColumnList)
        {
            SqlStatement.AppendLine($@"SELECT {string.Join(",", selectColumnList)} FROM {EntityContext.FullNameTableName} {EntityContext.TableName}");
            return this;
        }

        //public abstract SqlBuilder<T> Query(int pageIndex, int pageSize);

        //public abstract SqlBuilder<T> Query(int pageIndex, int pageSize, List<string> selectColumnList);

        public SqlBuilder<T> InnerJoin<TJoinTable, TRelationTable>(Expression<Func<TJoinTable, object>> joinExpression, ConditionOperator conditionOperator, Expression<Func<TRelationTable, object>> relationoinExpression)
        {
            return Join("INNER ", joinExpression, conditionOperator, relationoinExpression);
        }

        public SqlBuilder<T> LeftJoin<TJoinTable, TRelationTable>(Expression<Func<TJoinTable, object>> joinExpression, ConditionOperator conditionOperator, Expression<Func<TRelationTable, object>> relationoinExpression)
        {
            return Join("LEFT ", joinExpression, conditionOperator, relationoinExpression);
        }

        public SqlBuilder<T> RightJoin<TJoinTable, TRelationTable>(Expression<Func<TJoinTable, object>> joinExpression, ConditionOperator conditionOperator, Expression<Func<TRelationTable, object>> relationoinExpression)
        {
            return Join("RIGHT ", joinExpression, conditionOperator, relationoinExpression);
        }

        private SqlBuilder<T> Join<TJoinTable, TRelationTable>(string joinType, Expression<Func<TJoinTable, object>> joinExpression, ConditionOperator conditionOperator, Expression<Func<TRelationTable, object>> relationoinExpression)
        {
            var joinEntityContext = EntityHelper.GetEntityContext(default(TJoinTable));
            var joinExpressionProperty = GetJoinTablePropertyByExpress(joinEntityContext, joinExpression);

            var relationEntityContext = EntityHelper.GetEntityContext(default(TRelationTable));
            var relationTableProperty = GetJoinTablePropertyByExpress(relationEntityContext, relationoinExpression);

            SqlStatement.AppendLine($@"{joinType} JOIN {joinEntityContext.FullNameTableName} {joinEntityContext.TableName} ON {JoinConditionBuilder(joinEntityContext.TableName, joinExpressionProperty.PropertyName, conditionOperator, relationEntityContext.TableName, relationTableProperty.PropertyName)}; ");

            return this;
        }

        public SqlBuilder<T> WhereCondition(ConditionRelation conditionRelation, Expression<Func<T, object>> expr, ConditionOperator conditionOperator, object value)
        {
            if (SqlStatement.ToString().LastIndexOf("WHERE", StringComparison.Ordinal) == -1)
                SqlStatement.AppendLine("WHERE");

            if (conditionRelation != ConditionRelation.NULL)
                SqlStatement.Append($"{conditionRelation} ");

            var propertyValue = GetPropertyByExpress(EntityContext, expr);

            SqlStatement.AppendLine(ConditionBuilder(conditionOperator, EntityContext.TableName, propertyValue.PropertyName));

            SqlParameterList.Add(new SqlParameter(propertyValue.PropertyName, value));

            return this;
        }

        public SqlBuilder<T> WhereInCondition<TType>(ConditionRelation conditionRelation, Expression<Func<T, object>> expr, ConditionOperator conditionOperator, List<TType> value)
        {
            if (SqlStatement.ToString().LastIndexOf("WHERE", StringComparison.Ordinal) == -1)
                SqlStatement.AppendLine("WHERE");

            if (conditionRelation != ConditionRelation.NULL)
                SqlStatement.Append($"{conditionRelation} ");

            var propertyValue = GetPropertyByExpress(EntityContext, expr);

            SqlStatement.Append($" {propertyValue.PropertyName} IN (");

            for (var i = 0; i < value.Count; i++)
            {
                var filedName = ParameterToken + propertyValue.PropertyName + i;
                SqlStatement.Append(filedName);
                SqlParameterList.Add(new SqlParameter(filedName, value[i]));
            }

            SqlStatement.AppendLine(")");

            return this;
        }

        public SqlBuilder<T> WhereNotInCondition<TType>(ConditionRelation conditionRelation, Expression<Func<T, object>> expr, ConditionOperator conditionOperator, List<TType> value)
        {
            if (SqlStatement.ToString().LastIndexOf("WHERE", StringComparison.Ordinal) == -1)
                SqlStatement.AppendLine("WHERE");

            if (conditionRelation != ConditionRelation.NULL)
                SqlStatement.Append($"{conditionRelation} ");

            var propertyValue = GetPropertyByExpress(EntityContext, expr);

            SqlStatement.Append($" {propertyValue.PropertyName} NOT IN (");

            for (var i = 0; i < value.Count; i++)
            {
                var filedName = ParameterToken + propertyValue.PropertyName + i;
                SqlStatement.Append(filedName);
                SqlParameterList.Add(new SqlParameter(filedName, value[i]));
            }

            SqlStatement.AppendLine(")");

            return this;
        }

        public SqlBuilder<T> OrderBy(Expression<Func<T, object>> expr, OrderByEnums orderBy)
        {
            if (SqlStatement.ToString().LastIndexOf("ORDER BY", StringComparison.Ordinal) == -1)
                SqlStatement.Append("ORDER BY ");

            var propertyValue = GetPropertyByExpress(EntityContext, expr);

            SqlStatement.AppendLine($"{propertyValue.PropertyName} {orderBy}");

            return this;
        }

        public SqlBuilder<T> GroupBy(Expression<Func<T, object>> expr)
        {
            if (SqlStatement.ToString().LastIndexOf("GROUP BY", StringComparison.Ordinal) == -1)
                SqlStatement.Append("GROUP BY ");
            
            var propertyValue = GetPropertyByExpress(EntityContext, expr);

            SqlStatement.AppendLine($"{propertyValue.PropertyName}");

            return this;
        }

        public SqlBuilder<T> Builder()
        {
            //if (Sql.LastIndexOf(";", StringComparison.Ordinal) == -1)
            //
              SqlStatement.Append(";");

            return this;
        }

        private static EntityPropertyValue GetPropertyByExpress(EntityContext entityContext, Expression<Func<T, object>> expr)
        {
            var propertyName = "";
            if (expr.Body is UnaryExpression)
            {
                var uy = expr.Body as UnaryExpression;
                propertyName = (uy.Operand as MemberExpression).Member.Name;
            }
            else
            {
                propertyName = (expr.Body as MemberExpression).Member.Name;
            }
            var property = entityContext.PropertyValues.FirstOrDefault(m => m.PropertyName == propertyName);
            if (property == null)
                throw new Exception($"属性[{propertyName}],不存在");

            return property;
        }

        private static EntityPropertyValue GetJoinTablePropertyByExpress<TJoinTable>(EntityContext entityContext, Expression<Func<TJoinTable, object>> expression)
        {
            string propertyName;
            if (expression.Body is UnaryExpression)
            {
                var uy = expression.Body as UnaryExpression;
                propertyName = (uy.Operand as MemberExpression).Member.Name;
            }
            else
            {
                propertyName = (expression.Body as MemberExpression).Member.Name;
            }
            var property = entityContext.PropertyValues.FirstOrDefault(m => m.PropertyName == propertyName);
            if (property == null)
                throw new Exception($"Property[{propertyName}],不存在");

            return property;
        }

        public static string ConditionBuilder(ConditionOperator conditionOperator, string tableName, string filedName)
        {
            string condition = string.Empty;

            switch (conditionOperator)
            {
                case ConditionOperator.EqualTo:
                    condition = $"{tableName}.{filedName} = {ParameterToken}{tableName}_{filedName}";
                    break;
                case ConditionOperator.NotEqualTo:
                    condition = $"{tableName}.{filedName} <> {ParameterToken}{tableName}_{filedName}";
                    break;
                case ConditionOperator.GreaterThan:
                    condition = $"{tableName}.{filedName} > {ParameterToken}{tableName}_{filedName}";
                    break;
                case ConditionOperator.GreaterThanOrEqualTo:
                    condition = $"{tableName}.{filedName} >= {ParameterToken}{tableName}_{filedName}";
                    break;
                case ConditionOperator.LessThan:
                    condition = $"{tableName}.{filedName} < {ParameterToken}{tableName}_{filedName}";
                    break;
                case ConditionOperator.LessThanOrEqualTo:
                    condition = $"{tableName}.{filedName} <= {ParameterToken}{tableName}_{filedName}";
                    break;
                case ConditionOperator.Like:
                    condition = $"{tableName}.{filedName} LIKE {ParameterToken}{tableName}_{filedName}";
                    break;
                case ConditionOperator.LeftLike:
                    condition = $"{tableName}.{filedName} LIKE '{ParameterToken}{tableName}_{filedName}%'";
                    break;
                case ConditionOperator.ReightLike:
                    condition = $"{tableName}.{filedName} LIKE '%{ParameterToken}{tableName}_{filedName}'";
                    break;
                case ConditionOperator.NotLike:
                    condition = $"{tableName}.{filedName} NOT LIKE {ParameterToken}{tableName}_{filedName}";
                    break;
                case ConditionOperator.IsNull:
                    condition = $"{tableName}.{filedName} IS NULL";
                    break;
                case ConditionOperator.IsNotNull:
                    condition = $"{tableName}.{filedName} IS NOT NULL";
                    break;
            }
            return condition;
        }

        public static string JoinConditionBuilder(string joinTableName, string joinFiledName, ConditionOperator conditionOperator, string relationTableName, string relationFileName)
        {
            string condition = string.Empty;

            switch (conditionOperator)
            {
                case ConditionOperator.EqualTo:
                    condition = $" {joinTableName}.{joinFiledName} = {relationTableName}.{relationFileName}";
                    break;
                    //case ConditionOperator.NotEqualTo:
                    //    condition = $" {joinTableName}.{joinFiledName} <> {relationTableName}.{relationFileName}";
                    //    break;
                    //case ConditionOperator.GreaterThan:
                    //    condition = $" {joinTableName}.{joinFiledName} > {relationTableName}.{relationFileName}";
                    //    break;
                    //case ConditionOperator.GreaterThanOrEqualTo:
                    //    condition = $" {joinTableName}.{joinFiledName} >= {relationTableName}.{relationFileName}";
                    //    break;
                    //case ConditionOperator.LessThan:
                    //    condition = $" {joinTableName}.{joinFiledName} < {relationTableName}.{relationFileName}";
                    //    break;
                    //case ConditionOperator.LessThanOrEqualTo:
                    //    condition = $" {joinTableName}.{joinFiledName} <= {relationTableName}.{relationFileName}";
                    //    break;
            }
            return condition;
        }

    }
}
