using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject
{
    public class MetaFieldViewModel : MetaField
    {
        /// <summary>
        /// 组织字段的id
        /// </summary>
        public int FieldAggregationId { get; set; }
    }
}
