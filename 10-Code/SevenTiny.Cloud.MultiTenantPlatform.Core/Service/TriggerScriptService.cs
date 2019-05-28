using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using Seventiny.Cloud.ScriptEngine;
using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.ListView;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
{
    public class TriggerScriptService : MetaObjectManageRepository<TriggerScript>, ITriggerScriptService
    {
        public TriggerScriptService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IScriptEngineProvider scriptEngineProvider
            ) : base(multiTenantPlatformDbContext)
        {
            _dbContext = multiTenantPlatformDbContext;
            _scriptEngineProvider = scriptEngineProvider;
        }

        readonly MultiTenantPlatformDbContext _dbContext;
        readonly IScriptEngineProvider _scriptEngineProvider;

        public new Result<TriggerScript> Add(TriggerScript triggerScript)
        {
            triggerScript.ScriptType = (int)ScriptType.MetaObject;
            return base.Add(triggerScript);
        }

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
                myfield.ServiceType = triggerScript.ServiceType;
                myfield.ServiceMethod = triggerScript.ServiceMethod;
                myfield.TriggerPoint = triggerScript.TriggerPoint;
                myfield.Language = triggerScript.Language;
                myfield.OnFailurePolicy = triggerScript.OnFailurePolicy;
                myfield.Script = triggerScript.Script;

                myfield.Name = triggerScript.Name;
                myfield.Group = triggerScript.Group;
                myfield.SortNumber = triggerScript.SortNumber;
                myfield.Description = triggerScript.Description;
                myfield.ModifyBy = -1;
                myfield.ModifyTime = DateTime.Now;
            }
            return base.Update(myfield);
        }

        public Result CompilateAndCheckScript(string script, string applicationCode)
        {
            var dynamicScript = new DynamicScript
            {
                Script = script,
                Language = DynamicScriptLanguage.CSharp,
                ProjectName = applicationCode
            };
            return _scriptEngineProvider.CheckScript(dynamicScript);
        }

        public List<TriggerScript> GetTriggerScriptsUnDeletedByMetaObjectIdAndServiceType(int metaObjectId, int serviceType)
        {
            return _dbContext.Queryable<TriggerScript>().Where(t => t.MetaObjectId == metaObjectId && t.ServiceType == serviceType).ToList();
        }

        #region Execute Script
        public BsonDocument Run_MetaObject_Interface_Add_Before(int metaObjectId, string interfaceCode, BsonDocument bsonElements)
        {
            string applicationCode = interfaceCode.Split('.')[0];
            var triggerScripts = GetTriggerScriptsUnDeletedByMetaObjectIdAndServiceType(metaObjectId, (int)ServiceType.Interface_Add)?.Where(t => t.TriggerPoint == (int)TriggerPoint.Before).ToList();
            if (triggerScripts != null && triggerScripts.Any())
            {
                foreach (var script in triggerScripts)
                {
                    var dynamicScript = script.ToDynamicScript();
                    dynamicScript.FunctionName = FunctionName_MetaObject_Interface_Add_Before;
                    dynamicScript.ProjectName = applicationCode;
                    dynamicScript.Parameters = new[] { bsonElements };
                    var result = _scriptEngineProvider.RunScript<BsonDocument>(dynamicScript);
                    if (!result.IsSuccess && (OnFailurePolicy)script.OnFailurePolicy == OnFailurePolicy.Break)
                        throw new Exception(result.Message);
                }
            }
            return bsonElements;
        }
        public BsonDocument Run_MetaObject_Interface_Add_After(int metaObjectId, string interfaceCode, BsonDocument bsonElements)
        {
            throw new NotImplementedException();
        }
        public List<BsonDocument> Run_MetaObject_Interface_BatchAdd_Before(int metaObjectId, string interfaceCode, List<BsonDocument> bsonElementsList)
        {
            throw new NotImplementedException();
        }
        public List<BsonDocument> Run_MetaObject_Interface_BatchAdd_After(int metaObjectId, string interfaceCode, List<BsonDocument> bsonElementsList)
        {
            throw new NotImplementedException();
        }
        public FilterDefinition<BsonDocument> Run_MetaObject_Interface_Update_Before(int metaObjectId, string interfaceCode, FilterDefinition<BsonDocument> condition)
        {
            throw new NotImplementedException();
        }
        public FilterDefinition<BsonDocument> Run_MetaObject_Interface_Delete_Before(int metaObjectId, string interfaceCode, FilterDefinition<BsonDocument> condition)
        {
            throw new NotImplementedException();
        }

        public FilterDefinition<BsonDocument> Run_MetaObject_Interface_TableList_Before(int metaObjectId, string interfaceCode, FilterDefinition<BsonDocument> condition)
        {
            throw new NotImplementedException();
        }
        public TableListComponent Run_MetaObject_Interface_TableList_After(int metaObjectId, string interfaceCode, TableListComponent tableListComponent)
        {
            throw new NotImplementedException();
        }

        public FilterDefinition<BsonDocument> Run_MetaObject_Interface_SingleObject_Before(int metaObjectId, string interfaceCode, FilterDefinition<BsonDocument> condition)
        {
            throw new NotImplementedException();
        }
        public SingleObjectComponent Run_MetaObject_Interface_SingleObject_After(int metaObjectId, string interfaceCode, SingleObjectComponent singleObjectComponent)
        {
            throw new NotImplementedException();
        }

        public FilterDefinition<BsonDocument> Run_MetaObject_Interface_Count_Before(int metaObjectId, string interfaceCode, FilterDefinition<BsonDocument> condition)
        {
            throw new NotImplementedException();
        }
        public int Run_MetaObject_Interface_Count_After(int metaObjectId, string interfaceCode, int count)
        {
            throw new NotImplementedException();
        }

        public object Run_DataSource(string interfaceCode, Dictionary<string, object> argumentsDic)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 根据场景获取默认的脚本模板
        public string GetDefaultMetaObjectTriggerScriptByServiceTypeBefore(int serviceType)
        {
            switch ((ServiceType)serviceType)
            {
                case ServiceType.UI_ObjectData:
                    break;
                case ServiceType.Interface_Add: return DefaultScript_MetaObject_Interface_Add_Before;
                case ServiceType.Interface_BatchAdd: return DefaultScript_MetaObject_Interface_BatchAdd_Before;
                case ServiceType.Interface_Update: return DefaultScript_MetaObject_Interface_Update_Before;
                case ServiceType.Interface_Put: return DefaultScript_MetaObject_Interface_Put_Before;
                case ServiceType.Interface_Delete: return DefaultScript_MetaObject_Interface_Delete_Before;
                case ServiceType.Interface_TableList: return DefaultScript_MetaObject_Interface_TableList_Before;
                case ServiceType.Interface_SingleObject: return DefaultScript_MetaObject_Interface_SingleObject_Before;
                case ServiceType.Interface_Count: return DefaultScript_MetaObject_Interface_Count_Before;
                default: return string.Empty;
            }
            return string.Empty;
        }
        public string GetDefaultMetaObjectTriggerScriptByServiceTypeAfter(int serviceType)
        {
            switch ((ServiceType)serviceType)
            {
                case ServiceType.UI_ObjectData:
                    break;
                case ServiceType.Interface_Add: return DefaultScript_MetaObject_Interface_Add_After;
                case ServiceType.Interface_BatchAdd: return DefaultScript_MetaObject_Interface_BatchAdd_After;
                case ServiceType.Interface_Update: return DefaultScript_MetaObject_Interface_Update_After;
                case ServiceType.Interface_Put: return DefaultScript_MetaObject_Interface_Put_After;
                case ServiceType.Interface_Delete: return DefaultScript_MetaObject_Interface_Delete_After;
                case ServiceType.Interface_TableList: return DefaultScript_MetaObject_Interface_TableList_After;
                case ServiceType.Interface_SingleObject: return DefaultScript_MetaObject_Interface_SingleObject_After;
                case ServiceType.Interface_Count: return DefaultScript_MetaObject_Interface_Count_After;
                default: return string.Empty;
            }
            return string.Empty;
        }
        public string GetDefaultDataSourceTriggerScript() => DefaultScript_DataSource;
        #endregion

        #region 各类触发点的触发器脚本
        /// <summary>
        /// 所有脚本默认内置的通用命名空间引用，个性化的引用请写在各自脚本中
        /// </summary>
        private string DefaultCommonUsing
