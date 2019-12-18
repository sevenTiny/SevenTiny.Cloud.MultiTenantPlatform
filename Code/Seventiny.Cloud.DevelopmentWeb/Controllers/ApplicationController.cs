using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;

namespace SevenTiny.Cloud.DevelopmentWeb.Controllers
{
    public class ApplicationController : WebControllerBase
    {
        IApplicationService applicationService;
        IMetaObjectService metaObjectService;

        public ApplicationController(
            IApplicationService _applicationService,
            IMetaObjectService _metaObjectService)
        {
            applicationService = _applicationService;
            metaObjectService = _metaObjectService;
        }

        public IActionResult Setting()
        {
            return View();
        }

        public IActionResult Select()
        {
            var list = applicationService.GetEntitiesUnDeleted();
            return View(list);
        }

        public IActionResult List()
        {
            var list = applicationService.GetEntitiesUnDeleted();
            return View(list);
        }

        public IActionResult DeleteList()
        {
            var list = applicationService.GetEntitiesDeleted();
            return View(list);
        }

        public IActionResult Add()
        {
            var application = new SevenTiny.Cloud.MultiTenant.Core.Entity.Application();
            application.Icon = "cloud";
            return View(ResponseModel.Success(application));
        }

        public IActionResult AddLogic(SevenTiny.Cloud.MultiTenant.Core.Entity.Application application)
        {
            if (string.IsNullOrEmpty(application.Name))
            {
                return View("Add", ResponseModel.Error("Application Name Can Not Be Null！", application));
            }
            if (string.IsNullOrEmpty(application.Code))
            {
                return View("Add", ResponseModel.Error("Application Code Can Not Be Null！", application));
            }
            //校验code格式
            if (!application.Code.IsAlnum(2, 50))
            {
                return View("Add", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", application));
            }

            application.CreateBy = CurrentUserId;
            var addResult = applicationService.Add(application);
            if (!addResult.IsSuccess)
            {
                return View("Add", addResult.ToResponseModel());
            }

            return RedirectToAction("List");
        }

        public IActionResult Update(int id)
        {
            var application = applicationService.GetById(id);
            return View(ResponseModel.Success(application));
        }

        public IActionResult UpdateLogic(SevenTiny.Cloud.MultiTenant.Core.Entity.Application application)
        {
            if (application.Id == 0)
            {
                return View("Update", ResponseModel.Error("Application Id Can Not Be Null！", application));
            }
            if (string.IsNullOrEmpty(application.Name))
            {
                return View("Update", ResponseModel.Error("Application Name Can Not Be Null！", application));
            }
            if (string.IsNullOrEmpty(application.Code))
            {
                return View("Update", ResponseModel.Error("Application Code Can Not Be Null！", application));
            }

            application.ModifyBy = CurrentUserId;
            applicationService.Update(application);
            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(int id)
        {
            applicationService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Delete(int id)
        {
            applicationService.Delete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            applicationService.Recover(id);
            return JsonResultModel.Success("恢复成功");
        }

        public IActionResult Detail(string app)
        {
            if (string.IsNullOrEmpty(app))
                return Redirect("/Application/Select");

            var application = applicationService.GetByCode(app);

            SetApplictionSession(application.Id, application.Code);
            ViewData["Application"] = application.Code;
            SetUserInfoToViewData();

            return View();
        }
    }
}