namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums
{
    //数据源类型
    public enum DataSourceType
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
