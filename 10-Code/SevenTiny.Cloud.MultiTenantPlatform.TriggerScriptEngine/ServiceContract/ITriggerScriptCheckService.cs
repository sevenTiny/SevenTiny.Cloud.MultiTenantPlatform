using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.ServiceContract
{
    public interface ITriggerScriptCheckService
    {
        /// <summary>
        /// 编译检查脚本
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        Tuple<bool, string> CompilationAndCheckScript(string script);
    }
}
