using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;
using System;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class MetaFieldController : WebControllerBase
    {
        readonly IMetaFieldService _metaFieldService;
        readonly IMetaFieldRepository _metaFieldRepository;
        readonly IMetaObjectRepository _metaObjectRepository;

        public MetaFieldController(IMetaFieldService metaFieldService, IMetaObjectRepository metaObjectRepository, IMetaFieldRepository metaFieldRepository)
        {
            _metaFieldService = metaFieldService;
            _metaObjectRepository = metaObjectRepository;
            _metaFieldRepository = metaFieldRepository;
        }

        public IActionResult List(Guid metaObjectId)
        {
            SetMetaObjectInfoToSession(metaObjectId);
            return View(_metaFieldRepository.GetListUnDeletedByMetaObjectId(metaObjectId));
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
                    return _metaFieldService.Add(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(entity));

            return Redirect("/MetaField/List?metaObjectId=" + CurrentMetaObjectId);
        }

        public IActionResult Update(Guid id)
        {
            var metaObject = _metaFieldRepository.GetById(id);
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
            return _metaFieldRepository.LogicDelete(id).ToJsonResultModel();
        }
    }
}