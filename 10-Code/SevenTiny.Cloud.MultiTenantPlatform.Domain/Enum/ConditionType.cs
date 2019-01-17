namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum
{
    /// <summary>
    /// 搜索条件类型
    /// </summary>
    public enum ConditionType : int
    {
        /// <summary>
        /// ==
        /// </summary>
        Equal = 0,
        /// <summary>
        /// !=
        /// </summary>
        NotEqual = 1,
        /// <summary>
        /// >
        /// </summary>
        GreaterThan = 2,
        /// <summary>
        /// <
        /// </summary>
        LessThan = 3,
        /// <summary>
        /// >=
        /// </summary>
        GreaterThanEqual = 4,
        /// <summary>
        /// <=
        /// </summary>
        LessThanEqual = 5
    }
}
