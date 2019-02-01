using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    //[Produces("application/json")]
    [Route("api/CloudData")]
    [ApiController]
    public class CloudDataController : ControllerBase
    {
        public CloudDataController(
            IDataAccessService _dataAccessService,
            ISearchConditionAggregationService _conditionAggregationService,
            IInterfaceAggregationService _interfaceAggregationService,
            IFieldBizDataService _fieldBizDataService,
            ITriggerScriptEngineService _triggerScriptEngineService
            )
        {
            dataAccessService = _dataAccessService;
            conditionAggregationService = _conditionAggregationService;
            interfaceAggregationService = _interfaceAggregationService;
            fieldBizDataService = _fieldBizDataService;
            triggerScriptEngineService = _triggerScriptEngineService;
        }

        readonly IDataAccessService dataAccessService;
        readonly IInterfaceAggregationService interfaceAggregationService;
        readonly ISearchConditionAggregationService conditionAggregationService;
        readonly IFieldBizDataService fieldBizDataService;
        readonly ITriggerScriptEngineService triggerScriptEngineService;

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
                var interfaceAggregation = interfaceAggregationService.GetByMetaObjectIdAndInterfaceAggregationCode(queryArgs.interfaceCode);
                if (interfaceAggregation == null)
                {
                    return JsonResultModel.Error($"未能找到接口编码为[{queryArgs.interfaceCode}]对应的接口信息");
                }
                var filter = conditionAggregationService.AnalysisConditionToFilterDefinition(interfaceAggregation.MetaObjectId, interfaceAggregation.SearchConditionId, argumentsDic);

                //get result
                switch ((InterfaceType)interfaceAggregation.InterfaceType)
                {
                    case InterfaceType.CloudSingleObject:
                        filter = triggerScriptEngineService.SingleObjectBefore(interfaceAggregation.MetaObjectId, interfaceAggregation.Code, filter);
                        var document = dataAccessService.GetBsonDocumentsByCondition(filter, 1, 1, out int singleCount)?.FirstOrDefault();
                        SingleObjectComponent singleObjectComponent = new SingleObjectComponent
                        {
                            BizData = fieldBizDataService.ConvertToDictionary(interfaceAggregation.FieldListId, document),
                        };
                        singleObjectComponent = triggerScriptEngineService.SingleObjectAfter(interfaceAggregation.MetaObjectId, interfaceAggregation.Code, singleObjectComponent);
                        return JsonResultModel.Success("get single data success", singleObjectComponent);
                    case InterfaceType.CloudTableList:
                        filter = triggerScriptEngineService.TableListBefore(interfaceAggregation.MetaObjectId, interfaceAggregation.Code, filter);
                        var documents = dataAccessService.GetBsonDocumentsByCondition(filter, queryArgs.pageIndex, queryArgs.pageSize, out int totalCount);
                        TableListComponent tableListComponent = new TableListComponent
                        {
                            BizData = fieldBizDataService.ConvertToDictionaryList(interfaceAggregation.FieldListId, documents),
                            BizDataTotalCount = totalCount
                        };
                        tableListComponent = triggerScriptEngineService.TableListAfter(interfaceAggregation.MetaObjectId, interfaceAggregation.Code, tableListComponent);
                        return JsonResultModel.Success("get data list success", tableListComponent);
                    case InterfaceType.CloudCount:
                        filter = triggerScriptEngineService.SingleObjectBefore(interfaceAggregation.MetaObjectId, interfaceAggregation.Code, filter);
                        var count = dataAccessService.GetBsonDocumentCountByCondition(filter);
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

                //add data
                var addResult = dataAccessService.Add(metaObjectCode, bson);

                return addResult.ToJsonResultModel();
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }

        [HttpPut]
        public IActionResult Update()
        {
            BsonDocument bson = new BsonDocument();
            bson.Add(new BsonElement("name", "zhangsan"));
            bson.Add(new BsonElement("age", "21"));
            return new JsonResult(bson);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return null;
        }
    }
}