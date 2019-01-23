using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity
{
    public class TableListComponent
    {
        [JsonProperty("biz_data", NullValueHandling = NullValueHandling.Ignore, Order = 3)]
        public List<Dictionary<string, FieldBizData>> BizData { get; set; }
    }
}
