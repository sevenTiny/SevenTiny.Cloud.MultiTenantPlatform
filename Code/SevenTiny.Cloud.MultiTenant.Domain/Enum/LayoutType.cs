namespace SevenTiny.Cloud.MultiTenant.Domain.Enum
{
    public enum LayoutType
    {
        /// <summary>
        /// 搜索表单和列表布局
        /// </summary>
        SearchForm_TableList = 1
    }

    public static class LayoutTypeTranslater
    {
        public static string ToDescription(LayoutType layoutType)
        {
            switch (layoutType)
            {
                case LayoutType.SearchForm_TableList:
                    return "搜索表单+列表布局";
                default:
                    break;
            }
            return string.Empty;
        }
    }
}
