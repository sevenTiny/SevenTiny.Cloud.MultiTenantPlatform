using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;
using System;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class MenueController : WebControllerBase
    {
        IMenueService _menueService;

        public MenueController(IMenueService menueService)
        {
            _menueService = menueService;
        }

        public IActionResult Setting()
        {
            return View();
        }

        public IActionResult List()
        {
            var list = _menueService.GetListUnDeletedByMetaObjectId(CurrentMetaObjectId);
            return View(list);
        }

        public IActionResult Add()
        {
            var entity = new Menue();
            entity.Icon = "reorder";
            return View(ResponseModel.Success(entity));
        }

        public IActionResult AddLogic(Menue entity)
        {
            var result = Result.Success("Add succeed!")
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    entity.Code = $"{CurrentMetaObjectCode}.Menue.{entity.Code}";
                    entity.CreateBy = CurrentUserId;

                    return _menueService.Add(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(entity));

            return Redirect("List");
        }

        public IActionResult Update(Guid id)
        {
            var application = _menueService.GetById(id);
            return View(ResponseModel.Success(application));
        }

        public IActionResult UpdateLogic(Menue entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Id, nameof(entity.Id))
                .ContinueAssert(_ => entity.Id != Guid.Empty, "id can not be null")
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .Continue(_ =>
                {
                    entity.ModifyBy = CurrentUserId;
                    return _menueService.Update(entity);
                });


            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity)); ;

            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(Guid id)
        {
            _menueService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }
    }
}