using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    /// <summary>
    /// 每个Form表单包含的字段（一个Form对应多个FormMetaField以FormId区分）
    /// </summary>
    [Table]
    public class FormMetaField
    {
        [Key]
        [AutoIncrease]
        [Column]
        public int Id { get; set; }
        [Column]
        public int FormId { get; set; }
        [Column]
        public int MetaFieldId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Column]
        public string Name { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        [Column]
        public string Text { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        [Column]
        public int FieldType { get; set; }
        /// <summary>
        /// 字段长度
        /// </summary>
        [Column]
        public int FieldLength { get; set; }
        /// <summary>
        /// 是否显示
        /// </summary>
        [Column]
        public int IsVisible { get; set; } = 1;
        /// <summary>
        /// 是否必填
        /// </summary>
        [Column]
        public int IsMust { get; set; } = 0;
        /// <summary>
        /// 正则表达式
        /// </summary>
        [Column]
        public string Regular { get; set; }
        /// <summary>
        /// 列表排序值
        /// </summary>
        [Column]
        public int SortNumber { get; set; }
    }
}
