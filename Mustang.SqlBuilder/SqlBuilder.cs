using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Mustang.BuilderSql;

namespace Mustang.SqlBuilder
{
    public abstract class SqlBuilder<T> : ISqlBuilder where T : class
    {
        public string Sql { get; set; } = string.Empty;

        public List<SqlParameter> SqlParameters { get; set; } = new();

        protected EntityContext EntityContext;

        protected static string ParameterToken => "@";

        protected StringBuilder Statement = new();

        public virtual SqlBuilder<T> ReturnId()
        {
            return this;
        }

        public SqlBuilder<T> Exists(string columnName)
        {
            Statement.AppendLine($"SELECT {EntityContext.TableName}.{columnName} FROM {EntityContext.FullNameTableName}");

            return this;
        }

        public virtual SqlBuilder<T> Insert(T entity)
        {
            EntityContext = EntityHelper.GetEntityContext(entity);

            var columnNames = new List<string>();
            var values = new List<string>();

            foreach (var propertyValue in EntityContext.PropertyValues)
            {
                if (propertyValue.PropertyValue == null)
                    continue;

                columnNames.Add(propertyValue.PropertyName);
                values.Add($"{ParameterToken}{propertyValue.PropertyName}");

                SqlParameters.Add(new SqlParameter(propertyValue.PropertyName, propertyValue.PropertyValue));
            }

            Statement.AppendLine($"INSERT INTO {EntityContext.FullNameTableName} ({string.Join(",", columnNames)}) VALUES({string.Join(",", values)});");

            return this;
        }

        public SqlBuilder<T> Update(T entity)
        {
            EntityContext = EntityHelper.GetEntityContext(entity);

            var columnNames = new List<string>();

            foreach (var propertyValue in EntityContext.PropertyValues)
            {
                if (propertyValue.PropertyValue == null)
                    continue;

                columnNames.Add($"{propertyValue.PropertyName} = {ParameterToken}{propertyValue.PropertyName}");

                SqlParameters.Add(new SqlParameter(propertyValue.PropertyName, propertyValue.PropertyValue));
            }

            Statement.AppendLine($"UPDATE {EntityContext.FullNameTableName} SET {string.Join(",", columnNames)}");

            return this;
        }

        public SqlBuilder<T> Delete(T entity)
        {
            EntityContext = EntityHelper.GetEntityContext(entity);

            Statement.AppendLine($"DELETE FROM {EntityContext.FullNameTableName}");

            return this;
        }

        public SqlBuilder<T> Query()
        {
            EntityContext = EntityHelper.GetEntityContext(default(T));

            var columnNames = new List<string>();

            foreach (var propertyValue in EntityContext.PropertyValues)
            {
                columnNames.Add($"{EntityContext.TableName}.{propertyValue.PropertyName}");
            }

            Statement.AppendLine($"SELECT {string.Join(",", columnNames)} FROM {EntityContext.FullNameTableName} {EntityContext.TableName}");

            return this;
        }

        public SqlBuilder<T> Query(string selectColumn)
        {
            EntityContext = EntityHelper.GetEntityContext(default(T));

            Statement.AppendLine($@"SELECT {selectColumn} FROM {EntityContext.FullNameTableName} {EntityContext.TableName}");

            return this;
        }

        public SqlBuilder<T> Query(List<string> selectColumnList)
        {
            EntityContext = EntityHelper.GetEntityContext(default(T));

            Statement.AppendLine($@"SELECT {string.Join(",", selectColumnList)} FROM {EntityContext.FullNameTableName} {EntityContext.TableName}");

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

            Statement.AppendLine($@"{joinType} JOIN {joinEntityContext.FullNameTableName} {joinEntityContext.TableName} ON {JoinConditionBuilder(joinEntityContext.TableName, joinExpressionProperty.PropertyName, conditionOperator, relationEntityContext.TableName, relationTableProperty.PropertyName)} ");

            return this;
        }

        public SqlBuilder<T> JoinCondition<TJoinTable, TRelationTable>(Expression<Func<TJoinTable, object>> joinExpression, ConditionOperator conditionOperator, Expression<Func<TRelationTable, object>> relationoinExpression)
        {
            var joinEntityContext = EntityHelper.GetEntityContext(default(TJoinTable));
            var joinExpressionProperty = GetJoinTablePropertyByExpress(joinEntityContext, joinExpression);

            var relationEntityContext = EntityHelper.GetEntityContext(default(TRelationTable));
            var relationTableProperty = GetJoinTablePropertyByExpress(relationEntityContext, relationoinExpression);

            Statement.AppendLine($" {ConditionRelation.AND} {JoinConditionBuilder(joinEntityContext.TableName, joinExpressionProperty.PropertyName, conditionOperator, relationEntityContext.TableName, relationTableProperty.PropertyName)}");

            return this;
        }


