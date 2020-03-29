using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Configs;
using SevenTiny.Cloud.MultiTenant.Web.Models;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class CloudInterfaceController : WebControllerBase
    {
        ISearchConditionService _searchConditionService;
        ISearchConditionNodeService _searchConditionNodeService;
        IDataSourceService _dataSourceService;
        ICloudInterfaceService _cloudInterfaceService;

        public CloudInterfaceController(ICloudInterfaceService cloudInterfaceService, ISearchConditionService _searchConditionService, ISearchConditionNodeService searchConditionNodeService, IDataSourceService dataSourceService)
        {
            this._searchConditionService = _searchConditionService;
            _searchConditionNodeService = searchConditionNodeService;
            _dataSourceService = dataSourceService;
            _cloudInterfaceService = cloudInterfaceService;
        }

        public IActionResult List()
        {
            return View(_cloudInterfaceService.GetListUnDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        //private void SetDataSource()
        //{
        //    ViewData["InterfaceFields"] = interfaceFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
        //    ViewData["SearchConditions"] = _searchConditionService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
        //    ViewData["Forms"] = _formService.GetEntitiesByMetaObjectId(CurrentMetaObjectId);
        //    ViewData["ScriptDataSources"] = _dataSourceService.GetListByAppIdAndDataSourceType(CurrentApplicationId, DataSourceType.Script);
        //    ViewData["JsonDataSources"] = _dataSourceService.GetListByAppIdAndDataSourceType(CurrentApplicationId, DataSourceType.Json);
        //}

        //public IActionResult Add()
        //{
        //    SetDataSource();
        //    return View(ResponseModel.Success(new CloudInterface { }));
        //}

        //public IActionResult AddLogic(CloudInterface entity)
        //{
        //    SetDataSource();

        //    if (string.IsNullOrEmpty(entity.Name))
        //    {
        //        return View("Add", ResponseModel.Error("名称不能为空", entity));
        //    }
        //    if (string.IsNullOrEmpty(entity.Code))
        //    {
        //        return View("Add", ResponseModel.Error("编码不能为空", entity));
        //    }

        //    //校验code格式
        //    if (!entity.Code.IsAlnum(2, 50))
        //    {
        //        return View("Add", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", entity));
        //    }

        //    string tempCode = entity.Code;
        //    entity.MetaObjectId = CurrentMetaObjectId;
        //    entity.Code = $"{CurrentMetaObjectCode}.Interface.{entity.Code}";
        //    //检查编码或名称重复
        //    var checkResult = interfaceAggregationService.CheckSameCodeOrName(CurrentMetaObjectId, entity);
        //    if (!checkResult.IsSuccess)
        //    {
        //        entity.Code = tempCode;
        //        return View("Add", checkResult.ToResponseModel());
        //    }

        //    entity.CreateBy = CurrentUserId;
        //    interfaceAggregationService.Add(entity);

        //    return RedirectToAction("List");
        //}

        //public IActionResult Update(int id)
        //{
        //    SetDataSource();

        //    var metaObject = interfaceAggregationService.GetById(id);
        //    return View(ResponseModel.Success(metaObject));
        //}

        //public IActionResult UpdateLogic(CloudInterface entity)
        //{
        //    SetDataSource();

        //    if (entity.Id == 0)
        //    {
        //        return View("Update", ResponseModel.Error("修改的id传递错误", entity));
        //    }
        //    if (string.IsNullOrEmpty(entity.Name))
        //    {
        //        return View("Update", ResponseModel.Error("名称不能为空", entity));
        //    }
        //    if (string.IsNullOrEmpty(entity.Code))
        //    {
        //        return View("Update", ResponseModel.Error("编码不能为空", entity));
        //    }

        //    //检查编码或名称重复
        //    var checkResult = interfaceAggregationService.CheckSameCodeOrName(CurrentMetaObjectId, entity);
        //    if (!checkResult.IsSuccess)
        //    {
        //        return View("Update", checkResult.ToResponseModel());
        //    }

        //    entity.ModifyBy = CurrentUserId;
        //    interfaceAggregationService.Update(entity);

        //    return RedirectToAction("List");
        //}

        //public IActionResult Delete(int id)
        //{
        //    interfaceAggregationService.Delete(id);
        //    return JsonResultModel.Success("删除成功");
        //}

        //public IActionResult LogicDelete(int id)
        //{
        //    interfaceAggregationService.LogicDelete(id);
        //    return JsonResultModel.Success("删除成功");
        //}

        //public IActionResult Recover(int id)
        //{
        //    interfaceAggregationService.Recover(id);
        //    return JsonResultModel.Success("恢复成功");
        //}

        //public IActionResult Description(int id)
        //{
        //    var interfaceInfo = interfaceAggregationService.GetById(id);

        //    if (interfaceInfo.SearchConditionId != default(int))
        //        ViewData["ParametersConditionItems"] = _searchConditionNodeService.GetParametersConditionItemsBySearchConditionId(interfaceInfo.SearchConditionId);

        //    ViewData["DataApiUrl"] = UrlsConfig.Instance.DataApiUrl;
        //    return View(interfaceInfo);
        //}
    }
}