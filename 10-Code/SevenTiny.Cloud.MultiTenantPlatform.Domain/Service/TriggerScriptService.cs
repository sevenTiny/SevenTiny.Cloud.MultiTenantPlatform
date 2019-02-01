using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class TriggerScriptService : MetaObjectManageRepository<TriggerScript>, ITriggerScriptService
    {
        public TriggerScriptService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        readonly MultiTenantPlatformDbContext dbContext;

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

        public List<TriggerScript> GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(int metaObjectId, int scriptType, int triggerPoint)
        {
            return dbContext.QueryList<TriggerScript>(t => t.MetaObjectId == metaObjectId && t.ScriptType == scriptType && t.TriggerPoint == triggerPoint);
        }

        public string GetDefaultTriggerScriptByScriptTypeAndTriggerPoint(int scriptType, int triggerPoint)
        {
            switch ((TriggerPoint)triggerPoint)
            {
                case TriggerPoint.Before:
                    switch ((ScriptType)scriptType)
                    {
                        case ScriptType.Add:
                            break;
                        case ScriptType.Update:
                            break;
                        case ScriptType.Delete:
                            break;
                        case ScriptType.TableList: return DefaultQueryBeforeTriggerScript;
                        case ScriptType.SingleObject: return DefaultQueryBeforeTriggerScript;
                        case ScriptType.Count: return DefaultCountAfterTriggerScript;
                        default: return null;
                    }
                    break;
                case TriggerPoint.After:
                    switch ((ScriptType)scriptType)
                    {
                        case ScriptType.Add:
                            break;
                        case ScriptType.Update:
                            break;
                        case ScriptType.Delete:
                            break;
                        case ScriptType.TableList: return DefaultTableListAfterTriggerScript;
                        case ScriptType.SingleObject: return DefaultSingleObjectAfterTriggerScript;
                        case ScriptType.Count: return DefaultCountAfterTriggerScript;
                        default: return null;
                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        public string GetDefaultTriggerScriptDataSourceScript() => DefaultDataSourceTriggerScript;

        private string DefaultQueryBeforeTriggerScript
            => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using MongoDB.Bson;
using MongoDB.Driver;
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()
public FilterDefinition<BsonDocument> QueryBefore(string operateCode,FilterDefinition<BsonDocument> condition)
{
	//这里写业务逻辑
	//...
	return condition;
}
";
        private string DefaultTableListAfterTriggerScript
            => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()
public TableListComponent TableListAfter(string operateCode,TableListComponent tableListComponent)
{
	//这里写业务逻辑
	//...
	return tableListComponent;
}
";
        private string DefaultSingleObjectAfterTriggerScript
            => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()
public SingleObjectComponent SingleObjectAfter(string operateCode,SingleObjectComponent singleObjectComponent)
{
	//这里写业务逻辑
	//...
	return singleObjectComponent;
}
";
        private string DefaultCountAfterTriggerScript
            => @"
using System;
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()
public int CountAfter(string operateCode,int count)
{
	//这里写业务逻辑
	//...
	return count;
}
";
        private string DefaultDataSourceTriggerScript
    => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()
public object TriggerScriptDataSource(string operateCode, Dictionary<string, object> argumentsDic)
{
	//这里写业务逻辑
	//...
	return null;
}
";
    }
}
