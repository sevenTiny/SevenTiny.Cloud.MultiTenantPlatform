namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models
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

    public class EnumConvert
    {
        public static DataType ToDataType(int dataType)
        {
            switch (dataType)
            {
                case 0: return DataType.Unknown;
                case 1: return DataType.Number;
                case 2: return DataType.Text;
                case 3: return DataType.DateTime;
                case 4: return DataType.Date;
                case 5: return DataType.Boolean;
                case 6: return DataType.Int;
                case 7: return DataType.Long;
                case 9: return DataType.Double;
                case 10: return DataType.DataSource;
                case 11: return DataType.StandradDate;
                case 12: return DataType.StandradDateTime;
                default: return DataType.Unknown;
            }
        }
    }
}