=> @"//命名空间
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using logger = SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Logging.Logger;";
        /// <summary>
        /// 所有脚本类内方法外内置的通用代码段，个性化请写在各自脚本中
        /// </summary>
        private string DefaultCommonClassInnerCode
=> @"//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()等
MultiTenantDataDbContext db = new MultiTenantDataDbContext();";
        /// <summary>
        /// 所有脚本方法内默认内置的通用代码段，个性化请写在各自脚本中
        /// </summary>
        private string DefaultCommonMethodCode
            => @"";
        /// <summary>
        /// 查询条件的脚本可重用
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string Get_DefaultScript_MetaObject_Interface_QueryCondition(string methodName)
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public FilterDefinition<BsonDocument> {methodName}(string interfaceCode,FilterDefinition<BsonDocument> condition)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return condition;
}}";

        const string FunctionName_MetaObject_Interface_Add_Before = "Interface_Add_Before";
        const string FunctionName_MetaObject_Interface_Add_After = "Interface_Add_After";
        const string FunctionName_MetaObject_Interface_BatchAdd_Before = "Interface_BatchAdd_Before";
        const string FunctionName_MetaObject_Interface_BatchAdd_After = "Interface_BatchAdd_After";
        const string FunctionName_MetaObject_Interface_Update_Before = "Interface_Update_Before";
        const string FunctionName_MetaObject_Interface_Update_After = "Interface_Update_After";
        const string FunctionName_MetaObject_Interface_Put_Before = "Interface_Put_Before";
        const string FunctionName_MetaObject_Interface_Put_After = "Interface_Put_After";
        const string FunctionName_MetaObject_Interface_Delete_Before = "Interface_Delete_Before";
        const string FunctionName_MetaObject_Interface_Delete_After = "Interface_Delete_After";
        const string FunctionName_MetaObject_Interface_TableList_Before = "Interface_TableList_Before";
        const string FunctionName_MetaObject_Interface_TableList_After = "Interface_TableList_After";
        const string FunctionName_MetaObject_Interface_SingleObject_Before = "Interface_SingleObject_Before";
        const string FunctionName_MetaObject_Interface_SingleObject_After = "Interface_SingleObject_After";
        const string FunctionName_MetaObject_Interface_Count_Before = "Interface_Count_Before";
        const string FunctionName_MetaObject_Interface_Count_After = "Interface_Count_After";
        const string FunctionName_DataSource = "TriggerScriptDataSource";

        private string DefaultScript_MetaObject_Interface_Add_Before
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public BsonDocument {FunctionName_MetaObject_Interface_Add_Before}(string interfaceCode,BsonDocument bsonElements)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return bsonElements;
}}";
        private string DefaultScript_MetaObject_Interface_Add_After
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public BsonDocument {FunctionName_MetaObject_Interface_Add_After}(string interfaceCode,BsonDocument bsonElements)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return bsonElements;
}}";
        private string DefaultScript_MetaObject_Interface_BatchAdd_Before
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public List<BsonDocument> {FunctionName_MetaObject_Interface_BatchAdd_Before}(string interfaceCode,List<BsonDocument> bsonElementsList)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return bsonElementsList;
}}";
        private string DefaultScript_MetaObject_Interface_BatchAdd_After
            => $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public List<BsonDocument> {FunctionName_MetaObject_Interface_BatchAdd_After}(string interfaceCode,List<BsonDocument> bsonElementsList)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return bsonElementsList;
}}";
        private string DefaultScript_MetaObject_Interface_Update_Before
            => string.Empty;
        private string DefaultScript_MetaObject_Interface_Update_After => string.Empty;
        private string DefaultScript_MetaObject_Interface_Put_Before => string.Empty;
        private string DefaultScript_MetaObject_Interface_Put_After => string.Empty;
        private string DefaultScript_MetaObject_Interface_Delete_Before => string.Empty;
        private string DefaultScript_MetaObject_Interface_Delete_After => string.Empty;
        private string DefaultScript_MetaObject_Interface_TableList_Before
