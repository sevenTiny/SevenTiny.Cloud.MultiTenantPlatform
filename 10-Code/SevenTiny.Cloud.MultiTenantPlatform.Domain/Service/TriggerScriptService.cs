using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class TriggerScriptService : MetaObjectManageRepository<TriggerScript>, ITriggerScriptService
    {
        public TriggerScriptService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="triggerScript"></param>
        public new ResultModel Update(TriggerScript triggerScript)
        {
            TriggerScript myfield = GetById(triggerScript.Id);
            if (myfield != null)
            {
                //编码不允许修改
                //脚本类型不允许修改
                myfield.Script = triggerScript.Script;
                myfield.Name = triggerScript.Name;
                myfield.Group = triggerScript.Group;
                myfield.SortNumber = triggerScript.SortNumber;
                myfield.Description = triggerScript.Description;
                myfield.ModifyBy = -1;
                myfield.ModifyTime = DateTime.Now;
            }
            base.Update(myfield);
            return ResultModel.Success();
        }

        public TableListComponent TableListAfter(TableListComponent tableListComponent, int triggerScriptId)
        {
            var triggerScript = base.GetById(triggerScriptId);
            //如果没有找到脚本，则跳过
            if (triggerScript == null)
            {
                return tableListComponent;
            }
            //正常采用拼接的方式

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(@"
            using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
            
            public class TriggerScript_TableListComponent");
            stringBuilder.Append("{");
            stringBuilder.Append(triggerScript.Script);
            stringBuilder.Append("}");

            stringBuilder.Append("return new TriggerScript_TableListComponent().TableListAfter(tableListComponent);");

            var script = CSharpScript.Create<TableListComponent>(stringBuilder.ToString(),
                ScriptOptions.Default.AddReferences("SevenTiny.Cloud.MultiTenantPlatform.Domain"),//引用dll
                globalsType: typeof(TableListArg)
                );

            script.Compile();

            var result = script.RunAsync(globals: new TableListArg { tableListComponent = tableListComponent }).Result.ReturnValue;

            return result;
        }
    }

    public class TableListArg
    {
        public TableListComponent tableListComponent { get; set; }
    }
}
