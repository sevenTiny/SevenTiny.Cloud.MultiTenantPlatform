using SevenTiny.Bantina.Bankinate;
using System.Collections.Generic;
using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity
{
    /// <summary>
    /// 条件字段
    /// </summary>
    [Table]
    public class ConditionAggregation
    {
        [Key]
        [AutoIncrease]
        public int Id { get; set; }
        [Column("`Name`")]
        public string Name { get; set; }
        [Column("Icon")]
        public string Icon { get; set; }
        /// <summary>
        /// 标识是属于哪个Condition
        /// </summary>
        [Column]
        public int InterfaceSearchConditionId { get; set; }
        [Column]
        public int ParentId { get; set; }
        [Column]
        public int FieldId { get; set; }
        [Column]
        public string FieldCode { get; set; }
        [Column]
        public string FieldName { get; set; }
        [Column]
        public int ConditionType { get; set; }
        [Column]
        //如果参数传递，则按参数走，如果参数不传递，则使用默认值
        public string Value { get; set; }
        [Column]
        public int ValueType { get; set; }
        public List<ConditionAggregation> Children { get; set; }
    }
}
