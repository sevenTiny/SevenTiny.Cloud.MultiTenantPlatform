namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum
{
    /// <summary>
    /// 标准接口类型
    /// </summary>
    public enum InterfaceType : int
    {
        /// <summary>
        /// 单对象
        /// </summary>
        CloudSingleObject = 0,
        /// <summary>
        /// 对象列表
        /// </summary>
        CloudTableList = 1,
        /// <summary>
        /// 数量
        /// </summary>
        CloudCount = 2,
        /// <summary>
        /// 枚举数据源
        /// </summary>
        EnumeDataSource = 3
    }
}
