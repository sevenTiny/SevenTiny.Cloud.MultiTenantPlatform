using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System;

namespace Seventiny.Cloud.DevelopmentWeb.Controllers
{
    public class MetaObjectController : ControllerBase
    {
        IMetaObjectService metaObjectService;

        public MetaObjectController(IMetaObjectService _metaObjectService)
        {
            metaObjectService = _metaObjectService;
        }

        public IActionResult Setting(int applicationId, string applicationCode)
        {
            if (applicationId == 0 || string.IsNullOrEmpty(applicationCode))
                return Redirect("/Application/Select");

            ViewData["MetaObjects"] = metaObjectService.GetMetaObjectsUnDeletedByApplicationId(applicationId);

            ViewData["Application"] = applicationCode;
            ViewData["ApplicationId"] = applicationId;
            return View();
        }

        public IActionResult List(int applicationId, string applicationCode)
        {
            if (applicationId == 0 || string.IsNullOrEmpty(applicationCode))
                return Redirect("/Application/Select");

            SetApplictionSession(applicationId, applicationCode);
            return View(metaObjectService.GetMetaObjectsUnDeletedByApplicationId(CurrentApplicationId));
        }

        public IActionResult DeleteList()
        {
            return View(metaObjectService.GetMetaObjectsDeletedByApplicationId(CurrentApplicationId));
        }

        public IActionResult Add()
        {
            MetaObject metaObject = new MetaObject();
            return View(ResponseModel.Success(metaObject));
        }

        public IActionResult AddLogic(MetaObject metaObject)
        {
            if (string.IsNullOrEmpty(metaObject.Name))
            {
                return View("Add", ResponseModel.Error("MetaObject Name Can Not Be Null！", metaObject));
            }
            if (string.IsNullOrEmpty(metaObject.Code))
            {
                return View("Add", ResponseModel.Error("MetaObject Code Can Not Be Null！", metaObject));
            }
            //校验code格式
            if (!metaObject.Code.IsAlnum(2, 50))
            {
                return View("Add", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", metaObject));
            }

            var addResult = metaObjectService.AddMetaObject(CurrentApplicationId, CurrentApplicationCode, metaObject);
            if (!addResult.IsSuccess)
            {
                return View("Add", addResult.ToResponseModel());
            }

            return RedirectToAction("List");
        }

        public IActionResult Update(int id)
        {
            var metaObject = metaObjectService.GetById(id);
            return View(ResponseModel.Success(metaObject));

        }

        public IActionResult UpdateLogic(MetaObject metaObject)
        {
            if (metaObject.Id == 0)
            {
                return View("Update", ResponseModel.Error("MetaObject Id Can Not Be Null！", metaObject));
            }
            if (string.IsNullOrEmpty(metaObject.Name))
            {
                return View("Update", ResponseModel.Error("MetaObject Name Can Not Be Null！", metaObject));
            }
            if (string.IsNullOrEmpty(metaObject.Code))
            {
                return View("Update", ResponseModel.Error("MetaObject Code Can Not Be Null！", metaObject));
            }
            //校验code格式，编辑时界面会将'.'传递到后台，不能用下面正则校验，且编辑操作并不会更新编码，无需在此校验
            //if (!metaObject.Code.IsAlnum(2, 50))
            //{
            //    return View("Update", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", metaObject));
            //}

            if (metaObjectService.ExistSameNameWithOtherIdByApplicationId(CurrentApplicationId, metaObject.Id, metaObject.Name))
            {
                return View("Update", ResponseModel.Error("MetaObject Name Has Been Exist！", metaObject));
            }

            metaObjectService.Update(metaObject);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            metaObjectService.Delete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult LogicDelete(int id)
        {
            metaObjectService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            metaObjectService.Recover(id);
            return JsonResultModel.Success("恢复成功");
        }

        public IActionResult Switch(int id)
        {
            var obj = metaObjectService.GetById(id);
            SetMetaObjectSession(id, obj.Code);

            //这里换成当前MetaObject的MetaFields列表
            return Redirect("/MetaField/List");
        }
    }
}