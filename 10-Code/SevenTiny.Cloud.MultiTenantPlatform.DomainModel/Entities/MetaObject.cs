using SevenTiny.Bantina.Bankinate;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    public class MetaObject : CommonInfo
    {
        [Column]
        public int ApplicationId { get; set; }
    }
}
