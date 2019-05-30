using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using Seventiny.Cloud.DataApi.Models;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Service;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seventiny.Cloud.DataApi.Controllers
{
    [EnableCors("AllowSameDomain")]
    [Route("api/BatchCloudData")]
    [ApiController]
    public class BatchCloudDataController : ControllerBase
    {
        public BatchCloudDataController(
            IDataAccessService _dataAccessService,
            ISearchConditionService _searchConditionService,
            ISearchConditionNodeService _conditionAggregationService,
            IInterfaceAggregationService _interfaceAggregationService,
            IFieldBizDataService _fieldBizDataService,
            ITriggerScriptService triggerScriptService,
            IMetaObjectService _metaObjectService,
            IFormMetaFieldService formMetaFieldService
            )
        {
            dataAccessService = _dataAccessService;
            conditionAggregationService = _conditionAggregationService;
            interfaceAggregationService = _interfaceAggregationService;
            fieldBizDataService = _fieldBizDataService;
            _triggerScriptService = triggerScriptService;
            searchConditionService = _searchConditionService;
            metaObjectService = _metaObjectService;
            _formMetaFieldService = formMetaFieldService;
        }

        readonly IDataAccessService dataAccessService;
        readonly IInterfaceAggregationService interfaceAggregationService;
        readonly ISearchConditionNodeService conditionAggregationService;
        readonly IFieldBizDataService fieldBizDataService;
        readonly ISearchConditionService searchConditionService;
        readonly IMetaObjectService metaObjectService;
        readonly ITriggerScriptService _triggerScriptService;
        readonly IFormMetaFieldService _formMetaFieldService;

        [HttpPost]
        public IActionResult Post([FromQuery]QueryArgs queryArgs, [FromBody]JArray jArray)
        {
            try
            {
                //args check
                if (queryArgs == null)
                    return JsonResultModel.Error($"Parameter invalid:queryArgs = null");

                var checkResult = queryArgs.QueryArgsCheck();
                if (!checkResult.IsSuccess)
                    return checkResult.ToJsonResultModel();

                if (jArray == null || !jArray.Any())
                    return JsonResultModel.Error($"Parameter invalid:data = null");

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
                documents = _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_BatchAdd, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_BatchAdd_Before, ref documents, interfaceAggregation.Code, documents);

                //check data by form
                if (interfaceAggregation.FormId != default(int))
                {
                    var formCheckResult = _formMetaFieldService.ValidateFormData(interfaceAggregation.FormId, documents);
                    if (!formCheckResult.IsSuccess)
                        return formCheckResult.ToJsonResultModel();
                }

                //add data
                var addResult = dataAccessService.BatchAdd(metaObject, documents);

                //trigger after
                _triggerScriptService.RunTriggerScript(interfaceAggregation.MetaObjectId, applicationCode, ServiceType.Interface_BatchAdd, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_BatchAdd_After, ref documents, interfaceAggregation.Code, documents);

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
    }
}