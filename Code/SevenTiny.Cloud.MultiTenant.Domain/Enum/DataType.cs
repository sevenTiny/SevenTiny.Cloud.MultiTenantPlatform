namespace SevenTiny.Cloud.MultiTenant.Domain.Enum
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

    public static class DataTypeTranslator
    {
        public static string ToLabel(int datetype)
        {
            switch (datetype)
            {
                case (int)DataType.Unknown:
                    return "未知";
                case (int)DataType.Number:
                    return "正数";
                case (int)DataType.Text:
                    return "文本";
                case (int)DataType.DateTime:
                    return "时间";
                case (int)DataType.Date:
                    return "日期";
                case (int)DataType.Boolean:
                    return "布尔（true/false）";
                case (int)DataType.Int:
                    return "整数";
                case (int)DataType.Long:
                    return "长整数";
                //case (int)DataType.Float:
                //    return "小数（单精度，不建议使用）";
                case (int)DataType.Double:
                    return "小数（双精度）";
                case (int)DataType.DataSource:
                    return "数据源";
                case (int)DataType.StandradDate:
                    return "标准日期（不支持）";
                case (int)DataType.StandradDateTime:
                    return "标准时间（不支持）";
                default:
                    return string.Empty;
            }
        }
    }
}