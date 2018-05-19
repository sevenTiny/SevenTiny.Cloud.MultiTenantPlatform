using SevenTiny.Bantina.Bankinate;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    public class MetaObject:EntityInfo
    {
        [Column]
        public int ApplicationId { get; set; }
    }
}
