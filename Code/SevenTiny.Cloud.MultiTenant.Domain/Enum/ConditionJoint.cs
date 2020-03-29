namespace SevenTiny.Cloud.MultiTenant.Domain.Enum
{
    /// <summary>
    /// 条件连接符
    /// </summary>
    public enum ConditionJoint
    {
        /// <summary>
        /// &&
        /// </summary>
        And = 1,
        /// <summary>
        /// ||
        /// </summary>
        Or = 2
    }

    public static class ConditionJointTranslator
    {
        public static string ToLabelWithExplain(int type)
        {
            switch (type)
            {
                case (int)ConditionJoint.And:
                    return "And（与）";
                case (int)ConditionJoint.Or:
                    return "Or（或）";
                default:
                    return string.Empty;
            }
        }
        public static string ToLabel(int type)
        {
            switch (type)
            {
                case (int)ConditionJoint.And:
                    return "And";
                case (int)ConditionJoint.Or:
                    return "Or";
                default:
                    return string.Empty;
            }
        }
    }
}