        public SqlBuilder<T> WhereCondition(ConditionRelation conditionRelation, Expression<Func<T, object>> expr, ConditionOperator conditionOperator, object value = null)
        {
            if (conditionRelation == ConditionRelation.NULL)
                Statement.AppendLine(" WHERE ");

            if (conditionRelation != ConditionRelation.NULL)
                Statement.AppendLine($"{conditionRelation} ");

            var propertyValue = GetPropertyByExpress(EntityContext, expr);

            Statement.AppendLine(ConditionBuilder(conditionOperator, EntityContext.TableName, propertyValue.PropertyName));

            if (value != null)
            {
                var filedName = $"{ParameterToken}{EntityContext.TableName}_{propertyValue.PropertyName}";
                SqlParameters.Add(new SqlParameter(filedName, value));
            }

            return this;
        }

        public SqlBuilder<T> WhereCondition<TTable>(ConditionRelation conditionRelation, Expression<Func<TTable, object>> expr, ConditionOperator conditionOperator, object value = null)
        {
            if (conditionRelation == ConditionRelation.NULL)
                Statement.AppendLine(" WHERE ");

            if (conditionRelation != ConditionRelation.NULL)
                Statement.AppendLine($"{conditionRelation} ");

            var relationEntityContext = EntityHelper.GetEntityContext(default(TTable));
            var propertyValue = GetJoinTablePropertyByExpress(relationEntityContext, expr);

            Statement.AppendLine(ConditionBuilder(conditionOperator, relationEntityContext.TableName, propertyValue.PropertyName));

            if (value != null)
            {
                var filedName = $"{ParameterToken}{relationEntityContext.TableName}_{propertyValue.PropertyName}";
                SqlParameters.Add(new SqlParameter(filedName, value));
            }

            return this;
        }

        public SqlBuilder<T> WhereInCondition(ConditionRelation conditionRelation, Expression<Func<T, object>> expr, List<object> values)
        {
            if (Statement.ToString().LastIndexOf("WHERE", StringComparison.Ordinal) == -1)
                Statement.AppendLine("WHERE");

            if (conditionRelation != ConditionRelation.NULL)
                Statement.AppendLine($"{conditionRelation} ");

            var propertyValue = GetPropertyByExpress(EntityContext, expr);

            List<string> parameter = new List<string>();

            for (var i = 0; i < values.Count; i++)
            {
                var filedName = $"{ParameterToken}{EntityContext.TableName}_{propertyValue.PropertyName}{i}";
                parameter.Add(filedName);
                SqlParameters.Add(new SqlParameter(filedName, values[i]));
            }

            Statement.AppendLine($" {propertyValue.PropertyName} IN ({string.Join(",", parameter)})");

            return this;
        }

        public SqlBuilder<T> WhereNotInCondition(ConditionRelation conditionRelation, Expression<Func<T, object>> expr, List<object> values)
        {
            if (Statement.ToString().LastIndexOf("WHERE", StringComparison.Ordinal) == -1)
                Statement.AppendLine("WHERE");

            if (conditionRelation != ConditionRelation.NULL)
                Statement.AppendLine($"{conditionRelation} ");

            var propertyValue = GetPropertyByExpress(EntityContext, expr);
            var parameter = new List<string>();

            for (var i = 0; i < values.Count; i++)
            {
                var filedName = $"{ParameterToken}{EntityContext.TableName}_{propertyValue.PropertyName}{i}";
                parameter.Add(filedName);
                SqlParameters.Add(new SqlParameter(filedName, values[i]));
            }

            Statement.AppendLine($" {propertyValue.PropertyName} NOT IN ({string.Join(",", parameter)})");

            return this;
        }

        public SqlBuilder<T> OrderBy(Expression<Func<T, object>> expr, OrderByEnums orderBy)
        {
            var propertyValue = GetPropertyByExpress(EntityContext, expr);

            Statement.AppendLine($"ORDER BY {propertyValue.PropertyName} {orderBy}");

            return this;
        }

        public SqlBuilder<T> Builder()
        {
            Sql = Statement.ToString();

            if (Sql.LastIndexOf(";", StringComparison.Ordinal) == -1)
                Sql += ";";




            return this;
        }

        private static EntityPropertyValue GetPropertyByExpress(EntityContext entityContext, Expression<Func<T, object>> expr)
        {
            var propertyName = string.Empty;
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
            var propertyName = string.Empty;
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
                case ConditionOperator.NotEqualTo:
                    condition = $" {joinTableName}.{joinFiledName} <> {relationTableName}.{relationFileName}";
                    break;
                case ConditionOperator.GreaterThan:
                    condition = $" {joinTableName}.{joinFiledName} > {relationTableName}.{relationFileName}";
                    break;
                case ConditionOperator.GreaterThanOrEqualTo:
                    condition = $" {joinTableName}.{joinFiledName} >= {relationTableName}.{relationFileName}";
                    break;
                case ConditionOperator.LessThan:
                    condition = $" {joinTableName}.{joinFiledName} < {relationTableName}.{relationFileName}";
                    break;
                case ConditionOperator.LessThanOrEqualTo:
                    condition = $" {joinTableName}.{joinFiledName} <= {relationTableName}.{relationFileName}";
                    break;
            }
            return condition;
        }
    }
}
