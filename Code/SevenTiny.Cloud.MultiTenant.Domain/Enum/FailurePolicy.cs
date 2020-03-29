namespace SevenTiny.Cloud.MultiTenant.Domain.Enum
{
    /// <summary>
    /// 执行失败处理策略
    /// </summary>
    public enum OnFailurePolicy
    {
        Continue = 1,
        Break = 2
    }
    public static class FailurePolicyTranslator
    {
        public static string ToChinese(int scriptType)
        {
            switch (scriptType)
            {
                case (int)OnFailurePolicy.Continue: return "跳过脚本继续执行";
                case (int)OnFailurePolicy.Break: return "中断请求";
                default: return "未知";
            }
        }
    }
}
