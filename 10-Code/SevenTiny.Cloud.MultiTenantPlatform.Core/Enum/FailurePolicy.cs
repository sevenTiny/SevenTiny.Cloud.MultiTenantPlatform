namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Enum
{
    /// <summary>
    /// 执行失败处理策略
    /// </summary>
    public enum FailurePolicy
    {
        Continue = 1,
        Break = 2
    }
    public static class FailurePolicyTranslator
    {
        public static string ToCode(int scriptType)
        {
            switch (scriptType)
            {
                case (int)FailurePolicy.Continue: return "Continue";
                case (int)FailurePolicy.Break: return "Break";
                default: return "UnKnown";
            }
        }
        public static string ToChinese(int scriptType)
        {
            switch (scriptType)
            {
                case (int)FailurePolicy.Continue: return "跳过脚本继续执行";
                case (int)FailurePolicy.Break: return "中断请求";
                default: return "未知";
            }
        }
    }
}
