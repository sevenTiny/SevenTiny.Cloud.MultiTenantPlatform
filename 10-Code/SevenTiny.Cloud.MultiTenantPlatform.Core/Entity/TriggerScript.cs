using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    public class TriggerScript : MetaObjectManageInfo
    {
        [Column]
        public string Script { get; set; }
        [Column]
        public int ScriptType { get; set; }
        [Column]
        public int FailurePolicy { get; set; }
    }
}
