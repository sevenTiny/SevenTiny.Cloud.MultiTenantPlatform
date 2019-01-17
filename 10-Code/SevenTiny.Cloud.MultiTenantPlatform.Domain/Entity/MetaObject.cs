using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity
{
    public class MetaObject : CommonInfo
    {
        [Column]
        public int ApplicationId { get; set; }
    }
}
