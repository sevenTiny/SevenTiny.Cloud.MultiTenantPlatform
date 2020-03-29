using Newtonsoft.Json;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData
{
    public class BaseComponent
    {
        //
        // Summary:
        //     组件ID
        [JsonProperty("cmp_id", NullValueHandling = NullValueHandling.Ignore, Order = -10)]
        public virtual string CmpId { get; set; }
        //
        // Summary:
        //     组件名
        [JsonProperty("cmp_name", NullValueHandling = NullValueHandling.Ignore, Order = -9)]
        public virtual string CmpName { get; set; }
        //
        // Summary:
        //     组件类型 CmpType 符合组件参照ComponentType
        [JsonProperty("cmp_type", NullValueHandling = NullValueHandling.Ignore, Order = -8)]
        public string CmpType { get; set; }
    }
}
