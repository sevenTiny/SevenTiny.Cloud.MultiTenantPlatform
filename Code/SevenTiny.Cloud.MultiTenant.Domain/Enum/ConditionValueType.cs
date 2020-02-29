namespace SevenTiny.Cloud.MultiTenant.Domain.Enum
{
    /// <summary>
    /// 条件值类型
    /// </summary>
    public enum ConditionValueType
    {
        /// <summary>
        /// 参数传递
        /// </summary>
        Parameter = 0,
        /// <summary>
        /// 常量（固定的数值）
        /// </summary>
        Const = 1
    }

    public static class ConditionValueTypeTranslator
    {
        public static string ToLabel(int type)
        {
            switch (type)
            {
                case (int)ConditionValueType.Const:
                    return "固定值";
                case (int)ConditionValueType.Parameter:
                    return "参数传递值";
                default:
                    return string.Empty;
            }
        }
    }
}
