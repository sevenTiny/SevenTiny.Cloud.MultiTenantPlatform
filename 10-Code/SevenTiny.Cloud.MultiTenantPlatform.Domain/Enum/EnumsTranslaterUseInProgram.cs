namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum
{
    /// <summary>
    /// 枚举翻译器,在程序内部使用，不对外翻译
    /// </summary>
    public class EnumsTranslaterUseInProgram
    {
        public static string Tran_ConditionJoint(int type)
        {
            switch (type)
            {
                case (int)ConditionJoint.And:
                    return "And";
                case (int)ConditionJoint.Or:
                    return "Or";
                default:
                    return string.Empty;
            }
        }
        public static string Tran_ConditionType(int type)
        {
            switch (type)
            {
                case (int)ConditionType.Equal:
                    return "==";
                case (int)ConditionType.GreaterThan:
                    return ">";
                case (int)ConditionType.GreaterThanEqual:
                    return ">=";
                case (int)ConditionType.LessThan:
                    return "<";
                case (int)ConditionType.LessThanEqual:
                    return "<=";
                case (int)ConditionType.NotEqual:
                    return "!=";
                default:
                    return string.Empty;
            }
        }
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

        public static InterfaceType ToInterfaceType(int interfaceType)
        {
            switch (interfaceType)
            {
                case 1: return InterfaceType.CloudSingleObject;
                case 2: return InterfaceType.CloudTableList;
                case 3: return InterfaceType.CloudCount;
                case 4: return InterfaceType.EnumeDataSource;
                default: return InterfaceType.CloudCount;
            }
        }
    }
}
