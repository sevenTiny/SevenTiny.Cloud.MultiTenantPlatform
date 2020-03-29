using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.IndexPage
{
    /// <summary>
    /// 视图组件
    /// </summary>
    [JsonObject("indexpage_component")]
    public class IndexPageComponent
    {
        /// <summary>
        /// 视图页标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 视图页标题配的图标
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }
        /// <summary>
        /// 搜索表单组件
        /// </summary>
        [JsonProperty("search_form", NullValueHandling = NullValueHandling.Ignore)]
        public SearchFormComponent SearchFormComponent { get; set; }
        /// <summary>
        /// 按钮组件集合
        /// </summary>
        [JsonProperty("button_components", NullValueHandling = NullValueHandling.Ignore)]
        public ButtonComponent[] ButtonComponents { get; set; }
        /// <summary>
        /// 列表组件
        /// </summary>
        [JsonProperty("listview_component", NullValueHandling = NullValueHandling.Ignore)]
        public ListViewComponent ListViewComponent { get; set; }
        /// <summary>
        /// 布局类型
        /// </summary>
        [JsonProperty("layout_type")]
        public int LayoutType { get; set; }
        /// <summary>
        /// 布局描述
        /// </summary>
        [JsonProperty("layout_description")]
        public string LayoutDescription => LayoutTypeTranslater.ToDescription((LayoutType)LayoutType);
    }
}
