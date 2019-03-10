using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Caching;
using SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Base;
using SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Models;
using SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.ListView;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.Service
{
    public class TriggerScriptExecuteService : ITriggerScriptExecuteService
    {
        public TriggerScriptExecuteService(ITriggerScriptService _triggerScriptService)
        {
            triggerScriptService = _triggerScriptService;
        }

        readonly ITriggerScriptService triggerScriptService;

        /// <summary>
        /// 通用的执行脚本的方法
        /// </summary>
        /// <typeparam name="TResult">返回值类型</typeparam>
        /// <typeparam name="TArg">参数类型</typeparam>
        /// <param name="operateCode">操作码</param>
        /// <param name="triggerScript">触发器脚本</param>
        /// <param name="executeMethod">执行方法名</param>
        /// <param name="arg">参数</param>
        /// <returns></returns>
        private TResult CommonExecute<TResult, TArg>(string operateCode, string triggerScript, string executeMethod, TArg arg)
        {
            //校验脚本正确性
            if (string.IsNullOrEmpty(triggerScript))
                throw new ArgumentNullException("triggerScript", "triggerScript not fount");

            var triggerScriptInCache = TriggerScriptCache.GetSet(triggerScript, () =>
            {
                string scriptText = $"{TriggerScriptParsing.GetCompleteScript(triggerScript)} return new TriggerScript_Class().{executeMethod};";

                var script = CSharpScript.Create<TResult>(scriptText,
                    ScriptOptions.Default
                    .AddReferences(ReferenceProvider.GeneralRefrences),
                    globalsType: typeof(TArg)
                    );

                script.Compile();

                return script;
            });

            var result = triggerScriptInCache.RunAsync(globals: arg).Result.ReturnValue;

            return result;
        }

        /// <summary>
        /// 通用的执行前条件触发器脚本
        /// </summary>
        /// <param name="operateCode"></param>
        /// <param name="condition"></param>
        /// <param name="triggerScript"></param>
        /// <returns></returns>
        private FilterDefinition<BsonDocument> ExecuteQueryBefore(string operateCode, FilterDefinition<BsonDocument> condition, string triggerScript)
        {
            return CommonExecute<FilterDefinition<BsonDocument>, QueryBeforeArg>(operateCode, triggerScript, "QueryBefore(operateCode,condition)", new QueryBeforeArg { operateCode = operateCode, condition = condition });
        }

        public BsonDocument AddBefore(int metaObjectId, string operateCode, BsonDocument bsonElements)
        {
            var triggerScripts = triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(metaObjectId, (int)ScriptType.Add_Before);
            if (triggerScripts != null && triggerScripts.Any())
            {
                foreach (var item in triggerScripts)
                {
                    try
                    {
                        bsonElements = CommonExecute<BsonDocument, AddArg>(operateCode, item.Script, "AddBefore(operateCode,bsonElements)", new AddArg { operateCode = operateCode, bsonElements = bsonElements });
                    }
                    catch (Exception ex)
                    {
                        if (item.FailurePolicy == (int)FailurePolicy.Break)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return bsonElements;
        }

        public List<BsonDocument> BatchAddBefore(int metaObjectId, string operateCode, List<BsonDocument> bsonElementsList)
        {
            var triggerScripts = triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(metaObjectId, (int)ScriptType.BatchAdd_Before);
            if (triggerScripts != null && triggerScripts.Any())
            {
                foreach (var item in triggerScripts)
                {
                    try
                    {
                        bsonElementsList = CommonExecute<List<BsonDocument>, BatchAddArg>(operateCode, item.Script, "BatchAddBefore(operateCode,bsonElementsList)", new BatchAddArg { operateCode = operateCode, bsonElementsList = bsonElementsList });
                    }
                    catch (Exception ex)
                    {
                        if (item.FailurePolicy == (int)FailurePolicy.Break)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return bsonElementsList;
        }

        public FilterDefinition<BsonDocument> UpdateBefore(int metaObjectId, string operateCode, FilterDefinition<BsonDocument> condition)
        {
            var triggerScripts = triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(metaObjectId, (int)ScriptType.Update_Before);
            if (triggerScripts != null && triggerScripts.Any())
            {
                foreach (var item in triggerScripts)
                {
                    try
                    {
                        condition = ExecuteQueryBefore(operateCode, condition, item.Script);
                    }
                    catch (Exception ex)
                    {
                        if (item.FailurePolicy == (int)FailurePolicy.Break)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return condition;
        }

        public FilterDefinition<BsonDocument> DeleteBefore(int metaObjectId, string operateCode, FilterDefinition<BsonDocument> condition)
        {
            var triggerScripts = triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(metaObjectId, (int)ScriptType.Delete_Before);
            if (triggerScripts != null && triggerScripts.Any())
            {
                foreach (var item in triggerScripts)
                {
                    try
                    {
                        condition = ExecuteQueryBefore(operateCode, condition, item.Script);
                    }
                    catch (Exception ex)
                    {
                        if (item.FailurePolicy == (int)FailurePolicy.Break)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return condition;
        }

        //TableList触发器
        public FilterDefinition<BsonDocument> TableListBefore(int metaObjectId, string operateCode, FilterDefinition<BsonDocument> condition)
        {
            var triggerScripts = triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(metaObjectId, (int)ScriptType.TableList_Before);
            if (triggerScripts != null && triggerScripts.Any())
            {
                foreach (var item in triggerScripts)
                {
                    try
                    {
                        condition = ExecuteQueryBefore(operateCode, condition, item.Script);
                    }
                    catch (Exception ex)
                    {
                        if (item.FailurePolicy == (int)FailurePolicy.Break)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return condition;
        }
        public TableListComponent TableListAfter(int metaObjectId, string operateCode, TableListComponent tableListComponent)
        {
            var triggerScripts = triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(metaObjectId, (int)ScriptType.TableList_After);
            if (triggerScripts != null && triggerScripts.Any())
            {
                foreach (var item in triggerScripts)
                {
                    try
                    {
                        tableListComponent = CommonExecute<TableListComponent, TableListArg>(operateCode, item.Script, "TableListAfter(operateCode,tableListComponent)", new TableListArg { operateCode = operateCode, tableListComponent = tableListComponent });
                    }
                    catch (Exception ex)
                    {
                        if (item.FailurePolicy == (int)FailurePolicy.Break)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return tableListComponent;
        }

        //SingleObject触发器
        public FilterDefinition<BsonDocument> SingleObjectBefore(int metaObjectId, string operateCode, FilterDefinition<BsonDocument> condition)
        {
            var triggerScripts = triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(metaObjectId, (int)ScriptType.SingleObject_Before);
            if (triggerScripts != null && triggerScripts.Any())
            {
                foreach (var item in triggerScripts)
                {
                    try
                    {
                        condition = ExecuteQueryBefore(operateCode, condition, item.Script);
                    }
                    catch (Exception ex)
                    {
                        if (item.FailurePolicy == (int)FailurePolicy.Break)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return condition;
        }
        public SingleObjectComponent SingleObjectAfter(int metaObjectId, string operateCode, SingleObjectComponent singleObjectComponent)
        {
            var triggerScripts = triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(metaObjectId, (int)ScriptType.SingleObject_After);
            if (triggerScripts != null && triggerScripts.Any())
            {
                foreach (var item in triggerScripts)
                {
                    try
                    {
                        singleObjectComponent = CommonExecute<SingleObjectComponent, SingleObjectArg>(operateCode, item.Script, "SingleObjectAfter(operateCode,singleObjectComponent)", new SingleObjectArg { operateCode = operateCode, singleObjectComponent = singleObjectComponent });
                    }
                    catch (Exception ex)
                    {
                        if (item.FailurePolicy == (int)FailurePolicy.Break)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return singleObjectComponent;
        }

        //Count触发器
        public FilterDefinition<BsonDocument> CountBefore(int metaObjectId, string operateCode, FilterDefinition<BsonDocument> condition)
        {
            var triggerScripts = triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(metaObjectId, (int)ScriptType.Count_Before);
            if (triggerScripts != null && triggerScripts.Any())
            {
                foreach (var item in triggerScripts)
                {
                    try
                    {
                        condition = ExecuteQueryBefore(operateCode, condition, item.Script);
                    }
                    catch (Exception ex)
                    {
                        if (item.FailurePolicy == (int)FailurePolicy.Break)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return condition;
        }
        public int CountAfter(int metaObjectId, string operateCode, int count)
        {
            var triggerScripts = triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(metaObjectId, (int)ScriptType.Count_After);
            if (triggerScripts != null && triggerScripts.Any())
            {
                foreach (var item in triggerScripts)
                {
                    try
                    {
                        count = CommonExecute<int, CountArg>(operateCode, item.Script, "CountAfter(operateCode,count)", new CountArg { operateCode = operateCode, count = count });
                    }
                    catch (Exception ex)
                    {
                        if (item.FailurePolicy == (int)FailurePolicy.Break)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return count;
        }

        //触发器数据源
        public object TriggerScriptDataSource(string operateCode, Dictionary<string, object> argumentsDic, string script)
        {
            return CommonExecute<object, TriggerScriptDataSourceArg>(operateCode, script, "TriggerScriptDataSource(operateCode,argumentsDic)", new TriggerScriptDataSourceArg { operateCode = operateCode, argumentsDic = argumentsDic });
        }
    }
}
