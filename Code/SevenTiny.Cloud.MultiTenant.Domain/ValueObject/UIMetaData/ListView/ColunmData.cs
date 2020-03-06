using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView
{
    /// <summary>
    /// 列信息数据
    /// </summary>
    public class ColunmData
    {
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty("name", Order = 1)]
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [JsonProperty("text", Order = 2)]
        public string Text { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        [JsonProperty("field_type", Order = 3)]
        public int FieldType { get; set; }
        /// <summary>
        /// 字段长度
        /// </summary>
        [JsonProperty("field_length", Order = 4)]
        public int FieldLength { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        [JsonProperty("is_visible", Order = 5)]
        public bool IsVisible { get; set; }
        /// <summary>
        /// 是否是链接
        /// </summary>
        [JsonProperty("is_url", Order = 6)]
        public bool IsUrl { get; set; }
    }
}
