namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Enum
{
    /// <summary>
    /// 触发点
    /// </summary>
    public enum TriggerPoint
    {
        Before,
        After
    }

    public static class TriggerPointTranslator
    {
        public static string ToLabel(int scriptType)
        {
            switch ((TriggerPoint)scriptType)
            {
                case TriggerPoint.Before: return "Before";
                case TriggerPoint.After: return "After";
                default: return "UnKnown";
            }
        }
    }
}
