using Newtonsoft.Json;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity
{
    public class SingleObjectComponent
    {
        [JsonProperty("biz_data", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, FieldBizData> BizData { get; set; }
    }
}
