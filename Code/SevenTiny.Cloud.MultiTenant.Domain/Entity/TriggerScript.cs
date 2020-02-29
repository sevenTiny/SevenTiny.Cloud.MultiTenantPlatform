using SevenTiny.Cloud.ScriptEngine;
using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenant.Domain.Entity
{
    [Table]
    [TableCaching]
    public class TriggerScript : MetaObjectCommonBase
    {
        /// <summary>
        /// 触发器类型
        /// </summary>
        [Column]
        public int ScriptType { get; set; }
        /// <summary>
        /// 脚本对应服务
        /// </summary>
        [Column]
        public int ServiceType { get; set; }
        /// <summary>
        /// 服务方法[Get/Post/Put/Delete]
        /// </summary>
        [Column]
        public int ServiceMethod { get; set; }
        /// <summary>
        /// 触发点
        /// </summary>
        [Column]
        public int TriggerPoint { get; set; }
        /// <summary>
        /// 脚本语言
        /// </summary>
        [Column]
        public int Language { get; set; }
        /// <summary>
        /// 失败执行策略
        /// </summary>
        [Column]
        public int OnFailurePolicy { get; set; }
        /// <summary>
        /// 脚本内容
        /// </summary>
        [Column]
        public string Script { get; set; }
    }

    public static class TriggerScriptExtensions
    {
        public static DynamicScript ToDynamicScript(this TriggerScript triggerScript)
        {
            return new DynamicScript
            {
                Script = triggerScript.Script,
                Language = (DynamicScriptLanguage)triggerScript.Language
            };
        }
    }
}
