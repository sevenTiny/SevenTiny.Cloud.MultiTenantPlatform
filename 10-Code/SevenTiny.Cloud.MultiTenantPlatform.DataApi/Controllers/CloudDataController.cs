using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using System;
using System.Collections.Generic;

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
            ITriggerScriptService _triggerScriptService
            )
        {
            dataAccessService = _dataAccessService;
            conditionAggregationService = _conditionAggregationService;
            interfaceAggregationService = _interfaceAggregationService;
            fieldBizDataService = _fieldBizDataService;
            triggerScriptService = _triggerScriptService;
        }

        readonly IDataAccessService dataAccessService;
        readonly IInterfaceAggregationService interfaceAggregationService;
        readonly ISearchConditionAggregationService conditionAggregationService;
        readonly IFieldBizDataService fieldBizDataService;
        readonly ITriggerScriptService triggerScriptService;

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
                TableListComponent tableListComponent;
                int totalCount;

                switch (EnumsTranslaterUseInProgram.ToInterfaceType(interfaceAggregation.InterfaceType))
                {
                    case InterfaceType.CloudSingleObject:
                        var document = dataAccessService.GetBsonDocumentsByCondition(filter, 1, 1, out totalCount);
                        tableListComponent = new TableListComponent
                        {
                            BizData = fieldBizDataService.ConvertToDictionaryList(interfaceAggregation.FieldListId, document),
                            BizDataTotalCount = totalCount
                        };
                        return JsonResultModel.Success("Get Single Data Success", tableListComponent);
                    case InterfaceType.CloudTableList:
                        var documents = dataAccessService.GetBsonDocumentsByCondition(filter, queryArgs.pageIndex, queryArgs.pageSize, out totalCount);
                        tableListComponent = new TableListComponent
                        {
                            BizData = fieldBizDataService.ConvertToDictionaryList(interfaceAggregation.FieldListId, documents),
                            BizDataTotalCount = totalCount
                        };
                        tableListComponent = triggerScriptService.TableListAfter(tableListComponent, 1);
                        return JsonResultModel.Success("Get Data List Success", tableListComponent);
                    case InterfaceType.CloudCount:
                        var count = dataAccessService.GetBsonDocumentCountByCondition(filter);
                        return JsonResultModel.Success("Get Data Count Success", count);
                    case InterfaceType.EnumeDataSource:
                        break;
                    default:
                        break;
                }

                return JsonResultModel.Success("Success,No Data");
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
                return JsonResultModel.Error(JsonConvert.SerializeObject(ex));
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