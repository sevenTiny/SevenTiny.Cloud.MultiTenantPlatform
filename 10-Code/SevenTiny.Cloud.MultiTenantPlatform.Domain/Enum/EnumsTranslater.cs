namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum
{
    /// <summary>
    /// 枚举翻译器
    /// </summary>
    public class EnumsTranslater
    {
        public static string Tran_TrueFalse(int trueFalse)
        {
            switch (trueFalse)
            {
                case (int)TrueFalse.False:
                    return "否";
                case (int)TrueFalse.True:
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
                    return "布尔（是=1/true，否=0/false）";
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
        public static string Tran_InterfaceType(int datatype)
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
                default:
                    return string.Empty;
            }
        }
        public static string Tran_ConditionValueType(int type)
        {
            switch (type)
            {
                case (int)ConditionValueType.Const:
                    return "固定值";
                case (int)ConditionValueType.Parameter:
                    return "参数传递值";
                default:
                    return string.Empty;
            }
        }
        public static string Tran_ConditionJoint(int type)
        {
            switch (type)
            {
                case (int)ConditionJoint.And:
                    return "And（与）";
                case (int)ConditionJoint.Or:
                    return "Or（或）";
                default:
                    return string.Empty;
            }
        }
        public static string Tran_ConditionType(int type)
        {
            switch (type)
            {
                case (int)ConditionType.Equal:
                    return "==（等于）";
                case (int)ConditionType.GreaterThan:
                    return ">（大于）";
                case (int)ConditionType.GreaterThanEqual:
                    return ">=（大于等于）";
                case (int)ConditionType.LessThan:
                    return "<（小于）";
                case (int)ConditionType.LessThanEqual:
                    return "<=（小于等于）";
                case (int)ConditionType.NotEqual:
                    return "!=（不等于）";
                default:
                    return string.Empty;
            }
        }
    }
}
