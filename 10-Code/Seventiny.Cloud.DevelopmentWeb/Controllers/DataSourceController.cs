using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Bantina;

namespace Seventiny.Cloud.DevelopmentWeb.Controllers
{
    public class DataSourceController : ControllerBase
    {
        IDataSourceService _dataSourceService;
        ITriggerScriptService _triggerScriptService;

        public DataSourceController(
            IDataSourceService dataSourceService,
        ITriggerScriptService triggerScriptService
            )
        {
            _dataSourceService = dataSourceService;
            _triggerScriptService = triggerScriptService;
        }

        private Result CommonAddCheck(string dataSourceType, DataSource entity)
        {
            return Result.Success()
                .ContinueAssert(!string.IsNullOrEmpty(entity.Name), "名称不能为空")
                .ContinueAssert(!string.IsNullOrEmpty(entity.Code), "编码不能为空")
                .ContinueAssert(entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .ContinueAssert(!string.IsNullOrEmpty(entity.Script), "数据源内容不能为空")
                .Continue(re =>
                {
                    string tempCode = entity.Code;
                    entity.ApplicationId = CurrentApplicationId;
                    entity.Code = $"{CurrentApplicationCode}.{dataSourceType}.{entity.Code}";

                    //检查编码或名称重复
                    var checkResult = _dataSourceService.CheckSameCodeOrName(entity);
                    if (!checkResult.IsSuccess)
                    {
                        return checkResult;
                    }
                    entity.Code = tempCode;
                    return re;
                });
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
                     entity.DataSourceType = (int)DataSourceType.Script;
                     return re;
                 })
                 .Continue(re => _dataSourceService.Add(entity));

            if (result.IsSuccess)
                return RedirectToAction("ScriptDataSourceList");
            else
                return View("AddScriptDataSource", ResponseModel.Error(result.Message, entity));
        }

        public IActionResult UpdateScriptDataSource(int id)
        {
            return View(ResponseModel.Success(_dataSourceService.GetById(id)));
        }
        public IActionResult UpdateScriptDataSourceLogic(DataSource entity)
        {
            var result = Result.Success()
                .ContinueAssert(!string.IsNullOrEmpty(entity.Name), "名称不能为空")
                .ContinueAssert(!string.IsNullOrEmpty(entity.Code), "编码不能为空")
                .ContinueAssert(!string.IsNullOrEmpty(entity.Script), "数据源内容不能为空")
                .Continue(re => _dataSourceService.CheckSameCodeOrName(entity))
                .Continue(re => _triggerScriptService.CompilateAndCheckScript(entity.Script, CurrentApplicationCode))
                .Continue(re => _dataSourceService.Update(entity));

            if (result.IsSuccess)
                return RedirectToAction("ScriptDataSourceList");
            else
                return View("UpdateScriptDataSource", ResponseModel.Error(result.Message, entity));
        }

        public IActionResult ScriptDataSourceList()
        {
            return View(_dataSourceService.GetListByAppIdAndDataSourceType(CurrentApplicationId, DataSourceType.Script));
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
                 .Continue(re => _dataSourceService.Add(entity));

            if (result.IsSuccess)
                return RedirectToAction("JsonDataSourceList");
            else
                return View("AddJsonDataSource", ResponseModel.Error(result.Message, entity));
        }

        public IActionResult UpdateJsonDataSource(int id)
        {
            return View(ResponseModel.Success(_dataSourceService.GetById(id)));
        }
        public IActionResult UpdateJsonDataSourceLogic(DataSource entity)
        {
            var result = Result.Success()
                .ContinueAssert(!string.IsNullOrEmpty(entity.Name), "名称不能为空")
                .ContinueAssert(!string.IsNullOrEmpty(entity.Code), "编码不能为空")
                .ContinueAssert(!string.IsNullOrEmpty(entity.Script), "数据源内容不能为空")
                .Continue(re => _dataSourceService.CheckSameCodeOrName(entity))
                .Continue(re => _dataSourceService.Update(entity));

            if (result.IsSuccess)
                return RedirectToAction("JsonDataSourceList");
            else
                return View("UpdateScriptDataSource", ResponseModel.Error(result.Message, entity));
        }

        public IActionResult JsonDataSourceList()
        {
            return View(_dataSourceService.GetListByAppIdAndDataSourceType(CurrentApplicationId, DataSourceType.Json));
        }
        #endregion

        public IActionResult LogicDelete(int id)
        {
            _dataSourceService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Delete(int id)
        {
            return _dataSourceService.Delete(id).ToJsonResultModel();
        }

        public IActionResult Recover(int id)
        {
            _dataSourceService.Recover(id);
            return JsonResultModel.Success("恢复成功");
        }
    }
}
