using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class TriggerScriptController : WebControllerBase
    {
        readonly ITriggerScriptService _triggerScriptService;

        public TriggerScriptController(ITriggerScriptService triggerScriptService)
        {
            _triggerScriptService = triggerScriptService;
        }

        public IActionResult List()
        {
            return View(_triggerScriptService.GetListUnDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(TriggerScript entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    //check script
                    //var checkResult2 = _triggerScriptService.CompilateAndCheckScript(entity.Script, CurrentApplicationCode);
                    //if (!checkResult2.IsSuccess)
                    //{
                    //    return Result<TriggerScript>.Error($"脚本存在错误：{checkResult2.Message}", entity);
                    //}

                    entity.MetaObjectId = CurrentMetaObjectId;
                    entity.Code = $"{CurrentMetaObjectCode}.TriggerScript.{entity.Code}";
                    entity.CreateBy = CurrentUserId;

                    return _triggerScriptService.Add(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult Update(Guid id)
        {
            var obj = _triggerScriptService.GetById(id);
            return View(ResponseModel.Success(obj));
        }

        public IActionResult UpdateLogic(TriggerScript entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   //check script
                   //var checkResult2 = _triggerScriptService.CompilateAndCheckScript(triggerScript.Script, CurrentApplicationCode);
                   //if (!checkResult2.IsSuccess)
                   //{
                   //    return Result<TriggerScript>.Error($"脚本存在错误：{checkResult2.Message}", entity);
                   //}

                   entity.ModifyBy = CurrentUserId;
                   return _triggerScriptService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(Guid id)
        {
            _triggerScriptService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }


        //public IActionResult GetDefaultTriggerScript(int serviceType, int triggerPoint)
        //{
        //    string script = string.Empty;
        //    if (triggerPoint == (int)TriggerPoint.Before)
        //        script = _triggerScriptService.GetDefaultMetaObjectTriggerScriptByServiceTypeBefore(serviceType).TrimStart().TrimEnd();
        //    else if (triggerPoint == (int)TriggerPoint.After)
        //        script = _triggerScriptService.GetDefaultMetaObjectTriggerScriptByServiceTypeAfter(serviceType).TrimStart().TrimEnd();
        //    return JsonResultModel.Success("get default trigger script", script);
        //}
    }
}