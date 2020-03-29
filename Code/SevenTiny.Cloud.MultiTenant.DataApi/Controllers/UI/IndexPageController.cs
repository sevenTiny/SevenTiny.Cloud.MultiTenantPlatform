using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    [EnableCors("AllowSameDomain")]
    [Route("api/UI/[controller]")]
    [ApiController]
    public class IndexPageController : ControllerBase
    {
        public IndexPageController(
            IIndexViewService indexViewService
            )
        {
            _indexViewService = indexViewService;
        }

        readonly IIndexViewService _indexViewService;


        [HttpGet]
        public IActionResult Get([FromQuery]UIIndexPageQueryArgs queryArgs)
        {
            try
            {
                //args check
                if (queryArgs == null)
                    return JsonResultModel.Error($"Parameter invalid:queryArgs = null");

                var checkResult = queryArgs.ArgsCheck();

                if (!checkResult.IsSuccess)
                    return checkResult.ToJsonResultModel();

                //argumentsDic generate
                Dictionary<string, object> argumentsDic = new Dictionary<string, object>();
                foreach (var item in Request.Query)
                    argumentsDic.AddOrUpdate(item.Key.ToUpperInvariant(), item.Value);

                //get filter
                var indexView = _indexViewService.GetByCode(queryArgs.ViewName);
                if (indexView == null)
                    return JsonResultModel.Error($"未能找到视图编码为[{queryArgs.ViewName}]对应的视图信息");

                var indexPageComponent = _indexViewService.GetIndexPageComponentByIndexView(indexView);

                //IndexView触发器
                //filter = triggerScriptEngineService.TableListBefore(indexView.MetaObjectId, indexView.Code, filter);
                //var sort = metaFieldService.GetSortDefinitionBySortFields(indexView.MetaObjectId, queryArgs.SortFields);
                //var tableListComponent = dataAccessService.GetTableListComponent(indexView.MetaObjectId, indexView.FieldListId, filter, queryArgs.PageIndex, queryArgs.PageSize, sort, out int totalCount);
                //tableListComponent = triggerScriptEngineService.TableListAfter(indexView.MetaObjectId, indexView.Code, tableListComponent);

                return JsonResultModel.Success("get index page component success", indexPageComponent);
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