using SevenTiny.Bantina.Bankinate.Attributes;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity
{
    /// <summary>
    /// 条件字段
    /// </summary>
    [Table]
    public class SearchConditionAggregation
    {
        [Key]
        [AutoIncrease]
        [Column]
        public int Id { get; set; }
        [Column("`Name`")]
        public string Name { get; set; }
        [Column("Icon")]
        public string Icon { get; set; }
        /// <summary>
        /// 标识是属于哪个Condition
        /// </summary>
        [Column]
        public int SearchConditionId { get; set; }
        [Column]
        public int ParentId { get; set; }
        /// <summary>
        /// 连接节点没有field，field=-1。可以通过该字段判断是否为连接节点
        /// </summary>
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
        [Column]
        public string Text { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        [Column]
        public int Visible { get; set; }

        public List<SearchConditionAggregation> Children { get; set; }
    }
}
