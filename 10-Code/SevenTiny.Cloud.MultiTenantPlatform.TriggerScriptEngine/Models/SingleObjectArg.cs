using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Models
{
    public class SingleObjectArg
    {
        public string operateCode { get; set; }
        public SingleObjectComponent singleObjectComponent { get; set; }
    }
}
