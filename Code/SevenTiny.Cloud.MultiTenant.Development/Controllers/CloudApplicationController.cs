using Microsoft.AspNetCore.Http;
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
    public class CloudApplicationController : WebControllerBase
    {
        public CloudApplicationController(ICloudApplicationService applicationService, ICloudApplicationRepository cloudApplicationRepository)
        {
            _applicationService = applicationService;
            _cloudApplicationRepository = cloudApplicationRepository;
        }

        ICloudApplicationService _applicationService;
        ICloudApplicationRepository _cloudApplicationRepository;

        public IActionResult Setting()
        {
            return View();
        }

        public IActionResult Select()
        {
            var list = _cloudApplicationRepository.GetListUnDeleted();
            return View(list);
        }

        public IActionResult List()
        {
            var list = _cloudApplicationRepository.GetListUnDeleted();
            return View(list);
        }

        public IActionResult Add()
        {
            var application = new CloudApplication
            {
                Icon = "cloud"
            };
            application.Icon = "cloud";
            return View(ResponseModel.Success(application));
        }

        public IActionResult AddLogic(CloudApplication entity)
        {
            var result = Result<CloudApplication>.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    entity.CreateBy = CurrentUserId;
                    return _applicationService.Add(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult Update(Guid id)
        {
            var application = _cloudApplicationRepository.GetById(id);
            return View(ResponseModel.Success(application));
        }

        public IActionResult UpdateLogic(CloudApplication entity)
        {
            var result = Result<CloudApplication>.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _applicationService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(Guid id)
        {
            _cloudApplicationRepository.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Detail(Guid applicationId, string applicationCode)
        {
            if (string.IsNullOrEmpty(applicationCode))
                return Redirect("/CloudApplication/Select");

            SetApplictionSession(applicationId, applicationCode);
            ViewData["Application"] = applicationCode;
            SetUserInfoToViewData();

            return View();
        }
    }
}