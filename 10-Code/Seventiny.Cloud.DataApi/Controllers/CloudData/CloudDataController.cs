using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Service;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using Seventiny.Cloud.DataApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seventiny.Cloud.DataApi.Controllers
{
    //[Produces("application/json")]
    [EnableCors("AllowSameDomain")]
    [Route("api/CloudData")]
    [ApiController]
    public class CloudDataController : ControllerBase
    {
        public CloudDataController(
            IDataAccessService _dataAccessService,
            ISearchConditionNodeService _conditionAggregationService,
            IInterfaceAggregationService _interfaceAggregationService,
            ITriggerScriptService triggerScriptService,
            IDataSourceService dataSourceService,
            IMetaObjectService _metaObjectService,
            IMetaFieldService _metaFieldService,
            IFormMetaFieldService formMetaFieldService
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
        }

        readonly IDataAccessService dataAccessService;
        readonly IInterfaceAggregationService interfaceAggregationService;
        readonly ISearchConditionNodeService conditionAggregationService;
        readonly ITriggerScriptService _triggerScriptService;
        readonly IDataSourceService _dataSourceService;
        readonly IMetaObjectService metaObjectService;
        readonly IMetaFieldService metaFieldService;
        readonly IFormMetaFieldService _formMetaFieldService;

        [HttpGet]
        public IActionResult Get([FromQuery]QueryArgs queryArgs)
        {
            try
            {
                //args check
                if (queryArgs == null)
                    return JsonResultModel.Error($"Parameter invalid:queryArgs = null");

                var checkResult = queryArgs.QueryArgsCheck();
                if (!checkResult.IsSuccess)
                    return checkResult.ToJsonResultModel();

                //argumentsDic generate
                Dictionary<string, object> argumentsDic = new Dictionary<string, object>();
                foreach (var item in Request.Query)
                    argumentsDic.AddOrUpdate(item.Key.ToUpperInvariant(), item.Value);

                //get interface info
                var interfaceAggregation = interfaceAggregationService.GetByInterfaceAggregationCode(queryArgs._interface);
                if (interfaceAggregation == null)
                    return JsonResultModel.Error($"未能找到接口编码为[{queryArgs._interface}]对应的接口信息");

                //查询条件
                FilterDefinition<BsonDocument> filter;
                //应用编码
                string applicationCode = interfaceAggregation.Code.Split('.')[0];

                switch ((InterfaceType)interfaceAggregation.InterfaceType)
                {
                    case InterfaceType.SingleObject:
                        filter = conditionAggregationService.AnalysisConditionToFilterDefinitionByConditionId(interfaceAggregation.MetaObjectId, interfaceAggregation.SearchConditionId, argumentsDic);
                        filter = _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_SingleObject, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_SingleObject_Before, ref filter, interfaceAggregation.Code, filter);
                        var singleObjectComponent = dataAccessService.GetSingleObjectComponent(interfaceAggregation.MetaObjectId, interfaceAggregation.FieldListId, filter);
                        singleObjectComponent = _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_SingleObject, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_SingleObject_After, ref singleObjectComponent, interfaceAggregation.Code, singleObjectComponent);
                        return JsonResultModel.Success("get single data success", singleObjectComponent);
                    case InterfaceType.TableList:
                        filter = conditionAggregationService.AnalysisConditionToFilterDefinitionByConditionId(interfaceAggregation.MetaObjectId, interfaceAggregation.SearchConditionId, argumentsDic);
                        filter = _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_TableList, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_TableList_Before, ref filter, interfaceAggregation.Code, filter);
                        var sort = metaFieldService.GetSortDefinitionBySortFields(interfaceAggregation.MetaObjectId, null);
                        var tableListComponent = dataAccessService.GetTableListComponent(interfaceAggregation.MetaObjectId, interfaceAggregation.FieldListId, filter, queryArgs._pageIndex, queryArgs._pageSize, sort, out int totalCount);
                        tableListComponent = _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_TableList, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_TableList_After, ref tableListComponent, interfaceAggregation.Code, tableListComponent);
                        return JsonResultModel.Success("get data list success", tableListComponent);
                    case InterfaceType.Count:
                        filter = conditionAggregationService.AnalysisConditionToFilterDefinitionByConditionId(interfaceAggregation.MetaObjectId, interfaceAggregation.SearchConditionId, argumentsDic);
                        filter = _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_Count, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_Count_Before, ref filter, interfaceAggregation.Code, filter);
                        var count = dataAccessService.GetCount(interfaceAggregation.MetaObjectId, filter);
                        count = _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_Count, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_Count_After, ref count, interfaceAggregation.Code, filter, count);
                        return JsonResultModel.Success("get data count success", count);
                    case InterfaceType.JsonDataSource:
                        return new JsonResult(Newtonsoft.Json.JsonConvert.DeserializeObject(_dataSourceService.GetById(interfaceAggregation.DataSourceId).Script));
                    case InterfaceType.TriggerScriptDataSource:
                        object triggerScriptDataSourceResult = _triggerScriptService.RunDataSourceScript(applicationCode, interfaceAggregation.DataSourceId, interfaceAggregation.Code, argumentsDic);
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
                //args check
                if (queryArgs == null)
                    return JsonResultModel.Error($"Parameter invalid:queryArgs = null");

                var checkResult = queryArgs.QueryArgsCheck();
                if (!checkResult.IsSuccess)
                    return checkResult.ToJsonResultModel();

                var json = jObj.ToString();
                var bson = BsonDocument.Parse(json);
                if (bson == null || !bson.Any())
                    return JsonResultModel.Error("Parameter invalid:jObj = null 业务数据为空");

                //get interface info
                var interfaceAggregation = interfaceAggregationService.GetByInterfaceAggregationCode(queryArgs._interface);
                if (interfaceAggregation == null)
                    return JsonResultModel.Error($"未能找到接口编码为[{queryArgs._interface}]对应的接口信息");

                //应用编码
                string applicationCode = interfaceAggregation.Code.Split('.')[0];

                //get metaObject
                var metaObject = metaObjectService.GetById(interfaceAggregation.MetaObjectId);
                if (metaObject == null)
                    return JsonResultModel.Error($"未能找到对象Id为[{interfaceAggregation.MetaObjectId}]对应的对象信息");

                //trigger before
                bson = _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_Add, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_Add_Before, ref bson, interfaceAggregation.Code, bson);

                //check data by form
                if (interfaceAggregation.FormId != default(int))
                {
                    var formCheckResult = _formMetaFieldService.ValidateFormData(interfaceAggregation.FormId, bson);
                    if (!formCheckResult.IsSuccess)
                        return formCheckResult.ToJsonResultModel();
                }

                //add data
                var addResult = dataAccessService.Add(metaObject, bson);

                //trigger after
                _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_Add, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_Add_After, ref bson, interfaceAggregation.Code, bson);

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
                //args check
                if (queryArgs == null)
                    return JsonResultModel.Error($"Parameter invalid:queryArgs = null");

                var checkResult = queryArgs.QueryArgsCheck();
                if (!checkResult.IsSuccess)
                    return checkResult.ToJsonResultModel();

                var json = jObj.ToString();
                var bson = BsonDocument.Parse(json);
                if (bson == null || !bson.Any())
                    return JsonResultModel.Error("Parameter invalid:jObj = null 业务数据为空");

                //argumentsDic generate
                Dictionary<string, object> argumentsDic = new Dictionary<string, object>();
                foreach (var item in Request.Query)
                    argumentsDic.AddOrUpdate(item.Key.ToUpperInvariant(), item.Value);

                //get interface info
                var interfaceAggregation = interfaceAggregationService.GetByInterfaceAggregationCode(queryArgs._interface);
                if (interfaceAggregation == null)
                    return JsonResultModel.Error($"未能找到接口编码为[{queryArgs._interface}]对应的接口信息");

                //查询条件
                FilterDefinition<BsonDocument> filter;
                //应用编码
                string applicationCode = interfaceAggregation.Code.Split('.')[0];

                filter = conditionAggregationService.AnalysisConditionToFilterDefinitionByConditionId(interfaceAggregation.MetaObjectId, interfaceAggregation.SearchConditionId, argumentsDic);

                //trigger before
                bson = _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_Update, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_Update_Before, ref bson, interfaceAggregation.Code, bson, filter);

                //check data by form
                if (interfaceAggregation.FormId != default(int))
                {
                    var formCheckResult = _formMetaFieldService.ValidateFormData(interfaceAggregation.FormId, bson);
                    if (!formCheckResult.IsSuccess)
                        return formCheckResult.ToJsonResultModel();
                }

                //update data
                dataAccessService.Update(interfaceAggregation.MetaObjectId, filter, bson);

                //trigger after
                _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_Update, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_Update_After, ref bson, interfaceAggregation.Code, bson);

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
                //args check
                if (queryArgs == null)
                    return JsonResultModel.Error($"Parameter invalid:queryArgs = null");

                var checkResult = queryArgs.QueryArgsCheck();
                if (!checkResult.IsSuccess)
                    return checkResult.ToJsonResultModel();

                //argumentsDic generate
                Dictionary<string, object> argumentsDic = new Dictionary<string, object>();
                foreach (var item in Request.Query)
                    argumentsDic.AddOrUpdate(item.Key.ToUpperInvariant(), item.Value);

                //get interface info
                var interfaceAggregation = interfaceAggregationService.GetByInterfaceAggregationCode(queryArgs._interface);
                if (interfaceAggregation == null)
                    return JsonResultModel.Error($"未能找到接口编码为[{queryArgs._interface}]对应的接口信息");

                //查询条件
                FilterDefinition<BsonDocument> filter;
                //应用编码
                string applicationCode = interfaceAggregation.Code.Split('.')[0];

                filter = conditionAggregationService.AnalysisConditionToFilterDefinitionByConditionId(interfaceAggregation.MetaObjectId, interfaceAggregation.SearchConditionId, argumentsDic);

                //trigger before
                filter = _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_Delete, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_Delete_Before, ref filter, interfaceAggregation.Code, filter);

                //queryResult
                var sort = metaFieldService.GetSortDefinitionBySortFields(interfaceAggregation.MetaObjectId, null);
                var queryDatas = dataAccessService.GetList(interfaceAggregation.MetaObjectId, filter, sort);

                //delete
                dataAccessService.Delete(interfaceAggregation.MetaObjectId, filter);

                //trigger after
                _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_Delete, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_Delete_After, ref filter, interfaceAggregation.Code, queryDatas);

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