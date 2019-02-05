using Newtonsoft.Json;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity
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
        /// 列信息
        /// </summary>
        [JsonProperty("column_data", NullValueHandling = NullValueHandling.Ignore)]
        public List<ColunmData> ColunmDatas { get; set; }
    }
}
