using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity
{
    [Table]
    public class FieldListAggregation
    {
        [Column]
        public int FieldListId { get; set; }
        [Column]
        public int MetaFieldId { get; set; }
    }
}
