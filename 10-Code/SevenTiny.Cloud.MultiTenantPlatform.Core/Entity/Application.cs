using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    [Table]
    public class Application: CommonInfo
    {
        [Column]
        public string Icon { get; set; }
    }
}
