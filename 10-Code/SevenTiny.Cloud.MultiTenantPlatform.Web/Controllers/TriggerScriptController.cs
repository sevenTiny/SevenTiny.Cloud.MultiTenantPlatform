using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class TriggerScriptController : ControllerBase
    {
        readonly ITriggerScriptService triggerScriptService;
        readonly ITriggerScriptEngineService triggerScriptEngineService;

        public TriggerScriptController(
            ITriggerScriptService _triggerScriptService,
            ITriggerScriptEngineService _triggerScriptEngineService
            )
        {
            triggerScriptService = _triggerScriptService;
            triggerScriptEngineService = _triggerScriptEngineService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(triggerScriptService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult DeleteList()
        {
            return View(triggerScriptService.GetEntitiesDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(TriggerScript triggerScript)
        {
            if (string.IsNullOrEmpty(triggerScript.Name))
            {
                return View("Add", ResponseModel.Error("名称不能为空", triggerScript));
            }
            if (string.IsNullOrEmpty(triggerScript.Code))
            {
                return View("Add", ResponseModel.Error("编码不能为空", triggerScript));
            }
            //校验code格式
            if (!triggerScript.Code.IsAlnum(2, 50))
            {
                return View("Add", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", triggerScript));
            }
            if (string.IsNullOrEmpty(triggerScript.Code))
            {
                return View("Add", ResponseModel.Error("脚本不能为空", triggerScript));
            }

            //检查编码或名称重复
            var checkResult = triggerScriptService.CheckSameCodeOrName(CurrentMetaObjectId, triggerScript);
            if (!checkResult.IsSuccess)
            {
                return View("Add", checkResult.ToResponseModel());
            }

            triggerScript.MetaObjectId = CurrentMetaObjectId;
            triggerScript.Code = $"{CurrentMetaObjectCode}.TriggerScript.{triggerScript.Code}";

            //check script
            var checkResult2 = triggerScriptEngineService.CompilationAndCheckScript(triggerScript.Script);
            if (!checkResult2.Item1)
            {
                return View("Add", ResponseModel.Error($"脚本存在错误：{checkResult2.Item2}", triggerScript));
            }

            triggerScriptService.Add(triggerScript);

            return RedirectToAction("List");
        }

        public IActionResult Update(int id)
        {
            var obj = triggerScriptService.GetById(id);
            return View(ResponseModel.Success(obj));
        }

        public IActionResult UpdateLogic(TriggerScript triggerScript)
        {
            try
            {
                if (triggerScript.Id == 0)
                {
                    return View("Update", ResponseModel.Error("MetaField Id 不能为空", triggerScript));
                }
                if (string.IsNullOrEmpty(triggerScript.Name))
                {
                    return View("Update", ResponseModel.Error("MetaField Name 不能为空", triggerScript));
                }
                if (string.IsNullOrEmpty(triggerScript.Code))
                {
                    return View("Update", ResponseModel.Error("MetaField Code 不能为空", triggerScript));
                }

                //检查编码或名称重复
                var checkResult = triggerScriptService.CheckSameCodeOrName(CurrentMetaObjectId, triggerScript);
                if (!checkResult.IsSuccess)
                {
                    return View("Update", checkResult.ToResponseModel());
                }

                //check script
                var checkResult2 = triggerScriptEngineService.CompilationAndCheckScript(triggerScript.Script);
                if (!checkResult2.Item1)
                {
                    return View("Update", ResponseModel.Error($"脚本存在错误：\r\n{checkResult2.Item2}", triggerScript));
                }

                //更新操作
                triggerScriptService.Update(triggerScript);

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                return View("Update", ResponseModel.Error(ex.ToString(), triggerScript));
            }
        }

        public IActionResult LogicDelete(int id)
        {
            triggerScriptService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Delete(int id)
        {
            return triggerScriptService.Delete(id).ToJsonResultModel();
        }

        public IActionResult Recover(int id)
        {
            triggerScriptService.Recover(id);
            return JsonResultModel.Success("恢复成功");
        }

        public IActionResult Detail()
        {
            return View();
        }

        public IActionResult GetDefaultTriggerScript(int scriptType)
        {
            string script = triggerScriptService.GetDefaultTriggerScriptByScriptType(scriptType);
            return JsonResultModel.Success("get default trigger script", script);
        }
    }
}