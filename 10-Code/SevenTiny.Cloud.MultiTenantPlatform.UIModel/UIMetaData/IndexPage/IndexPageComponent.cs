using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.Enum;

namespace SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.IndexPage
{
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
        /// 搜索表单
        /// </summary>
        [JsonProperty("search_form", NullValueHandling = NullValueHandling.Ignore)]
        public SearchFormComponent SearchForm { get; set; }
        /// <summary>
        /// 按钮列表
        /// </summary>
        [JsonProperty("button_list", NullValueHandling = NullValueHandling.Ignore)]
        public ButtonComponent[] ButtonList { get; set; }
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
