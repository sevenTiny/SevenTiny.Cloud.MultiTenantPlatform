using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;
using System;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class MetaFieldController : WebControllerBase
    {
        readonly IMetaFieldService _metaFieldService;
        private IMetaFieldAppService _metaFieldAppService;

        public MetaFieldController(IMetaFieldService metaFieldService, IMetaFieldAppService metaFieldAppService)
        {
            _metaFieldService = metaFieldService;
            _metaFieldAppService = metaFieldAppService;
        }

        public IActionResult List(Guid metaObjectId)
        {
            SetMetaObjectInfoToSession(metaObjectId);
            return View(_metaFieldService.GetListUnDeletedByMetaObjectId(metaObjectId));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(MetaField entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    entity.MetaObjectId = CurrentMetaObjectId;
                    entity.CreateBy = CurrentUserId;
                    entity.ShortCode = entity.Name;
                    return _metaFieldService.Add(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(entity));

            return Redirect("/MetaField/List?metaObjectId=" + CurrentMetaObjectId);
        }

        public IActionResult Update(Guid id)
        {
            var metaObject = _metaFieldService.GetById(id);
            return View(ResponseModel.Success(metaObject));
        }

        public IActionResult UpdateLogic(MetaField entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _metaFieldService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return Redirect("/MetaField/List?metaObjectId=" + CurrentMetaObjectId);
        }

        public IActionResult LogicDelete(Guid id)
        {
            return _metaFieldService.LogicDelete(id).ToJsonResultModel();
        }
    }
}