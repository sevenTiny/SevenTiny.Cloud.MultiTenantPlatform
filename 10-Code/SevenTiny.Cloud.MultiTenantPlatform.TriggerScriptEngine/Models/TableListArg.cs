using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;

namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Models
{
    public class TableListArg
    {
        public string operateCode { get; set; }
        public TableListComponent tableListComponent { get; set; }
    }
}
