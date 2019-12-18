namespace SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject
{
    /// <summary>
    /// 排序字段
    /// </summary>
    public class SortField
    {
        /// <summary>
        /// 需要排序的列
        /// </summary>
        public string Column { get; set; }
        /// <summary>
        /// 是否倒序
        /// </summary>
        public bool IsDesc { get; set; }
    }
}
