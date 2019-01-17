//using Microsoft.AspNetCore.Mvc;
//using MongoDB.Bson;
//using Newtonsoft.Json;
//using SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract;
//using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;
//using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Checker;
//using System;
//using System.Collections.Generic;

//namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
//{
//    [Produces("application/json")]
//    [Route("api/CloudData")]
//    public class CloudDataController : Controller
//    {
//        private readonly IAggregationConditionService _aggregationConditionService;
//        private readonly IInterfaceAggregationService _interfaceAggregationService;
//        private readonly IMultitenantDataService _multitenantDataService;
//        public CloudDataController(IAggregationConditionService aggregationConditionService, IInterfaceAggregationService interfaceAggregationService, IMultitenantDataService multitenantDataService)
//        {
//            _aggregationConditionService = aggregationConditionService;
//            _interfaceAggregationService = interfaceAggregationService;
//            _multitenantDataService = multitenantDataService;
//        }

//        public IActionResult Get(int tenantId, QueryArgs queryArgs)
//        {
//            try
//            {
//                //1.args check
//                if (queryArgs == null)
//                {
//                    throw new ArgumentNullException("queryArgs can not be null!");
//                }
//                queryArgs.QueryArgsCheck();

//                //2.argumentsDic generate
//                Dictionary<string, object> argumentsDic = new Dictionary<string, object>();
//                foreach (var item in Request.Form)
//                {
//                    if (!argumentsDic.ContainsKey(item.Key))
//                    {
//                        argumentsDic.Add(item.Key, item.Value);
//                    }
//                }
//                foreach (var item in Request.Query)
//                {
//                    if (!argumentsDic.ContainsKey(item.Key))
//                    {
//                        argumentsDic.Add(item.Key, item.Value);
//                    }
//                }

//                //3.get filter
//                int conditionId = _interfaceAggregationService.GetConditionAggregationByInterfaceAggregationCode(queryArgs.interfaceCode)?.Id ?? 0;
//                var filter = _aggregationConditionService.AnalysisConditionToFilterDefinition(conditionId, argumentsDic);

//                //4.query result
//                object data = _multitenantDataService.GetObjectDatasByCondition(tenantId, filter, queryArgs.pageIndex, queryArgs.pageSize);

//                return JsonResultModel.Success("get data success", data);
//            }
//            catch (ArgumentException argEx)
//            {
//                return JsonResultModel.Error(argEx.Message);
//            }
//            catch (Exception ex)
//            {
//                return JsonResultModel.Error(JsonConvert.SerializeObject(ex));
//            }
//        }

//        public IActionResult Post(int tenantId, BsonDocument bsons)
//        {
//            ArgumentsChecker.CheckTenantId(tenantId);
//            ArgumentsChecker.CheckNull("bsons", bsons);
//            _multitenantDataService.Add(tenantId, bsons);
//            return JsonResultModel.Success("add success");
//        }

//        public IActionResult Update()
//        {
//            return null;
//        }

//        public IActionResult Delete()
//        {
//            return null;
//        }
//    }
//}