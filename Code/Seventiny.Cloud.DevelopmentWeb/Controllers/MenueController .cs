using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;

namespace SevenTiny.Cloud.DevelopmentWeb.Controllers
{
    public class MenueController : WebControllerBase
    {
        IMenueService _menueService;

        public MenueController(
            IMenueService menueService)
        {
            _menueService = menueService;
        }

        public IActionResult Setting()
        {
            return View();
        }

        public IActionResult List()
        {
            var list = _menueService.GetUnDeletedEntitiesByApplicationId(CurrentApplicationId);
            return View(list);
        }

        public JsonResult AnalysisMenue()
        {
            return _menueService.AnalysisMenueTree().ToJsonResultModel();
        }

        public IActionResult Add()
        {
            var entity = new Menue();
            entity.Icon = "reorder";
            return View(ResponseModel.Success(entity));
        }

        public IActionResult AddLogic(Menue entity)
        {
            var result =  Result.Success("Add succeed!")
                .ContinueAssert(!string.IsNullOrEmpty(entity.Name), "Name Can Not Be Null！")
                .ContinueAssert(!string.IsNullOrEmpty(entity.Code), "Code Can Not Be Null！")
                .Continue(re =>
                {
                    if (!entity.Code.IsAlnum(2, 50))
                        return Result.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）");
                    return re;
                })
                .Continue(re =>
                {
                    entity.ApplicationId = CurrentApplicationId;
                    entity.ApplicationCode = CurrentApplicationCode;
                    return re;
                })
                .Continue(re => {
            entity.CreateBy = CurrentUserId;
                    return _menueService.Add(entity);
                });

            if (result.IsSuccess)
                return Redirect("List");
            else
                return View("Add", result.ToResponseModel());
        }

        public IActionResult Update(int id)
        {
            var application = _menueService.GetById(id);
            return View(ResponseModel.Success(application));
        }

        public IActionResult UpdateLogic(Menue entity)
        {
            if (entity.Id == 0)
            {
                return View("Update", ResponseModel.Error("Id Can Not Be Null！", entity));
            }
            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Update", ResponseModel.Error(" Name Can Not Be Null！", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Update", ResponseModel.Error("Code Can Not Be Null！", entity));
            }

            entity.ModifyBy = CurrentUserId;
            _menueService.Update(entity);
            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(int id)
        {
            _menueService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Delete(int id)
        {
            _menueService.Delete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            _menueService.Recover(id);
            return JsonResultModel.Success("恢复成功");
        }
    }
}