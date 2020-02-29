using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class TriggerScriptController : WebControllerBase
    {
        readonly ITriggerScriptService triggerScriptService;

        public TriggerScriptController(
            ITriggerScriptService _triggerScriptService
            )
        {
            triggerScriptService = _triggerScriptService;
        }

        public IActionResult List()
        {
            return View(triggerScriptService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId));
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
            if (string.IsNullOrEmpty(triggerScript.Script))
            {
                return View("Add", ResponseModel.Error("脚本不能为空", triggerScript));
            }

            //检查编码或名称重复
            var checkResult = triggerScriptService.CheckSameCodeOrName(CurrentMetaObjectId, triggerScript);
            if (!checkResult.IsSuccess)
            {
                return View("Add", checkResult.ToResponseModel());
            }

            //check script
            var checkResult2 = triggerScriptService.CompilateAndCheckScript(triggerScript.Script, CurrentApplicationCode);
            if (!checkResult2.IsSuccess)
            {
                return View("Add", ResponseModel.Error($"脚本存在错误：{checkResult2.Message}", triggerScript));
            }

            triggerScript.MetaObjectId = CurrentMetaObjectId;
            triggerScript.Code = $"{CurrentMetaObjectCode}.TriggerScript.{triggerScript.Code}";
            triggerScript.CreateBy = CurrentUserId;
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
                    return View("Update", ResponseModel.Error("Id 不能为空", triggerScript));
                }
                if (string.IsNullOrEmpty(triggerScript.Name))
                {
                    return View("Update", ResponseModel.Error("Name 不能为空", triggerScript));
                }
                if (string.IsNullOrEmpty(triggerScript.Code))
                {
                    return View("Update", ResponseModel.Error("Code 不能为空", triggerScript));
                }

                //check script
                var checkResult2 = triggerScriptService.CompilateAndCheckScript(triggerScript.Script, CurrentApplicationCode);
                if (!checkResult2.IsSuccess)
                {
                    return View("Update", ResponseModel.Error($"脚本存在错误：\r\n{checkResult2.Message}", triggerScript));
                }

                triggerScript.ModifyBy = CurrentUserId;
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

        public IActionResult GetDefaultTriggerScript(int serviceType, int triggerPoint)
        {
            string script = string.Empty;
            if (triggerPoint == (int)TriggerPoint.Before)
                script = triggerScriptService.GetDefaultMetaObjectTriggerScriptByServiceTypeBefore(serviceType).TrimStart().TrimEnd();
            else if (triggerPoint == (int)TriggerPoint.After)
                script = triggerScriptService.GetDefaultMetaObjectTriggerScriptByServiceTypeAfter(serviceType).TrimStart().TrimEnd();
            return JsonResultModel.Success("get default trigger script", script);
        }
    }
}