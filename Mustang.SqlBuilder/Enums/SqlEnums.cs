namespace Mustang.BuilderSql
{
    /// <summary>
    /// SQL关系条件枚举
    /// </summary>
    public enum ConditionRelation
    {
        /// <summary>
        /// 不需要条件
        /// </summary>
        NULL,
        /// <summary>
        /// AND
        /// </summary>
        AND,
        /// <summary>
        /// OR
        /// </summary>
        OR
    }
    /// <summary>
    /// 数据筛选条件枚举
    /// </summary>
    public enum ConditionOperator
    {
        /// <summary>
        /// 等于
        /// </summary>
        EqualTo,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqualTo,
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan,
        /// <summary>
        /// 小于
        /// </summary>
        LessThan,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterThanOrEqualTo,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessThanOrEqualTo,
        /// <summary>
        /// 包含
        /// </summary>
        In,
        /// <summary>
        /// 不包含
        /// </summary>
        NotIn,
        /// <summary>
        /// 模糊查询
        /// </summary>
        Like,
        /// <summary>
        /// 
        /// </summary>
        NotLike,
        LeftLike,
        ReightLike,
        IsNull,
        IsNotNull//,
        //Between,
        //NotBetween
    }

    public enum OrderByEnums
    {
        ASC,
        DESC
    }
}
