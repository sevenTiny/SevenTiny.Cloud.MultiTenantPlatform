using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Base
{
    internal class TriggerScriptParsing
    {
        /// <summary>
        /// using 分隔符（脚本中必须存在该分割符）
        /// </summary>
        internal static readonly string USING_SEPARATOR = "//end using";

        /// <summary>
        /// 获取完整的脚本
        /// </summary>
        /// <param name="triggerScript">触发器脚本（方法）</param>
        /// <returns></returns>
        internal static string GetCompleteScript(string triggerScript)
        {
            //检查标识是否存在
            if (!triggerScript.Contains(USING_SEPARATOR))
                throw new KeyNotFoundException($"'{USING_SEPARATOR}' not found in trigger script");

            string[] scriptArray = Regex.Split(triggerScript, USING_SEPARATOR, RegexOptions.IgnoreCase);

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(ReferenceProvider.GeneralUsing);

            //append using
            stringBuilder.Append(scriptArray[0]);

            stringBuilder.Append("public class TriggerScript_Class");
            stringBuilder.Append("{");

            //common operation
            stringBuilder.Append("MultiTenantDataDbContext Db = new MultiTenantDataDbContext();");

            //script method
            stringBuilder.Append(scriptArray[1]);
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}
