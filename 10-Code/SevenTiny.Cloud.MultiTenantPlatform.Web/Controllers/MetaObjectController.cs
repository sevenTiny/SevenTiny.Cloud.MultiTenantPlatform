using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class MetaObjectController : Controller
    {
        IMetaObjectService metaObjectService;
        IMetaObjectAppService metaObjectAppService;

        public MetaObjectController(IMetaObjectService _metaObjectService,IMetaObjectAppService _metaObjectAppService)
        {
            metaObjectService = _metaObjectService;
            metaObjectAppService = _metaObjectAppService;
        }

        private int CurrentApplicationId => HttpContext.Session.GetInt32("ApplicationId") ?? throw new ArgumentNullException("ApplicationId is null,please select application first!");
        private string CurrentApplicationCode => HttpContext.Session.GetString("ApplicationCode") ?? throw new ArgumentNullException("ApplicationCode is null,please select application first!");

        public IActionResult List()
        {
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

            var addResult = metaObjectAppService.AddMetaObject(CurrentApplicationId, CurrentApplicationCode, metaObject);
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
            if (metaObjectService.ExistSameNameWithOtherIdByApplicationId(CurrentApplicationId, metaObject.Id, metaObject.Name))
            {
                return View("Update", ResponseModel.Error("MetaObject Name Has Been Exist！", metaObject));
            }

            metaObjectService.Update(metaObject);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            metaObjectAppService.Delete(id);
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
            HttpContext.Session.SetInt32("MetaObjectId", id);
            //这里换成当前MetaObject的MetaFields列表
            return Redirect("/MetaField/List");
        }
    }
}