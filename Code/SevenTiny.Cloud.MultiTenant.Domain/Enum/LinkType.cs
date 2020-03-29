namespace SevenTiny.Cloud.MultiTenant.Domain.Enum
{
    /// <summary>
    /// 链接打开方式
    /// </summary>
    public enum LinkType
    {
        /// <summary>
        /// 当前页打开
        /// </summary>
        //Self = 1,
        /// <summary>
        /// 框架内打开
        /// </summary>
        Frame = 2,
        /// <summary>
        /// 新页面打开
        /// </summary>
        Blank = 3
    }
    public static class LinkTypeTranslator
    {
        public static string ToChinese(int linkType)
        {
            switch ((LinkType)linkType)
            {
                case LinkType.Frame:
                    return "框架内打开";
                case LinkType.Blank:
                    return "新页面打开";
                default:
                    return string.Empty;
            }
        }
    }
}
