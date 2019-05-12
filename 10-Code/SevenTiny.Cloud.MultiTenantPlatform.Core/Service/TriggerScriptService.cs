using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using SevenTiny.Bantina;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
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
        public new Result<TriggerScript> Update(TriggerScript triggerScript)
        {
            TriggerScript myfield = GetById(triggerScript.Id);
            if (myfield != null)
            {
                //编码不允许修改
                //脚本类型不允许修改

                //如果脚本有改动，则清空脚本缓存
                if (!myfield.Script.Equals(triggerScript.Script))
                    TriggerScriptCache.ClearCache(triggerScript.Script);

                myfield.Script = triggerScript.Script;
                myfield.FailurePolicy = triggerScript.FailurePolicy;

                myfield.Name = triggerScript.Name;
                myfield.Group = triggerScript.Group;
                myfield.SortNumber = triggerScript.SortNumber;
                myfield.Description = triggerScript.Description;
                myfield.ModifyBy = -1;
                myfield.ModifyTime = DateTime.Now;
            }
            base.Update(myfield);
            return Result<TriggerScript>.Success();
        }

        public List<TriggerScript> GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(int metaObjectId, int scriptType)
        {
            return dbContext.Queryable<TriggerScript>().Where(t => t.MetaObjectId == metaObjectId && t.ScriptType == scriptType).ToList();
        }

        public string GetDefaultTriggerScriptByScriptType(int scriptType)
        {
            switch ((ScriptType)scriptType)
            {
                case ScriptType.Add_Before: return DefaultAddBeforeTriggerScript;

                case ScriptType.Update_Before:
                case ScriptType.Delete_Before:
                case ScriptType.TableList_Before:
                case ScriptType.SingleObject_Before:
                case ScriptType.Count_Before: return DefaultQueryBeforeTriggerScript;

                case ScriptType.TableList_After: return DefaultTableListAfterTriggerScript;
                case ScriptType.SingleObject_After: return DefaultSingleObjectAfterTriggerScript;
                case ScriptType.Count_After: return DefaultCountAfterTriggerScript;
                default: return null;
            }
        }

        public string GetDefaultTriggerScriptDataSourceScript() => DefaultDataSourceTriggerScript;

        /// <summary>
        /// 所有脚本默认内置的通用命名空间引用，个性化的引用请写在各自脚本中
        /// </summary>
        private string DefaultCommonUsing
            => @"
using SevenTiny.Cloud.MultiTenantPlatform.Core.CloudEntity;
using MongoDB.Bson;
using MongoDB.Driver;
";

        /// <summary>
        /// 所有脚本类内方法外内置的通用代码段，个性化请卸写在各自脚本中
        /// </summary>
        private string DefaultCommonClassInnerCode
           => @"
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()
";

        /// <summary>
        /// 所有脚本方法内默认内置的通用代码段，个性化请写在各自脚本中
        /// </summary>
        private string DefaultCommonMethodCode
            => @"

";

        #region 各类触发点的触发器脚本

        private string DefaultQueryBeforeTriggerScript
            => $@"
{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public FilterDefinition<BsonDocument> QueryBefore(string operateCode,FilterDefinition<BsonDocument> condition)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return condition;
}}
";
        private string DefaultBatchAddBeforeTriggerScript
    => $@"
{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public List<BsonDocument> BatchAddBefore(string operateCode,List<BsonDocument> bsonElementsList)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return bsonElementsList;
}}
";
        private string DefaultAddBeforeTriggerScript
    => $@"
{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public BsonDocument AddBefore(string operateCode,BsonDocument bsonElements)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return bsonElements;
}}
";
        private string DefaultTableListAfterTriggerScript
            => $@"
{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public TableListComponent TableListAfter(string operateCode,TableListComponent tableListComponent)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return tableListComponent;
}}
";
        private string DefaultSingleObjectAfterTriggerScript
            => $@"
{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public SingleObjectComponent SingleObjectAfter(string operateCode,SingleObjectComponent singleObjectComponent)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return singleObjectComponent;
}}
";
        private string DefaultCountAfterTriggerScript
            => $@"
{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public int CountAfter(string operateCode,int count)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return count;
}}
";
        private string DefaultDataSourceTriggerScript
    => $@"
{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public object TriggerScriptDataSource(string operateCode, Dictionary<string, object> argumentsDic)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return null;
}}
";

        #endregion
    }
}
