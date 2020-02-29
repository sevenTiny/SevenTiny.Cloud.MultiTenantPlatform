namespace SevenTiny.Cloud.MultiTenant.Domain.Enum
{
    /// <summary>
    /// 标准接口类型
    /// </summary>
    public enum InterfaceType : int
    {
        /// <summary>
        /// 新增
        /// </summary>
        Add = 1,
        /// <summary>
        /// 批量新增
        /// </summary>
        BatchAdd = 2,
        /// <summary>
        /// 修改
        /// </summary>
        Update = 3,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 4,
        /// <summary>
        /// 单对象
        /// </summary>
        SingleObject = 5,
        /// <summary>
        /// 对象列表
        /// </summary>
        TableList = 6,
        /// <summary>
        /// 数量
        /// </summary>
        Count = 7,
        /// <summary>
        /// 枚举数据源
        /// </summary>
        JsonDataSource = 8,
        /// <summary>
        /// 可执行脚本数据源
        /// </summary>
        ExecutableScriptDataSource = 9
    }
    public static class InterfaceTypeTranslator
    {
        public static string ToChinese(int datatype)
        {
            switch ((InterfaceType)datatype)
            {
                case InterfaceType.Add:
                    return "新增";
                case InterfaceType.BatchAdd:
                    return "新增（批量）";
                case InterfaceType.Update:
                    return "修改";
                case InterfaceType.Delete:
                    return "删除";
                case InterfaceType.SingleObject:
                    return "单对象";
                case InterfaceType.TableList:
                    return "数据集合";
                case InterfaceType.Count:
                    return "数据量";
                case InterfaceType.JsonDataSource:
                    return "Json数据源";
                case InterfaceType.ExecutableScriptDataSource:
                    return "可执行脚本数据源";
                default:
                    return string.Empty;
            }
        }
    }
}
