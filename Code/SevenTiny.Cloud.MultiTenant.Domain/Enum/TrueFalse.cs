namespace SevenTiny.Cloud.MultiTenant.Domain.Enum
{
    /// <summary>
    /// TrueFalse枚举
    /// </summary>
    public enum TrueFalse : int
    {
        False = 0,
        True = 1
    }
    public static class TrueFalseTranslator
    {
        public static string ToLabel(int trueFalse)
        {
            switch (trueFalse)
            {
                case (int)TrueFalse.False:
                    return "否";
                case (int)TrueFalse.True:
                    return "是";
                default:
                    return "UnKnown";
            }
        }
        public static bool ToBoolean(int trueFale)
        {
            switch (trueFale)
            {
                case (int)TrueFalse.True: return true;
                case (int)TrueFalse.False: return false;
                default: return false;
            }
        }
    }
}
