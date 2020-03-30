using SevenTiny.Bantina.Bankinate.Attributes;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    /// <summary>
    /// 云接口
    /// </summary>
    [Table]
    [TableCaching]
    public class CloudInterface : MetaObjectCommonBase
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
        public Guid SearchConditionId { get; set; }
        /// <summary>
        /// 搜索条件名称
        /// </summary>
        [Column]
        public string SearchConditionName { get; set; }
        /// <summary>
        /// 列表Id
        /// </summary>
        [Column]
        public Guid ListViewId { get; set; }
        /// <summary>
        /// 列表名称
        /// </summary>
        [Column]
        public string ListViewName { get; set; }
        /// <summary>
        /// 表单Id
        /// </summary>
        [Column]
        public Guid FormViewId { get; set; }
        /// <summary>
        /// 表单名称
        /// </summary>
        [Column]
        public string FormViewName { get; set; }
        /// <summary>
        /// 数据源Id
        /// </summary>
        [Column]
        public Guid DataSourceId { get; set; }
        /// <summary>
        /// 数据源名称
        /// </summary>
        [Column]
        public string DataSourceName { get; set; }
    }
}
