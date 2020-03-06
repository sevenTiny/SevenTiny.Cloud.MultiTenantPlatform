using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView
{
    /// <summary>
    /// 视图组件中返回的列表信息
    /// </summary>
    public class ListViewComponent
    {
        /// <summary>
        /// 编码
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
