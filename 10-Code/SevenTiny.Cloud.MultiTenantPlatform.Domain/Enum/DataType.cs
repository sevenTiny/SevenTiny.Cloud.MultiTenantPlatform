namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum
{
    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType : int
    {
        Unknown = 0,
        Number = 1,
        Text = 2,
        DateTime = 3,
        Date = 4,
        Boolean = 5,
        Int = 6,
        Long = 7,
        //Float = 8,
        Double = 9,
        DataSource = 10,
        StandradDate = 11,
        StandradDateTime = 12
    }
}
