namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum
{
    /// <summary>
    /// 脚本类型
    /// </summary>
    public enum ScriptType
    {
        Add_Before = 1,
        Update_Before = 3,
        Delete_Before = 5,
        TableList_Before = 7,
        TableList_After = 8,
        SingleObject_Before = 9,
        SingleObject_After = 10,
        Count_Before = 11,
        Count_After = 12
    }
    public static class ScriptTypeTranslator
    {
        public static string ToCode(int scriptType)
        {
            switch ((ScriptType)scriptType)
            {
                case ScriptType.Add_Before: return "Add_Before";
                case ScriptType.Update_Before: return "Update_Before";
                case ScriptType.Delete_Before: return "Delete_Before";
                case ScriptType.TableList_Before: return "TableList_Before";
                case ScriptType.TableList_After: return "TableList_After";
                case ScriptType.SingleObject_Before: return "SingleObject_Before";
                case ScriptType.SingleObject_After: return "SingleObject_After";
                case ScriptType.Count_Before: return "Count_Before";
                case ScriptType.Count_After: return "Count_After";
                default: return "UnKnown";
            }
        }
    }
}
