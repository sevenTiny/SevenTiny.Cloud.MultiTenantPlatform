namespace SevenTiny.Cloud.MultiTenant.Domain.Enum
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

    public static class ConditionTypeTranslator
    {
        public static string ToLabelWithExplain(int type)
        {
            switch (type)
            {
                case (int)ConditionType.Equal:
                    return "==（等于）";
                case (int)ConditionType.GreaterThan:
                    return ">（大于）";
                case (int)ConditionType.GreaterThanEqual:
                    return ">=（大于等于）";
                case (int)ConditionType.LessThan:
                    return "<（小于）";
                case (int)ConditionType.LessThanEqual:
                    return "<=（小于等于）";
                case (int)ConditionType.NotEqual:
                    return "!=（不等于）";
                default:
                    return string.Empty;
            }
        }
        public static string ToLabel(int type)
        {
            switch (type)
            {
                case (int)ConditionType.Equal:
                    return "==";
                case (int)ConditionType.GreaterThan:
                    return ">";
                case (int)ConditionType.GreaterThanEqual:
                    return ">=";
                case (int)ConditionType.LessThan:
                    return "<";
                case (int)ConditionType.LessThanEqual:
                    return "<=";
                case (int)ConditionType.NotEqual:
                    return "!=";
                default:
                    return string.Empty;
            }
        }
    }
}
