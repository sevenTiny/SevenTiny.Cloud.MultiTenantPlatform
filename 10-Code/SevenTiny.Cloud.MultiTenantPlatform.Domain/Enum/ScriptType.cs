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
        SingleObject = 5,
        Count = 6
    }
    public static class ScriptTypeTranslator
    {
        public static string ToCode(int scriptType)
        {
            switch (scriptType)
            {
                case (int)ScriptType.Add: return "Add";
                case (int)ScriptType.Update: return "Update";
                case (int)ScriptType.Delete: return "Delete";
                case (int)ScriptType.TableList: return "TableList";
                case (int)ScriptType.SingleObject: return "SingleObject";
                case (int)ScriptType.Count: return "Count";
                default: return "UnKnown";
            }
        }
    }
}
