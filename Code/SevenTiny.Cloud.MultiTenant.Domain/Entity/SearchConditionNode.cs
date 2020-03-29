using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    /// <summary>
    /// 条件字段
    /// 每一个大条件里面会包含很多子条件，以SearchConditionId字段区分
    /// </summary>
    [Table]
    [TableCaching]
    public class SearchConditionNode : CommonBase
    {
        [Column]
        public string Icon { get; set; }
        /// <summary>
        /// 标识是属于哪个Condition
        /// </summary>
        [Column]
        public Guid SearchConditionId { get; set; }
        [Column]
        public Guid ParentId { get; set; }
        /// <summary>
        /// 连接节点没有field，field=-1。可以通过该字段判断是否为连接节点
        /// </summary>
        [Column]
        public Guid MetaFieldId { get; set; }
        [Column]
        public string MetaFieldCode { get; set; }
        [Column]
        public string MetaFieldName { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        [Column]
        public int MetaFieldType { get; set; }
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

        public List<SearchConditionNode> Children { get; set; }
    }
}
