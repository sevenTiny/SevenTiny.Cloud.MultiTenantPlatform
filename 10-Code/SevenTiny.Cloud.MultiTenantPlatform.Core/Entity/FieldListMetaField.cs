using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [Table]
    public class FieldListMetaField
    {
        [Key]
        [AutoIncrease]
        [Column]
        public int Id { get; set; }
        [Column]
        public int FieldListId { get; set; }
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
        /// 是否是链接
        /// </summary>
        [Column]
        public int IsLink { get; set; } = 0;
        /// <summary>
        /// 列表排序值
        /// </summary>
        [Column]
        public int SortNumber { get; set; }
    }
}
