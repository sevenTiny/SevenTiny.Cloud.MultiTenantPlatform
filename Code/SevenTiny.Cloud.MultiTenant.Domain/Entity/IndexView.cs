using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    /// <summary>
    /// 视图
    /// </summary>
    [Table]
    [TableCaching]
    public class IndexView : MetaObjectCommonBase
    {
        /// <summary>
        /// 页面标题
        /// </summary>
        [Column]
        public string Title { get; set; }
        /// <summary>
        /// 标题图标
        /// </summary>
        [Column]
        public string Icon { get; set; }
        [Column]
        public int SearchConditionId { get; set; }
        [Column]
        public string SearchConditionName { get; set; }
        [Column]
        public int FieldListId { get; set; }
        [Column]
        public string FieldListName { get; set; }
        [Column]
        public int LayoutType { get; set; }//数据库表还未加字段！！！
    }
}
