using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    //[Produces("application/json")]
    [EnableCors("AllowSameDomain")]
    [Route("api/CloudData")]
    [ApiController]
    public class CloudDataController : ControllerBase
    {
        public CloudDataController(
            IDataAccessService _dataAccessService,
            ISearchConditionService _searchConditionService,
            ISearchConditionNodeService _conditionAggregationService,
            IInterfaceAggregationService _interfaceAggregationService,
            IFieldBizDataService _fieldBizDataService,
            ITriggerScriptExecuteService _triggerScriptEngineService,
            IMetaObjectService _metaObjectService,
            IMetaFieldService _metaFieldService
            )
        {
            dataAccessService = _dataAccessService;
            conditionAggregationService = _conditionAggregationService;
            interfaceAggregationService = _interfaceAggregationService;
            fieldBizDataService = _fieldBizDataService;
            triggerScriptEngineService = _triggerScriptEngineService;
            searchConditionService = _searchConditionService;
            metaObjectService = _metaObjectService;
            metaFieldService = _metaFieldService;
        }

        readonly IDataAccessService dataAccessService;
        readonly IInterfaceAggregationService interfaceAggregationService;
        readonly ISearchConditionNodeService conditionAggregationService;
        readonly IFieldBizDataService fieldBizDataService;
        readonly ITriggerScriptExecuteService triggerScriptEngineService;
        readonly ISearchConditionService searchConditionService;
        readonly IMetaObjectService metaObjectService;
        readonly IMetaFieldService metaFieldService;

        [HttpGet]
        public IActionResult Get([FromQuery]QueryArgs queryArgs)
        {
            try
            {
                //args check
                if (queryArgs == null)
                {
                    return JsonResultModel.Error($"Parameter invalid:queryArgs = null");
                }

                var checkResult = queryArgs.QueryArgsCheck();

                if (!checkResult.IsSuccess)
                {
                    return checkResult.ToJsonResultModel();
                }

                //argumentsDic generate
                Dictionary<string, object> argumentsDic = new Dictionary<string, object>();
                foreach (var item in Request.Query)
                {
                    if (!argumentsDic.ContainsKey(item.Key))
                    {
                        argumentsDic.Add(item.Key.ToUpperInvariant(), item.Value);
                    }
                }

                //get filter
                var interfaceAggregation = interfaceAggregationService.GetByInterfaceAggregationCode(queryArgs.interfaceCode);
                if (interfaceAggregation == null)
                {
                    return JsonResultModel.Error($"未能找到接口编码为[{queryArgs.interfaceCode}]对应的接口信息");
                }
                var filter = conditionAggregationService.AnalysisConditionToFilterDefinitionByConditionId(interfaceAggregation.MetaObjectId, interfaceAggregation.SearchConditionId, argumentsDic);

                //get result
                switch ((InterfaceType)interfaceAggregation.InterfaceType)
                {
                    case InterfaceType.CloudSingleObject:
                        filter = triggerScriptEngineService.SingleObjectBefore(interfaceAggregation.MetaObjectId, interfaceAggregation.Code, filter);
                        var singleObjectComponent = dataAccessService.GetSingleObjectComponent(interfaceAggregation.MetaObjectId, interfaceAggregation.FieldListId, filter);
                        singleObjectComponent = triggerScriptEngineService.SingleObjectAfter(interfaceAggregation.MetaObjectId, interfaceAggregation.Code, singleObjectComponent);
                        return JsonResultModel.Success("get single data success", singleObjectComponent);
                    case InterfaceType.CloudTableList:
                        filter = triggerScriptEngineService.TableListBefore(interfaceAggregation.MetaObjectId, interfaceAggregation.Code, filter);
                        var sort = metaFieldService.GetSortDefinitionBySortFields(interfaceAggregation.MetaObjectId, null);
                        var tableListComponent = dataAccessService.GetTableListComponent(interfaceAggregation.MetaObjectId, interfaceAggregation.FieldListId, filter, queryArgs.pageIndex, queryArgs.pageSize, sort, out int totalCount);
                        tableListComponent = triggerScriptEngineService.TableListAfter(interfaceAggregation.MetaObjectId, interfaceAggregation.Code, tableListComponent);
                        return JsonResultModel.Success("get data list success", tableListComponent);
                    case InterfaceType.CloudCount:
                        filter = triggerScriptEngineService.CountBefore(interfaceAggregation.MetaObjectId, interfaceAggregation.Code, filter);
                        var count = dataAccessService.GetCount(interfaceAggregation.MetaObjectId, filter);
                        count = triggerScriptEngineService.CountAfter(interfaceAggregation.MetaObjectId, interfaceAggregation.Code, count);
                        return JsonResultModel.Success("get data count success", count);
                    case InterfaceType.EnumeDataSource:
                        break;
                    case InterfaceType.TriggerScriptDataSource:
                        object triggerScriptDataSourceResult = triggerScriptEngineService.TriggerScriptDataSource(interfaceAggregation.Code, argumentsDic, interfaceAggregation.Script);
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
        public IActionResult Post(string metaObjectCode, [FromBody]JObject jObj)
        {
            try
            {
                if (string.IsNullOrEmpty(metaObjectCode))
                    return JsonResultModel.Error($"Parameter invalid:metaObjectCode = null");
                if (jObj == null)
                    return JsonResultModel.Error($"Parameter invalid:data = null");

                var json = jObj.ToString();
                var bson = BsonDocument.Parse(json);

                //get metaObject
                var metaObject = metaObjectService.GetByCode(metaObjectCode);
                if (metaObject == null)
                {
                    return JsonResultModel.Error($"未能找到对象编码为[{metaObjectCode}]对应的对象信息");
                }

                //trigger before
                bson = triggerScriptEngineService.AddBefore(metaObject.Id, metaObjectCode, bson);

                //add data
                var addResult = dataAccessService.Add(metaObject, bson);

                return addResult.ToJsonResultModel();
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }

        /**
         Content-Type: application/json
         * */
        [HttpPut]
        public IActionResult Update(string conditionCode, [FromBody]JObject jObj)
        {
            try
            {
                //args check
                if (string.IsNullOrEmpty(conditionCode))
                    return JsonResultModel.Error($"Parameter invalid:conditionCode = null");
                if (jObj == null)
                    return JsonResultModel.Error($"Parameter invalid:data = null");

                //argumentsDic generate
                Dictionary<string, object> argumentsDic = new Dictionary<string, object>();
                foreach (var item in Request.Query)
                {
                    if (!argumentsDic.ContainsKey(item.Key))
                    {
                        argumentsDic.Add(item.Key.ToUpperInvariant(), item.Value);
                    }
                }

                //get filter
                var searchCondition = searchConditionService.GetByCode(conditionCode);
                if (searchCondition == null)
                {
                    return JsonResultModel.Error($"SearchCondition not found by conditionCode[{searchCondition}]");
                }
                var filter = conditionAggregationService.AnalysisConditionToFilterDefinitionByConditionId(searchCondition.MetaObjectId, searchCondition.Id, argumentsDic);

                //get object
                var json = jObj.ToString();
                var bson = BsonDocument.Parse(json);

                //update before
                filter = triggerScriptEngineService.UpdateBefore(searchCondition.MetaObjectId, searchCondition.Code, filter);

                //update data
                dataAccessService.Update(searchCondition.MetaObjectId, filter, bson);

                return JsonResultModel.Success("success");
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }

        [HttpDelete]
        public IActionResult Delete(string conditionCode)
        {
            try
            {
                //args check
                if (string.IsNullOrEmpty(conditionCode))
                    return JsonResultModel.Error($"Parameter invalid:conditionCode = null");

                //argumentsDic generate
                Dictionary<string, object> argumentsDic = new Dictionary<string, object>();
                foreach (var item in Request.Query)
                {
                    if (!argumentsDic.ContainsKey(item.Key))
                    {
                        argumentsDic.Add(item.Key.ToUpperInvariant(), item.Value);
                    }
                }

                //get filter
                var searchCondition = searchConditionService.GetByCode(conditionCode);
                if (searchCondition == null)
                {
                    return JsonResultModel.Error($"SearchCondition not found by conditionCode[{searchCondition}]");
                }
                var filter = conditionAggregationService.AnalysisConditionToFilterDefinitionByConditionId(searchCondition.MetaObjectId, searchCondition.Id, argumentsDic);

                //delete before
                filter = triggerScriptEngineService.UpdateBefore(searchCondition.MetaObjectId, searchCondition.Code, filter);

                //delete
                dataAccessService.Delete(searchCondition.MetaObjectId, filter);

                return JsonResultModel.Success("success");
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }
    }
}