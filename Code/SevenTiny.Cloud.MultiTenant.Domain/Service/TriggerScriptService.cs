using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using SevenTiny.Cloud.ScriptEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class TriggerScriptService : MetaObjectCommonServiceBase<TriggerScript>, ITriggerScriptService
    {
        public TriggerScriptService(IDataSourceRepository dataSourceRepository, IDynamicScriptEngine dynamicScriptEngine, IDataSourceService dataSourceService, ITriggerScriptRepository triggerScriptRepository) : base(triggerScriptRepository)
        {
            _dynamicScriptEngine = dynamicScriptEngine;
            _dataSourceService = dataSourceService;
            _triggerScriptRepository = triggerScriptRepository;
            _dataSourceRepository = dataSourceRepository;
        }

        readonly IDynamicScriptEngine _dynamicScriptEngine;
        readonly IDataSourceService _dataSourceService;
        ITriggerScriptRepository _triggerScriptRepository;
        IDataSourceRepository _dataSourceRepository;

        public new Result Add(TriggerScript triggerScript)
        {
            triggerScript.ScriptType = (int)ScriptType.MetaObject;
            triggerScript.ServiceMethod = (int)ServiceMethod.Post;
            return base.Add(triggerScript);
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="source"></param>
        public new Result Update(TriggerScript source)
        {
            return base.UpdateWithOutCode(source, _ =>
            {
                //编码不允许修改
                source.ServiceType = source.ServiceType;
                source.ServiceMethod = source.ServiceMethod;
                source.TriggerPoint = source.TriggerPoint;
                source.Language = source.Language;
                source.OnFailurePolicy = source.OnFailurePolicy;
                source.Script = source.Script;
            });
        }

        public Result CompilateAndCheckScript(string script, string applicationCode)
        {
            var dynamicScript = new DynamicScript
            {
                Script = script,
                Language = DynamicScriptLanguage.Csharp,
                FunctionName = "Test",//这里
            };

            var result = _dynamicScriptEngine.CheckScript(dynamicScript);

            if (result.IsSuccess)
                return Result.Success(result.Message);

            return Result.Error(result.Message);
        }

        public List<TriggerScript> GetTriggerScriptListUnDeletedByMetaObjectIdAndServiceType(Guid metaObjectId, int serviceType)
        {
            return _triggerScriptRepository.GetUnDeletedListByMetaObjectIdAndServiceType(metaObjectId, serviceType);
        }

        public List<TriggerScript> GetUnDeletedListByMetaObjectIdAndServiceType(Guid metaObjectId, int serviceType)
        {
            return _triggerScriptRepository.GetUnDeletedListByMetaObjectIdAndServiceType(metaObjectId, serviceType);
        }

        #region Execute Script
        public T RunTriggerScript<T>(QueryPiplineContext queryPiplineContext, TriggerPoint triggerPoint, string functionName, T result, params object[] parameters)
        {
            var triggerScripts = queryPiplineContext.TriggerScriptsOfOneServiceType?.Where(t => t.TriggerPoint == (int)triggerPoint).ToList();
            if (triggerScripts != null && triggerScripts.Any())
            {
                foreach (var script in triggerScripts)
                {
                    var dynamicScript = script.ToDynamicScript();
                    dynamicScript.FunctionName = functionName;
                    dynamicScript.Parameters = parameters;
                    var executeResult = _dynamicScriptEngine.Execute<T>(dynamicScript);
                    if (!executeResult.IsSuccess && (OnFailurePolicy)script.OnFailurePolicy == OnFailurePolicy.Break)
                        throw new Exception(executeResult.Message);
                    else
                        result = executeResult.Data;
                }
            }
            return result;
        }

        public object RunDataSourceScript(QueryPiplineContext queryPiplineContext, params object[] parameters)
        {
            var dataSource = _dataSourceRepository.GetById(queryPiplineContext.DataSourceId);
            if (dataSource != null)
            {
                var dynamicScript = new DynamicScript();
                dynamicScript.Script = dataSource.Script;
                dynamicScript.Language = DynamicScriptLanguage.Csharp;
                dynamicScript.FunctionName = FunctionName_DataSource;
                dynamicScript.Parameters = parameters;
                var executeResult = _dynamicScriptEngine.Execute<object>(dynamicScript);
                if (executeResult.IsSuccess)
                    return executeResult.Data;
                else
                    throw new Exception(executeResult.Message);
            }
            return null;
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
using SevenTiny.Bantina.Logging;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Context;
using SevenTiny.Cloud.MultiTenant.UI.DataAccess;";
        /// <summary>
        /// 所有脚本类内方法外内置的通用代码段，个性化请写在各自脚本中
        /// </summary>
        private string DefaultCommonClassInnerCode
=> @"//EndUsing
//注释：上面的 EndUsing 注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()...；
ILog logger = new LogManager();
//注释：查询数据请使用下面的db；
MultiTenantDataDbContext db = new MultiTenantDataDbContext();";
        /// <summary>
        /// 所有脚本方法内默认内置的通用代码段，个性化请写在各自脚本中
        /// </summary>
        private string DefaultCommonMethodCode
            => @"";

        public const string FunctionName_MetaObject_Interface_Add_Before = "Interface_Add_Before";
        public const string FunctionName_MetaObject_Interface_Add_After = "Interface_Add_After";
        public const string FunctionName_MetaObject_Interface_BatchAdd_Before = "Interface_BatchAdd_Before";
        public const string FunctionName_MetaObject_Interface_BatchAdd_After = "Interface_BatchAdd_After";
        public const string FunctionName_MetaObject_Interface_Update_Before = "Interface_Update_Before";
        public const string FunctionName_MetaObject_Interface_Update_After = "Interface_Update_After";
        public const string FunctionName_MetaObject_Interface_Delete_Before = "Interface_Delete_Before";
        public const string FunctionName_MetaObject_Interface_Delete_After = "Interface_Delete_After";
        public const string FunctionName_MetaObject_Interface_TableList_Before = "Interface_TableList_Before";
        public const string FunctionName_MetaObject_Interface_TableList_After = "Interface_TableList_After";
        public const string FunctionName_MetaObject_Interface_SingleObject_Before = "Interface_SingleObject_Before";
        public const string FunctionName_MetaObject_Interface_SingleObject_After = "Interface_SingleObject_After";
        public const string FunctionName_MetaObject_Interface_Count_Before = "Interface_Count_Before";
        public const string FunctionName_MetaObject_Interface_Count_After = "Interface_Count_After";
        public const string FunctionName_DataSource = "TriggerScriptDataSource";

        /// <summary>
        /// 查询条件的脚本可重用
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string Get_DefaultScript_MetaObject_Interface_QueryCondition(string methodName)
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public FilterDefinition<BsonDocument> {methodName}(ApplicationContext applicationContext,string interfaceCode,FilterDefinition<BsonDocument> condition)
{{
    {DefaultCommonMethodCode}
	return condition;
}}";

        private string DefaultScript_MetaObject_Interface_Add_Before
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public BsonDocument {FunctionName_MetaObject_Interface_Add_Before}(ApplicationContext applicationContext,string interfaceCode,BsonDocument bsonElements)
{{
    {DefaultCommonMethodCode}
	return bsonElements;
}}";
        private string DefaultScript_MetaObject_Interface_Add_After
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public BsonDocument {FunctionName_MetaObject_Interface_Add_After}(ApplicationContext applicationContext,string interfaceCode,BsonDocument bsonElements)
{{
    {DefaultCommonMethodCode}
	return bsonElements;
}}";
        private string DefaultScript_MetaObject_Interface_BatchAdd_Before
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public List<BsonDocument> {FunctionName_MetaObject_Interface_BatchAdd_Before}(ApplicationContext applicationContext,string interfaceCode,List<BsonDocument> bsonElementsList)
{{
    {DefaultCommonMethodCode}
	return bsonElementsList;
}}";
        private string DefaultScript_MetaObject_Interface_BatchAdd_After
            => $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public List<BsonDocument> {FunctionName_MetaObject_Interface_BatchAdd_After}(ApplicationContext applicationContext,string interfaceCode,List<BsonDocument> bsonElementsList)
{{
    {DefaultCommonMethodCode}
	return bsonElementsList;
}}";
        private string DefaultScript_MetaObject_Interface_Update_Before
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public FilterDefinition<BsonDocument> {FunctionName_MetaObject_Interface_Update_Before}(ApplicationContext applicationContext,string interfaceCode,FilterDefinition<BsonDocument> condition,BsonDocument bsonElements)
{{
    {DefaultCommonMethodCode}
	return condition;
}}";
        private string DefaultScript_MetaObject_Interface_Update_After
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public void {FunctionName_MetaObject_Interface_Update_After}(ApplicationContext applicationContext,string interfaceCode,FilterDefinition<BsonDocument> condition,BsonDocument bsonElements)
{{
    {DefaultCommonMethodCode}
}}";
        private string DefaultScript_MetaObject_Interface_Delete_Before
=> Get_DefaultScript_MetaObject_Interface_QueryCondition(FunctionName_MetaObject_Interface_Delete_Before);
        private string DefaultScript_MetaObject_Interface_Delete_After
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public void {FunctionName_MetaObject_Interface_Delete_After}(ApplicationContext applicationContext,string interfaceCode,List<BsonDocument> bsonElementsList)
{{
    {DefaultCommonMethodCode}
}}";
        private string DefaultScript_MetaObject_Interface_TableList_Before
=> Get_DefaultScript_MetaObject_Interface_QueryCondition(FunctionName_MetaObject_Interface_TableList_Before);
        private string DefaultScript_MetaObject_Interface_TableList_After
=> $@"{DefaultCommonUsing}
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.UserInfo;
{DefaultCommonClassInnerCode}
public TableListComponent {FunctionName_MetaObject_Interface_TableList_After}(ApplicationContext applicationContext,string interfaceCode, TableListComponent tableListComponent)
{{
    { DefaultCommonMethodCode}
    return tableListComponent;
}}";
        private string DefaultScript_MetaObject_Interface_SingleObject_Before
=> Get_DefaultScript_MetaObject_Interface_QueryCondition(FunctionName_MetaObject_Interface_SingleObject_Before);
        private string DefaultScript_MetaObject_Interface_SingleObject_After
=> $@"{DefaultCommonUsing}
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.UserInfo;
{DefaultCommonClassInnerCode}
public SingleObjectComponent {FunctionName_MetaObject_Interface_SingleObject_After}(ApplicationContext applicationContext,string interfaceCode,SingleObjectComponent singleObjectComponent)
{{
    {DefaultCommonMethodCode}
	return singleObjectComponent;
}}";
        private string DefaultScript_MetaObject_Interface_Count_Before
=> Get_DefaultScript_MetaObject_Interface_QueryCondition(FunctionName_MetaObject_Interface_Count_Before);
        private string DefaultScript_MetaObject_Interface_Count_After
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public int {FunctionName_MetaObject_Interface_Count_After}(ApplicationContext applicationContext,string interfaceCode,FilterDefinition<BsonDocument> condition,int count)
{{
    {DefaultCommonMethodCode}
	return count;
}}";

        private string DefaultScript_DataSource
=> $@"{DefaultCommonUsing}
{DefaultCommonClassInnerCode}
public object {FunctionName_DataSource}(ApplicationContext applicationContext,string interfaceCode, Dictionary<string, object> argumentsDic)
{{
    {DefaultCommonMethodCode}
	return null;
}}";
        #endregion
    }
}
