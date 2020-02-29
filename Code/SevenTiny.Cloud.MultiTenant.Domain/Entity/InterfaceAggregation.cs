using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    /// <summary>
    /// 组织接口
    /// </summary>
    [Table]
    [TableCaching]
    public class InterfaceAggregation : MetaObjectCommonBase
    {
        /// <summary>
        /// 接口类型
        /// </summary>
        [Column]
        public int InterfaceType { get; set; }
        /// <summary>
        /// 搜索条件Id
        /// </summary>
        [Column]
        public int SearchConditionId { get; set; }
        /// <summary>
        /// 搜索条件名称
        /// </summary>
        [Column]
        public string SearchConditionName { get; set; }
        /// <summary>
        /// 列表Id
        /// </summary>
        [Column]
        public int FieldListId { get; set; }
        /// <summary>
        /// 列表名称
        /// </summary>
        [Column]
        public string FieldListName { get; set; }
        /// <summary>
        /// 表单Id
        /// </summary>
        [Column]
        public int FormId { get; set; }
        /// <summary>
        /// 表单名称
        /// </summary>
        [Column]
        public string FormName { get; set; }
        /// <summary>
        /// 数据源Id
        /// </summary>
        [Column]
        public int DataSourceId { get; set; }
        /// <summary>
        /// 数据源名称
        /// </summary>
        [Column]
        public string DataSourceName { get; set; }
    }
}
