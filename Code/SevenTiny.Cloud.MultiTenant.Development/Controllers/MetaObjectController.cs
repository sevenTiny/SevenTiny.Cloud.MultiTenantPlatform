using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;
using System;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class MetaObjectController : WebControllerBase
    {
        public MetaObjectController(IMetaObjectService metaObjectService)
        {
            _metaObjectService = metaObjectService;
        }

        IMetaObjectService _metaObjectService;

        public IActionResult List(Guid applicationId)
        {
            return View(_metaObjectService.GetMetaObjectListUnDeletedByApplicationId(applicationId));
        }

        public IActionResult Add()
        {
            MetaObject metaObject = new MetaObject();
            return View(ResponseModel.Success(metaObject));
        }

        public IActionResult AddLogic(MetaObject entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    entity.CreateBy = CurrentUserId;
                    return _metaObjectService.Add(CurrentApplicationId, CurrentApplicationCode, entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult Update(Guid id)
        {
            var metaObject = _metaObjectService.GetById(id);
            return View(ResponseModel.Success(metaObject));

        }

        public IActionResult UpdateLogic(MetaObject entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _metaObjectService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(Guid id)
        {
            _metaObjectService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }
    }
}