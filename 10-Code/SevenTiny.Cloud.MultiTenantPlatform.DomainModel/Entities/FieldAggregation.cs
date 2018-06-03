using SevenTiny.Bantina.Bankinate;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    [Table]
    public class FieldAggregation
    {
        [Column]
        public int InterfaceFieldId { get; set; }
        [Column]
        public int MetaFieldId { get; set; }
    }
}
