using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.Service;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    [EnableCors("AllowSameDomain")]
    [Route("api/BatchCloudData")]
    [ApiController]
    public class BatchCloudDataController : ApiControllerBase
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

        [HttpPost]
        public IActionResult Post([FromQuery]QueryArgs queryArgs, [FromBody]JArray jArray)
        {
            try
            {
                if (jArray == null || !jArray.Any())
                    return JsonResultModel.Error($"Parameter invalid:data = null");

                //Pretreatment create queryContext
                QueryPiplineContext queryContext = PretreatmentAndCreateQueryPiplineContext(queryArgs);
                queryContext.MetaObject = metaObjectService.GetById(queryContext.MetaObjectId);

                if (queryContext.MetaObject == null)
                    return JsonResultModel.Error($"未能找到对象Id为[{queryContext.MetaObjectId}]对应的对象信息");

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

                //缓存某个服务下的全部触发器脚本，包括before和after
                queryContext.TriggerScriptsOfOneServiceType = _triggerScriptService.GetTriggerScriptsUnDeletedByMetaObjectIdAndServiceType(queryContext.MetaObjectId, (int)ServiceType.Interface_BatchAdd);

                //trigger before
                documents = _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.Before, TriggerScriptService.FunctionName_MetaObject_Interface_BatchAdd_Before, documents, CurrentApplicationContext, queryContext.InterfaceCode, documents);

                //check data by form
                if (queryContext.FormId != default(int))
                {
                    var formCheckResult = _formMetaFieldService.ValidateFormData(queryContext.FormId, documents);
                    if (!formCheckResult.IsSuccess)
                        return formCheckResult.ToJsonResultModel();
                }

                //add data
                var addResult = dataAccessService.BatchAdd(queryContext.TenantId, queryContext.MetaObject, documents);

                //trigger after
                _triggerScriptService.RunTriggerScript(queryContext, TriggerPoint.After, TriggerScriptService.FunctionName_MetaObject_Interface_BatchAdd_After, documents, CurrentApplicationContext, queryContext.InterfaceCode, documents);

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