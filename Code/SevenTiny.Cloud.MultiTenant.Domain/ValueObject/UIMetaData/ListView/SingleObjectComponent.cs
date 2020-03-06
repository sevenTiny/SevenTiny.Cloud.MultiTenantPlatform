using Newtonsoft.Json;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView
{
    public class SingleObjectComponent
    {
        [JsonProperty("biz_data", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, FieldBizData> BizData { get; set; }
        /// <summary>
        /// 列信息
        /// </summary>
        [JsonProperty("column_data", NullValueHandling = NullValueHandling.Ignore)]
        public List<Column> ColunmDatas { get; set; }
    }
}
