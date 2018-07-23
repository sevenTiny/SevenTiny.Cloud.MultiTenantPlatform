using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.CloudModel;
using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    [Produces("application/json")]
    [Route("api/CloudData")]
    public class CloudDataController : Controller
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IMetaObjectRepository _metaObjectRepository;
        private readonly IObjectDataService _objectDataService;
        private readonly IAggregationConditionService _aggregationConditionService;
        private readonly IInterfaceAggregationRepository _interfaceAggregationRepository;
        public CloudDataController(IApplicationRepository applicationRepository, IMetaObjectRepository metaObjectRepository, IObjectDataService objectDataService, IAggregationConditionService aggregationConditionService, IInterfaceAggregationRepository interfaceAggregationRepository)
        {
            _applicationRepository = applicationRepository;
            _metaObjectRepository = metaObjectRepository;
            _objectDataService = objectDataService;
            _aggregationConditionService = aggregationConditionService;
            _interfaceAggregationRepository = interfaceAggregationRepository;
        }
        // GET api/values/5
        [HttpGet]
        [HttpGet("{tenantId:int}/{api:int}")]
        public string Get(int tenantId, int api)
        {
            return $"tenantId:{tenantId},api:{api}";
        }

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

                int applicationId = _applicationRepository.GetEntity(t => t.Code.Equals(queryArgs.ApplicationCode))?.Id ?? default(int);

                if (applicationId == default(int))
                {
                    throw new ArgumentNullException("application not found!");
                }

                int metaObjectId = _metaObjectRepository.GetEntity(t => t.ApplicationId == applicationId && t.Code.Equals(queryArgs.MetaObjectCode))?.Id ?? default(int);

                if (metaObjectId == default(int))
                {
                    throw new ArgumentNullException("metaobject not found!");
                }

                int conditionId = _interfaceAggregationRepository.GetEntity(t => t.MetaObjectId == metaObjectId && t.Code.Equals(queryArgs.InterfaceCode))?.InterfaceSearchConditionId ?? default(int);

                if (conditionId == default(int))
                {
                    throw new ArgumentNullException("condition not found!");
                }

                Dictionary<string, object> conditionValueDic = new Dictionary<string, object>();

                foreach (var item in Request.Form)
                {
                    if (!conditionValueDic.ContainsKey(item.Key))
                    {
                        conditionValueDic.Add(item.Key, item.Value);
                    }
                }

                foreach (var item in Request.Query)
                {
                    if (!conditionValueDic.ContainsKey(item.Key))
                    {
                        conditionValueDic.Add(item.Key, item.Value);
                    }
                }

                //get filter by condition id and query string.
                var filter = _aggregationConditionService.AnalysisConditionToFilterDefinition(conditionId, conditionValueDic);

                // todo:xxx query

                return JsonResultModel.Success("");
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

        public IActionResult Post(int tenantId, ObjectData objectData)
        {
            try
            {
                objectData.TenantId = tenantId;
                _objectDataService.Insert(objectData);
                return JsonResultModel.Success("add succeed");
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(JsonConvert.SerializeObject(ex));
            }
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