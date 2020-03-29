using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.Service;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Context;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    //[Produces("application/json")]
    [EnableCors("AllowSameDomain")]
    [Route("api/CloudData")]
    [ApiController]
    public class CloudDataController : ApiControllerBase
    {
        public CloudDataController(
            IDataAccessService _dataAccessService,
            ISearchConditionNodeService _conditionAggregationService,
            IInterfaceAggregationService _interfaceAggregationService,
            ITriggerScriptService triggerScriptService,
            IDataSourceService dataSourceService,
            IMetaObjectService _metaObjectService,
            IMetaFieldService _metaFieldService,
            IFormMetaFieldService formMetaFieldService,
            IFieldListMetaFieldService fieldListMetaFieldService
            )
        {
            dataAccessService = _dataAccessService;
            conditionAggregationService = _conditionAggregationService;
            interfaceAggregationService = _interfaceAggregationService;
            _triggerScriptService = triggerScriptService;
            metaObjectService = _metaObjectService;
            metaFieldService = _metaFieldService;
            _dataSourceService = dataSourceService;
            _formMetaFieldService = formMetaFieldService;
            _fieldListMetaFieldService = fieldListMetaFieldService;
        }

        readonly IDataAccessService dataAccessService;
        readonly IInterfaceAggregationService interfaceAggregationService;
        readonly ISearchConditionNodeService conditionAggregationService;
        readonly ITriggerScriptService _triggerScriptService;
        readonly IDataSourceService _dataSourceService;
        readonly IMetaObjectService metaObjectService;
        readonly IMetaFieldService metaFieldService;
        readonly IFormMetaFieldService _formMetaFieldService;
        readonly IFieldListMetaFieldService _fieldListMetaFieldService;

        /// <summary>
        /// 1.预处理统一的参数验证逻辑
        /// 2.根据接口信息创建查询管道上下文
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        private QueryPiplineContext PretreatmentAndCreateQueryPiplineContext(QueryArgs queryArgs)
        {
            //args check
            if (queryArgs == null)
                throw new Exception($"Parameter invalid:queryArgs = null");

            queryArgs.QueryArgsCheck();

            //argumentsDic generate
            Dictionary<string, object> argumentsDic = new Dictionary<string, object>();
            foreach (var item in Request.Query)
                argumentsDic.AddOrUpdate(item.Key.ToUpperInvariant(), item.Value);

            //get interface info
            var interfaceAggregation = interfaceAggregationService.GetByInterfaceAggregationCode(queryArgs._interface);
            if (interfaceAggregation == null)
                throw new Exception($"未能找到接口编码为[{queryArgs._interface}]对应的接口信息");

            //create queryContext
            QueryPiplineContext queryContext = new QueryPiplineContext();
            queryContext.InterfaceCode = queryArgs._interface;
            queryContext.ArgumentsDic = argumentsDic;
            queryContext.ApplicationCode = interfaceAggregation.Code.Split('.')[0];
            queryContext.MetaObjectId = interfaceAggregation.MetaObjectId;
            queryContext.SearchConditionId = interfaceAggregation.SearchConditionId;
            queryContext.FieldListId = interfaceAggregation.FieldListId;
            queryContext.DataSourceId = interfaceAggregation.DataSourceId;
            queryContext.InterfaceType = (InterfaceType)interfaceAggregation.InterfaceType;

            //设置ApplicationCode到Session
            SetApplictionCodeToSession(queryContext.ApplicationCode);

            return queryContext;
        }

        [HttpGet]
        public IActionResult Get([FromQuery]QueryArgs queryArgs)
        {
            try
            {
                //Pretreatment create queryContext
                QueryPiplineContext queryContext = PretreatmentAndCreateQueryPiplineContext(queryArgs);

                //查询条件
                FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;

                //【SingleObject,TableList,Count】预处理一些逻辑
                if (new InterfaceType[] { InterfaceType.SingleObject, InterfaceType.TableList, InterfaceType.Count }.Contains(queryContext.InterfaceType))
                {
                    //缓存对象下的全部未删除字段信息
                    queryContext.MetaFieldsUnDeleted = metaFieldService.GetEntitiesUnDeletedByMetaObjectId(queryContext.MetaObjectId);
                    //组织查询条件
                    filter = conditionAggregationService.AnalysisConditionToFilterDefinitionByConditionId(queryContext);
                    //缓存列字段信息
                    if (queryContext.InterfaceType == InterfaceType.SingleObject || queryContext.InterfaceType == InterfaceType.TableList)
                    {
                        queryContext.FieldListMetaFieldsOfFieldListId = _fieldListMetaFieldService.GetByFieldListId(queryContext.FieldListId);
                    }
                }

                switch (queryContext.InterfaceType)
                {
                    case InterfaceType.SingleObject:
                        //缓存某个服务下的全部触发器脚本，包括before和after
                        queryContext.TriggerScriptsOfOneServiceType = _triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndServiceType(queryContext.MetaObjectId, (int)ServiceType.Interface_SingleObject);
                        filter = _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_SingleObject_Before, filter, CurrentApplicationContext, queryContext.InterfaceCode, filter);
                        var singleObjectComponent = dataAccessService.GetSingleObjectComponent(queryContext, filter);
                        singleObjectComponent = _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_SingleObject_After, singleObjectComponent, CurrentApplicationContext, queryContext.InterfaceCode, singleObjectComponent);
                        return JsonResultModel.Success("get single data success", singleObjectComponent);
                    case InterfaceType.TableList:
                        //缓存某个服务下的全部触发器脚本，包括before和after
                        queryContext.TriggerScriptsOfOneServiceType = _triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndServiceType(queryContext.MetaObjectId, (int)ServiceType.Interface_TableList);
                        filter = _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_TableList_Before, filter, CurrentApplicationContext, queryContext.InterfaceCode, filter);
                        var sort = metaFieldService.GetSortDefinitionBySortFields(queryContext, new[] { new SortField { Column = "ModifyTime", IsDesc = true } });
                        var tableListComponent = dataAccessService.GetTableListComponent(queryContext, filter, queryArgs._pageIndex, queryArgs._pageSize, sort, out int totalCount);
                        tableListComponent = _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_TableList_After, tableListComponent, CurrentApplicationContext, queryContext.InterfaceCode, tableListComponent);
                        return JsonResultModel.Success("get data list success", tableListComponent);
                    case InterfaceType.Count:
                        //缓存某个服务下的全部触发器脚本，包括before和after
                        queryContext.TriggerScriptsOfOneServiceType = _triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndServiceType(queryContext.MetaObjectId, (int)ServiceType.Interface_Count);
                        filter = _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_Count_Before, filter, CurrentApplicationContext, queryContext.InterfaceCode, filter);
                        var count = dataAccessService.GetCount(queryContext.TenantId, queryContext.MetaObjectId, filter);
                        count = _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_Count_After, count, CurrentApplicationContext, queryContext.InterfaceCode, filter, count);
                        return JsonResultModel.Success("get data count success", count);
                    case InterfaceType.JsonDataSource:
                        return new JsonResult(Newtonsoft.Json.JsonConvert.DeserializeObject(_dataSourceService.GetById(queryContext.DataSourceId).Script));
                    case InterfaceType.ExecutableScriptDataSource:
                        object triggerScriptDataSourceResult = _triggerScriptService.RunDataSourceScript(queryContext, CurrentApplicationContext, queryContext.InterfaceCode, queryContext.ArgumentsDic);
                        return JsonResultModel.Success("get trigger script result success", triggerScriptDataSourceResult);
                    default:
                        break;
                }

                return JsonResultModel.Success("success,no data");
            }
            catch (ArgumentNullException argNullEx)
            {
                return JsonResultModel.Error(argNullEx.Message);
            }
            catch (ArgumentException argEx)
            {
                return JsonResultModel.Error(argEx.Message);
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.Message);
            }
        }

        /**
         Content-Type: application/json
         * */
        [HttpPost]
        public IActionResult Post([FromQuery]QueryArgs queryArgs, [FromBody]JObject jObj)
        {
            try
            {
                var json = jObj.ToString();
                var bson = BsonDocument.Parse(json);
                if (bson == null || !bson.Any())
                    return JsonResultModel.Error("Parameter invalid:jObj = null 业务数据为空");

                //Pretreatment create queryContext
                QueryPiplineContext queryContext = PretreatmentAndCreateQueryPiplineContext(queryArgs);
                queryContext.MetaObject = metaObjectService.GetById(queryContext.MetaObjectId);

                if (queryContext.MetaObject == null)
                    return JsonResultModel.Error($"未能找到对象Id为[{queryContext.MetaObjectId}]对应的对象信息");

                //缓存某个服务下的全部触发器脚本，包括before和after
                queryContext.TriggerScriptsOfOneServiceType = _triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndServiceType(queryContext.MetaObjectId, (int)ServiceType.Interface_Add);

                //trigger before
                bson = _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_Add_Before, bson, CurrentApplicationContext, queryContext.InterfaceCode, bson);

                //check data by form
                if (queryContext.FormId != default(int))
                {
                    var formCheckResult = _formMetaFieldService.ValidateFormData(queryContext.FormId, bson);
                    if (!formCheckResult.IsSuccess)
                        return formCheckResult.ToJsonResultModel();
                }

                //add data
                var addResult = dataAccessService.Add(queryContext.TenantId, queryContext.MetaObject, bson);

                //trigger after
                _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_Add_After, bson, CurrentApplicationContext, queryContext.InterfaceCode, bson);

                return addResult.ToJsonResultModel();
            }
            catch (ArgumentNullException argNullEx)
            {
                return JsonResultModel.Error(argNullEx.Message);
            }
            catch (ArgumentException argEx)
            {
                return JsonResultModel.Error(argEx.Message);
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.Message);
            }
        }

        /**
         Content-Type: application/json
         * */
        [HttpPut]
        public IActionResult Update([FromQuery]QueryArgs queryArgs, [FromBody]JObject jObj)
        {
            try
            {
                var json = jObj.ToString();
                var bson = BsonDocument.Parse(json);
                if (bson == null || !bson.Any())
                    return JsonResultModel.Error("Parameter invalid:jObj = null 业务数据为空");

                //Pretreatment create queryContext
                QueryPiplineContext queryContext = PretreatmentAndCreateQueryPiplineContext(queryArgs);

                //缓存某个服务下的全部触发器脚本，包括before和after
                queryContext.TriggerScriptsOfOneServiceType = _triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndServiceType(queryContext.MetaObjectId, (int)ServiceType.Interface_Update);

                //查询条件
                FilterDefinition<BsonDocument> filter = conditionAggregationService.AnalysisConditionToFilterDefinitionByConditionId(queryContext);

                //trigger before
                bson = _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_Update_Before, bson, CurrentApplicationContext, queryContext.InterfaceCode, bson, filter);

                //check data by form
                if (queryContext.FormId != default(int))
                {
                    var formCheckResult = _formMetaFieldService.ValidateFormData(queryContext.FormId, bson);
                    if (!formCheckResult.IsSuccess)
                        return formCheckResult.ToJsonResultModel();
                }

                //update data
                dataAccessService.Update(queryContext.TenantId, queryContext.MetaObjectId, filter, bson);

                //trigger after
                _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_Update_After, bson, CurrentApplicationContext, queryContext.InterfaceCode, bson);

                return JsonResultModel.Success("success");
            }
            catch (ArgumentNullException argNullEx)
            {
                return JsonResultModel.Error(argNullEx.Message);
            }
            catch (ArgumentException argEx)
            {
                return JsonResultModel.Error(argEx.Message);
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery]QueryArgs queryArgs)
        {
            try
            {
                //Pretreatment create queryContext
                QueryPiplineContext queryContext = PretreatmentAndCreateQueryPiplineContext(queryArgs);

                //缓存某个服务下的全部触发器脚本，包括before和after
                queryContext.TriggerScriptsOfOneServiceType = _triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndServiceType(queryContext.MetaObjectId, (int)ServiceType.Interface_Delete);

                //查询条件
                FilterDefinition<BsonDocument> filter = conditionAggregationService.AnalysisConditionToFilterDefinitionByConditionId(queryContext);

                //trigger before
                filter = _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_Delete_Before, filter, CurrentApplicationContext, queryContext.InterfaceCode, filter);

                //queryResult       
                var queryDatas = dataAccessService.GetList(queryContext.TenantId, queryContext.MetaObjectId, filter, null);

                //delete
                dataAccessService.Delete(queryContext.TenantId, queryContext.MetaObjectId, filter);

                //trigger after
                _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_Delete_After, filter, CurrentApplicationContext, queryContext.InterfaceCode, queryDatas);

                return JsonResultModel.Success("success");
            }
            catch (ArgumentNullException argNullEx)
            {
                return JsonResultModel.Error(argNullEx.Message);
            }
            catch (ArgumentException argEx)
            {
                return JsonResultModel.Error(argEx.Message);
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.Message);
            }
        }
    }
}