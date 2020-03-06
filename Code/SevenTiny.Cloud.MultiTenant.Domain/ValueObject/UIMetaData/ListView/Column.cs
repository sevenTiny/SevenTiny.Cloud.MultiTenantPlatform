using Newtonsoft.Json;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView
{
    public class Column : BaseComponent
    {
        //
        // Summary:
        //     列数据
        [JsonProperty("cmp_data", Order = 1)]
        public ColumnCmpData CmpData { get; set; }
        //
        // Summary:
        //     编辑时的验证
        [JsonProperty("validators", Order = 1, NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, Dictionary<string, object>> Validators { get; set; }
    }
}
