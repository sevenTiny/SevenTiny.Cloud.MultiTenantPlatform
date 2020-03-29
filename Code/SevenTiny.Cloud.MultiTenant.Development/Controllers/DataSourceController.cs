using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Bantina;
using System;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class DataSourceController : WebControllerBase
    {
        IDataSourceService _dataSourceService;
        ITriggerScriptService _triggerScriptService;

        public DataSourceController(IDataSourceService dataSourceService, ITriggerScriptService triggerScriptService)
        {
            _triggerScriptService = triggerScriptService;
            _dataSourceService = dataSourceService;
        }

        private Result CommonAddCheck(string dataSourceType, DataSource entity)
        {
            return Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Script, nameof(entity.Script))
                ;
        }

        #region ScriptDataSource
        public IActionResult AddScriptDataSource()
        {
            var defaultScript = _triggerScriptService.GetDefaultDataSourceTriggerScript();
            return View(ResponseModel.Success(new DataSource { Script = defaultScript }));
        }
        public IActionResult AddScriptDataSourceLogic(DataSource entity)
        {
            var result = CommonAddCheck("ScriptDataSource", entity)
                 .Continue(re => _triggerScriptService.CompilateAndCheckScript(entity.Script, CurrentApplicationCode))
                 .Continue(re =>
                 {
                     entity.Code = $"{CurrentApplicationCode}.ScriptDataSource.{entity.Code}";
                     entity.ApplicationId = CurrentApplicationId;
                     entity.DataSourceType = (int)DataSourceType.Script;
                     return re;
                 })
                 .Continue(re =>
                 {
                     entity.CreateBy = CurrentUserId;
                     return _dataSourceService.Add(entity);
                 });

            if (result.IsSuccess)
                return RedirectToAction("ScriptDataSourceList");
            else
                return View("AddScriptDataSource", ResponseModel.Error(result.Message, entity));
        }

        public IActionResult UpdateScriptDataSource(Guid id)
        {
            return View(ResponseModel.Success(_dataSourceService.GetById(id)));
        }
        public IActionResult UpdateScriptDataSourceLogic(DataSource entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Script, nameof(entity.Script))
                .Continue(re => _triggerScriptService.CompilateAndCheckScript(entity.Script, CurrentApplicationCode))
                .Continue(re =>
                {
                    entity.ModifyBy = CurrentUserId;
                    return _dataSourceService.Update(entity);
                });

            if (result.IsSuccess)
                return RedirectToAction("ScriptDataSourceList");
            else
                return View("UpdateScriptDataSource", ResponseModel.Error(result.Message, entity));
        }

        public IActionResult ScriptDataSourceList()
        {
            return View(_dataSourceService.GetListByApplicationIdAndDataSourceType(CurrentApplicationId, DataSourceType.Script));
        }
        #endregion

        #region JsonDataSource
        public IActionResult AddJsonDataSource()
        {
            var defaultScript =
    @"{
    ""Key"":7tiny
}";
            return View(ResponseModel.Success(new DataSource { Script = defaultScript }));
        }
        public IActionResult AddJsonDataSourceLogic(DataSource entity)
        {
            var result = CommonAddCheck("JsonDataSource", entity)
                .Continue(re =>
                {
                    entity.ApplicationId = CurrentApplicationId;
                    try
                    {
                        Newtonsoft.Json.JsonConvert.DeserializeObject(entity.Script);
                    }
                    catch (System.Exception)
                    {
                        return Result.Error("非法的Json格式.");
                    }
                    return re;
                })
                .Continue(re =>
                {
                    entity.Code = $"{CurrentApplicationCode}.JsonDataSource.{entity.Code}";
                    entity.DataSourceType = (int)DataSourceType.Json;
                    return re;
                })
                 .Continue(re =>
                 {
                     entity.CreateBy = CurrentUserId;
                     return _dataSourceService.Add(entity);
                 });

            if (result.IsSuccess)
                return RedirectToAction("JsonDataSourceList");
            else
                return View("AddJsonDataSource", ResponseModel.Error(result.Message, entity));
        }

        public IActionResult UpdateJsonDataSource(Guid id)
        {
            return View(ResponseModel.Success(_dataSourceService.GetById(id)));
        }
        public IActionResult UpdateJsonDataSourceLogic(DataSource entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Script, nameof(entity.Script))
                .Continue(re =>
                {
                    try
                    {
                        Newtonsoft.Json.JsonConvert.DeserializeObject(entity.Script);
                    }
                    catch (System.Exception)
                    {
                        return Result.Error("非法的Json格式.");
                    }
                    return re;
                })
                .Continue(re =>
                {
                    entity.ModifyBy = CurrentUserId;
                    return _dataSourceService.Update(entity);
                });

            if (result.IsSuccess)
                return RedirectToAction("JsonDataSourceList");
            else
                return View("UpdateJsonDataSource", ResponseModel.Error(result.Message, entity));
        }

        public IActionResult JsonDataSourceList()
        {
            return View(_dataSourceService.GetListByApplicationIdAndDataSourceType(CurrentApplicationId, DataSourceType.Json));
        }
        #endregion

        public IActionResult LogicDelete(Guid id)
        {
            _dataSourceService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }
    }
}
