using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Checker;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    [Produces("application/json")]
    [Route("api/CloudData")]
    public class CloudDataController : Controller
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

        public IActionResult Post(int tenantId, BsonDocument bsons)
        {
            if (tenantId <= 0)
                throw new ArgumentException($"Parameter invalid: tenantId={tenantId}");

            ArgumentsChecker.CheckNull("bsons", bsons);

            dataAccessService.Add(tenantId, bsons);
            return JsonResultModel.Success("add success");
        }

        public IActionResult Update()
        {
            return null;
        }

        public IActionResult Delete()
        {
            return null;
        }
    }
}