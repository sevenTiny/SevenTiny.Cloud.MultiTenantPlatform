namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum
{
    /// <summary>
    /// 脚本类型
    /// </summary>
    public enum ScriptType
    {
        Add = 1,
        Update = 2,
        Delete = 3,
        TableList = 4,
        SingleObject = 5
    }
    public static class ScriptTypeTranslator
    {
        public static string ToName(int scriptType)
        {
            switch (scriptType)
            {
                case (int)ScriptType.TableList: return "列表";
                case (int)ScriptType.SingleObject: return "单对象";
                case (int)ScriptType.Add: return "新增";
                case (int)ScriptType.Update: return "修改";
                case (int)ScriptType.Delete: return "删除";
                default: return "未知";
            }
        }
        public static string ToCode(int scriptType)
        {
            switch (scriptType)
            {
                case (int)ScriptType.TableList: return "TableList";
                case (int)ScriptType.SingleObject: return "SingleObject";
                case (int)ScriptType.Add: return "Add";
                case (int)ScriptType.Update: return "Update";
                case (int)ScriptType.Delete: return "Delete";
                default: return "UnKnown";
            }
        }
    }
}