=> Get_DefaultScript_MetaObject_Interface_QueryCondition("Interface_TableList_Before");
        private string DefaultScript_MetaObject_Interface_TableList_After
=> $@"{DefaultCommonUsing}
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.ListView;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.UserInfo;
{DefaultCommonClassInnerCode}
public TableListComponent {FunctionName_MetaObject_Interface_TableList_After}(string interfaceCode, TableListComponent tableListComponent)
{{
    { DefaultCommonMethodCode}
    //这里写业务逻辑
    //...
    return tableListComponent;
}}";
        private string DefaultScript_MetaObject_Interface_SingleObject_Before
=> Get_DefaultScript_MetaObject_Interface_QueryCondition("Interface_SingleObject_Before");
        private string DefaultScript_MetaObject_Interface_SingleObject_After
=> $@"{DefaultCommonUsing}
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.ListView;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.UserInfo;
{DefaultCommonClassInnerCode}
public SingleObjectComponent {FunctionName_MetaObject_Interface_SingleObject_After}(string interfaceCode,SingleObjectComponent singleObjectComponent)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return singleObjectComponent;
}}";
        private string DefaultScript_MetaObject_Interface_Count_Before
=> Get_DefaultScript_MetaObject_Interface_QueryCondition("Interface_Count_Before");
        private string DefaultScript_MetaObject_Interface_Count_After
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public int {FunctionName_MetaObject_Interface_Count_After}(string interfaceCode,int count)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return count;
}}";

        private string DefaultScript_DataSource
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public object {FunctionName_DataSource}(string operateCode, Dictionary<string, object> argumentsDic)
{{
    {DefaultCommonMethodCode}
	//这里写业务逻辑
	//...
	return null;
}}";
        #endregion
    }
}
