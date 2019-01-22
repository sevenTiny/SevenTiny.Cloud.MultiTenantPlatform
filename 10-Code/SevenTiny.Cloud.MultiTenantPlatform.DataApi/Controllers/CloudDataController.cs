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
            IInterfaceAggregationService _interfaceAggregationService
            )
        {
            dataAccessService = _dataAccessService;
            conditionAggregationService = _conditionAggregationService;
            interfaceAggregationService = _interfaceAggregationService;
        }

        readonly IDataAccessService dataAccessService;
        readonly IInterfaceAggregationService interfaceAggregationService;
        readonly IConditionAggregationService conditionAggregationService;

        [HttpGet]
        public IActionResult Get(int tenantId, QueryArgs queryArgs)
        {
            try
            {
                //1.args check
                if (queryArgs == null)
                {
                    throw new ArgumentNullException("queryArgs can not be null!");
                }
                queryArgs.QueryArgsCheck();

                //2.argumentsDic generate
                Dictionary<string, object> argumentsDic = new Dictionary<string, object>();
                foreach (var item in Request.Form)
                {
                    if (!argumentsDic.ContainsKey(item.Key))
                    {
                        argumentsDic.Add(item.Key, item.Value);
                    }
                }
                foreach (var item in Request.Query)
                {
                    if (!argumentsDic.ContainsKey(item.Key))
                    {
                        argumentsDic.Add(item.Key, item.Value);
                    }
                }

                //3.get filter
                var interfaceAggregation = interfaceAggregationService.GetByMetaObjectCodeAndInterfaceAggregationCode(queryArgs.metaObjectCode, queryArgs.interfaceCode);
                var filter = conditionAggregationService.AnalysisConditionToFilterDefinition(interfaceAggregation.InterfaceSearchConditionId, argumentsDic);

                //4.query result
                object data = dataAccessService.GetObjectDatasByCondition(tenantId, filter, queryArgs.pageIndex, queryArgs.pageSize);

                return JsonResultModel.Success("get data success", data);
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

                dataAccessService.Add(0, bson);

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