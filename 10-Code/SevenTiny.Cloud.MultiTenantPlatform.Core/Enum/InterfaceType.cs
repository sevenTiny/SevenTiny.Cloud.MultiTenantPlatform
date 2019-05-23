namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Enum
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
    public static class InterfaceTypeTranslator
    {
        public static string ToLabel(int datatype)
        {
            switch (datatype)
            {
                case (int)InterfaceType.CloudSingleObject:
                    return "单对象";
                case (int)InterfaceType.CloudTableList:
                    return "数据集合";
                case (int)InterfaceType.CloudCount:
                    return "数据量";
                case (int)InterfaceType.EnumeDataSource:
                    return "枚举数据源";
                case (int)InterfaceType.TriggerScriptDataSource:
                    return "触发器数据源";
                default:
                    return string.Empty;
            }
        }
    }
}
