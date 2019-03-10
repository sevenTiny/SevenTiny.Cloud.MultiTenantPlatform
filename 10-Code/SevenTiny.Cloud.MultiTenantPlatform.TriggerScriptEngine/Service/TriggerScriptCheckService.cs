using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Base;
using SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.ServiceContract;
using System;
using System.IO;

namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Service
{
    public class TriggerScriptCheckService : ITriggerScriptCheckService
    {
        /// <summary>
        /// 编译并检查脚本
        /// </summary>
        /// <param name="script">脚本</param>
        /// <returns></returns>
        public Tuple<bool, string> CompilationAndCheckScript(string script)
        {
            //获得完整的脚本
            string completeScript = TriggerScriptParsing.GetCompleteScript(script);

            var syntaxTree = CSharpSyntaxTree.ParseText(completeScript);

            //todo:这句编译检查每次会内存增长,这里要考虑性能问题!!!
            var assemblyName = $"GenericGenerator.script";
            var compilation = CSharpCompilation.Create(assemblyName, new[] { syntaxTree })
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(ReferenceProvider.GetGeneralMetadataReferences());

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                ms.Flush();
                ms.Dispose();
                return new Tuple<bool, string>(result.Success, string.Join(";\r\n", result.Diagnostics));
            }
        }
    }
}
