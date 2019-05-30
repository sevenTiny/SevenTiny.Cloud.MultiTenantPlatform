using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;

namespace SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject
{
    public class MetaFieldViewModel : MetaField
    {
        /// <summary>
        /// 组织字段的id
        /// </summary>
        public int FieldAggregationId { get; set; }
    }
}
