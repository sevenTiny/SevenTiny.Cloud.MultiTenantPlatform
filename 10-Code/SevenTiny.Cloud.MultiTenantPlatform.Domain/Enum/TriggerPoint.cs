namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum
{
    /// <summary>
    /// 触发点
    /// </summary>
    public enum TriggerPoint
    {
        Before = 1,
        After = 2
    }
    public static class TriggerPointTranslator
    {
        public static string ToCode(int scriptType)
        {
            switch (scriptType)
            {
                case (int)TriggerPoint.Before: return "Before";
                case (int)TriggerPoint.After: return "After";
                default: return "UnKnown";
            }
        }
    }
}
