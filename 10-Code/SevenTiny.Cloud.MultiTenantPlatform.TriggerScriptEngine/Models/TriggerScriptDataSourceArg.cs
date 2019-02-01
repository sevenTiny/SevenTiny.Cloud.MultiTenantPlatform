using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Models
{
    public class TriggerScriptDataSourceArg
    {
        public string operateCode { get; set; }
        public Dictionary<string, object> argumentsDic { get; set; }
    }
}
