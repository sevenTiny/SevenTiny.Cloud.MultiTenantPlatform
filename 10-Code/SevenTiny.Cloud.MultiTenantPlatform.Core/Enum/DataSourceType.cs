namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Enum
{
    /// <summary>
    /// 数据源类型
    /// </summary>
    public enum DataSourceType : int
    {
        //常量
        Const = 1,
        //枚举
        Enum = 2,
        //脚本（结果）
        Script = 3
        //接口（暂不支持,注意配置到接口里面的循环引用）
        //Interface = 4
    }
}
