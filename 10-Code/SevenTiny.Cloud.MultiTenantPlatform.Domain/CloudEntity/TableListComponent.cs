using Newtonsoft.Json;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity
{
    public class TableListComponent
    {
        [JsonProperty("biz_data", NullValueHandling = NullValueHandling.Ignore, Order = 3)]
        public List<Dictionary<string, FieldBizData>> BizData { get; set; }
        /// <summary>
        /// 业务行总数
        /// </summary>
        [JsonProperty("biz_data_total_count", NullValueHandling = NullValueHandling.Ignore, Order = 3)]
        public int BizDataTotalCount { get; set; }
    }
}
