using Newtonsoft.Json;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.IndexPage
{
    /// <summary>
    /// 搜索表单
    /// </summary>
    public class SearchFormComponent
    {
        /// <summary>
        /// 搜索项
        /// </summary>
        [JsonProperty("search_items", NullValueHandling = NullValueHandling.Ignore)]
        public SearchItem[] SearchItems { get; set; }
    }

    /// <summary>
    /// 搜索项
    /// </summary>
    public class SearchItem
    {
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public object Value { get; set; }
        [JsonProperty("value_type")]
        public int ValueType { get; set; }
        [JsonProperty("visible")]
        public bool Visible { get; set; }
    }
}
