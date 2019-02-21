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
        CloudSingleObject = 1,
        /// <summary>
        /// 对象列表
        /// </summary>
        CloudTableList = 2,
        /// <summary>
        /// 数量
        /// </summary>
        CloudCount = 3,
        /// <summary>
        /// 枚举数据源
        /// </summary>
        EnumeDataSource = 4,
        /// <summary>
        /// 触发器脚本数据源
        /// </summary>
        TriggerScriptDataSource = 5
    }
}
