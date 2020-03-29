using SevenTiny.Bantina.Bankinate.Attributes;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    /// <summary>
    /// 配置字段
    /// 用于表单或者列表的子字段的配置
    /// 一个表单对应多个配置字段
    /// 一个列表对应多个配置字段
    /// </summary>
    [Table]
    [TableCaching]
    public class ConfigField : MetaObjectCommonBase
    {
        /// <summary>
        /// 元数据字段的Id
        /// </summary>
        [Column]
        public Guid MetaFieldId { get; set; }
        /// <summary>
        /// 表单Id
        /// </summary>
        [Column]
        public Guid FormId { get; set; }
        /// <summary>
        /// 列表视图Id
        /// </summary>
        [Column]
        public Guid ListViewId { get; set; }

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
        /// 是否是链接
        /// </summary>
        [Column]
        public int IsLink { get; set; } = 0;
    }
}
