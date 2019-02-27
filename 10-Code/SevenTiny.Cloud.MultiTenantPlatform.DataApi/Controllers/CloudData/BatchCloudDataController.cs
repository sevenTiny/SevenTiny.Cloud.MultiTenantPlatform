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
    [Route("api/BatchCloudData")]
    [ApiController]
    public class BatchCloudDataController : ControllerBase
    {
        public BatchCloudDataController(
            IDataAccessService _dataAccessService,
            ISearchConditionService _searchConditionService,
            ISearchConditionAggregationService _conditionAggregationService,
            IInterfaceAggregationService _interfaceAggregationService,
            IFieldBizDataService _fieldBizDataService,
            ITriggerScriptEngineService _triggerScriptEngineService,
            IMetaObjectService _metaObjectService
            )
        {
            dataAccessService = _dataAccessService;
            conditionAggregationService = _conditionAggregationService;
            interfaceAggregationService = _interfaceAggregationService;
            fieldBizDataService = _fieldBizDataService;
            triggerScriptEngineService = _triggerScriptEngineService;
            searchConditionService = _searchConditionService;
            metaObjectService = _metaObjectService;
        }

        readonly IDataAccessService dataAccessService;
        readonly IInterfaceAggregationService interfaceAggregationService;
        readonly ISearchConditionAggregationService conditionAggregationService;
        readonly IFieldBizDataService fieldBizDataService;
        readonly ITriggerScriptEngineService triggerScriptEngineService;
        readonly ISearchConditionService searchConditionService;
        readonly IMetaObjectService metaObjectService;

        [HttpPost]
        public IActionResult Post(string metaObjectCode, [FromBody]JArray jArray)
        {
            try
            {
                if (string.IsNullOrEmpty(metaObjectCode))
                    return JsonResultModel.Error($"Parameter invalid:metaObjectCode = null");
                if (jArray == null)
                    return JsonResultModel.Error($"Parameter invalid:data = null");

                if (jArray.Any())
                {
                    //get metaObject
                    var metaObject = metaObjectService.GetByCode(metaObjectCode);
                    if (metaObject == null)
                    {
                        return JsonResultModel.Error($"未能找到对象编码为[{metaObjectCode}]对应的对象信息");
                    }

                    int successCount = 0;
                    int errorCount = 0;

                    List<BsonDocument> documents = new List<BsonDocument>();
                    foreach (var item in jArray)
                    {
                        try
                        {
                            var json = item.ToString();
                            documents.Add(BsonDocument.Parse(json));
                            successCount++;
                        }
                        catch (Exception)
                        {
                            errorCount++;
                        }
                    }

                    //trigger before
                    documents = triggerScriptEngineService.BatchAddBefore(metaObject.Id, metaObjectCode, documents);

                    //add data
                    var addResult = dataAccessService.BatchAdd(metaObject, documents);

                    if (addResult.IsSuccess)
                    {
                        return JsonResultModel.Error($"插入成功!日志信息:成功[{successCount}]条，失败[{errorCount}]条");
                    }
                    else
                    {
                        return JsonResultModel.Error("插入失败");
                    }
                }
                return JsonResultModel.Error("插入失败");
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.ToString());
            }
        }
    }
}