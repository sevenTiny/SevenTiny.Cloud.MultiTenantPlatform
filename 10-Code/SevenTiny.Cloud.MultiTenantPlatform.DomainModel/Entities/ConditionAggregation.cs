using SevenTiny.Bantina.Bankinate;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    /// <summary>
    /// 条件字段
    /// </summary>
    [Table]
    public class ConditionAggregation : CommonInfo
    {
        /// <summary>
        /// 标识是属于哪个Condition
        /// </summary>
        [Column]
        public int InterfaceSearchConditionId { get; set; }
        [Column]
        public string FieldCode { get; set; }
        [Column]
        public string FieldName { get; set; }
        [Column]
        public int ConditionType { get; set; }
        [Column]
        public string Value { get; set; }
    }
}
