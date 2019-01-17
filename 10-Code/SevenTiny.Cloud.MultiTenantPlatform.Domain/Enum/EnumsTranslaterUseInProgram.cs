namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum
{
    /// <summary>
    /// 枚举翻译器,在程序内部使用，不对外翻译
    /// </summary>
    public class EnumsTranslaterUseInProgram
    {
        public static string Tran_ConditionJoint(int type)
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
        public static string Tran_ConditionType(int type)
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
