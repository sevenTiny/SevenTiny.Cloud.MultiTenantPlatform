namespace SevenTiny.Cloud.MultiTenant.Domain.Enum
{
    /// <summary>
    /// 数据源类型
    /// </summary>
    public enum DataSourceType : int
    {
        /// <summary>
        /// Json
        /// </summary>
        Json = 1,
        /// <summary>
        /// 执行脚本（结果）
        /// </summary>
        Script = 2
        //接口（暂不支持,注意配置到接口里面的循环引用）
        //Interface = 3
    }

    public static class DataSourceTypeTranslator
    {
        public static string ToChinese(int dataSourceType)
        {
            switch ((DataSourceType)dataSourceType)
            {
                case DataSourceType.Json: return "Json对象";
                case DataSourceType.Script: return "执行脚本";
                default: return "UnKnown";
            }
        }
    }
}
