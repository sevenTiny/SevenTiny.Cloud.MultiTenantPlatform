namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums
{
    public class EnumsTranslater
    {
        public static string Tran_TrueFalse(int trueFalse)
        {
            switch (trueFalse)
            {
                case (int)Enums.TrueFalse.False:
                    return "否";
                case (int)Enums.TrueFalse.True:
                    return "是";
                default:
                    return string.Empty;
            }
        }
        public static string Tran_DataType(int datetype)
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
                    return "布尔";
                case (int)DataType.Int:
                    return "整数";
                case (int)DataType.Long:
                    return "长整数";
                case (int)DataType.Float:
                    return "小数(不建议使用)";
                case (int)DataType.Double:
                    return "小数";
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
