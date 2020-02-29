namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
{
    /// <summary>
    /// 搜索条件实体
    /// </summary>
    public class SearchData
    {
        /// <summary>
        /// 搜索视图编码
        /// </summary>
        public string SearchView { get; set; }
        /// <summary>
        /// 搜索条件项
        /// </summary>
        public SearchItem[] Items { get; set; }
    }
    public class SearchItem
    {
        /// <summary>
        /// 字段编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 字段显示名称
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 字段值
        /// </summary>
        public object Value { get; set; }
    }
}
