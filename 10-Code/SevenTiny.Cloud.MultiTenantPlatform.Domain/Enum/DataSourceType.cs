namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum
{
    /// <summary>
    /// 数据源类型
    /// </summary>
    public enum DataSourceType : int
    {
        //常量
        Const = 0,
        //枚举
        Enum = 1,
        //脚本（结果）
        Script = 2
        //接口（暂不支持,注意配置到接口里面的循环引用）
        //Interface = 2
    }
}
