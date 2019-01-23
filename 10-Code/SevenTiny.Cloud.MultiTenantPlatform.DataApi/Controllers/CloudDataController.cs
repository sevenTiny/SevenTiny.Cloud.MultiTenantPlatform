using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using MongoDB.Bson.Serialization;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Checker;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    //[Produces("application/json")]
    [Route("api/CloudData")]
    [ApiController]
    public class CloudDataController : ControllerBase
    {
        public CloudDataController(
            IDataAccessService _dataAccessService,
            IConditionAggregationService _conditionAggregationService,
            IInterfaceAggregationService _interfaceAggregationService,
            IMetaObjectService _metaObjectService,
            IFieldBizDataService _fieldBizDataService
            )
        {
            dataAccessService = _dataAccessService;
            conditionAggregationService = _conditionAggregationService;
            interfaceAggregationService = _interfaceAggregationService;
            metaObjectService = _metaObjectService;
            fieldBizDataService = _fieldBizDataService;
        }

        readonly IDataAccessService dataAccessService;
        readonly IInterfaceAggregationService interfaceAggregationService;
        readonly IConditionAggregationService conditionAggregationService;
        readonly IMetaObjectService metaObjectService;
        readonly IFieldBizDataService fieldBizDataService;

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
                        argumentsDic.Add(item.Key, item.Value);
                    }
                }

                //get metaObjectInfo
                MetaObject metaObject = metaObjectService.GetByCode(queryArgs.metaObjectCode);
                if (metaObject == null)
                {
                    return JsonResultModel.Error($"未能找到对象编码[{queryArgs.metaObjectCode}]对应的对象信息");
                }

                //get filter
                var interfaceAggregation = interfaceAggregationService.GetByMetaObjectIdAndInterfaceAggregationCode(metaObject.Id, queryArgs.interfaceCode);
                var filter = conditionAggregationService.AnalysisConditionToFilterDefinition(interfaceAggregation.InterfaceSearchConditionId, argumentsDic);

                switch (EnumsTranslaterUseInProgram.ToInterfaceType(interfaceAggregation.InterfaceType))
                {
                    case InterfaceType.CloudSingleObject:
                        var document = dataAccessService.GetBsonDocumentByCondition(filter);
                        return JsonResultModel.Success("Get Single Data Success", document);
                    case InterfaceType.CloudTableList:
                        var documents = dataAccessService.GetBsonDocumentsByCondition(filter, queryArgs.pageIndex, queryArgs.pageSize);
                        //转成前端易处理的Table组件
                        TableListComponent tableListComponent = new TableListComponent();
                        tableListComponent.BizData = fieldBizDataService.ConvertToDictionaryList(interfaceAggregation.InterfaceFieldId, documents);
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
                if (!addResult.IsSuccess)
                    return addResult.ToJsonResultModel();

                return JsonResultModel.Success("add success");
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