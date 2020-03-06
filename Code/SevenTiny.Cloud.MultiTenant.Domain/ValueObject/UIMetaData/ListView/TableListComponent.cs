using Newtonsoft.Json;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView
{
    public class TableListComponent
    {
        [JsonProperty("biz_data", NullValueHandling = NullValueHandling.Ignore)]
        public List<Dictionary<string, FieldBizData>> BizData { get; set; }
        /// <summary>
        /// 业务行总数
        /// </summary>
        [JsonProperty("biz_data_total_count", NullValueHandling = NullValueHandling.Ignore)]
        public int BizDataTotalCount { get; set; }
        /// <summary>
        /// 页数，通过总数/页大小 计算取得，注意除数0的处理
        /// </summary>
        [JsonProperty("page_count")]
        public int PageCount { get; set; }
        /// <summary>
        /// 列信息
        /// </summary>
        [JsonProperty("sub_cmps")]
        public List<Column> Columns { get; set; }
    }
}
