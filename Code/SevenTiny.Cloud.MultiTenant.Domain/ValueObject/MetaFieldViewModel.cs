using SevenTiny.Cloud.MultiTenant.Domain.Entity;

namespace SevenTiny.Cloud.MultiTenant.Domain.ValueObject
{
    public class MetaFieldViewModel : MetaField
    {
        /// <summary>
        /// 组织字段的id
        /// </summary>
        public int FieldAggregationId { get; set; }
    }
}
