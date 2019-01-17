using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity
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
