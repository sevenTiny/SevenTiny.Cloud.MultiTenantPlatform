using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Enum;
using SevenTiny.Cloud.Account.Core.ServiceContract;
using SevenTiny.Cloud.Account.Models;

namespace SevenTiny.Cloud.Account.Controllers
{
    [Authorize("Administrator")]
    public class TenantApplicationLicenseController : WebControllerBase
    {
        ITenantApplicationLicenseService _tenantApplicationLicenseService;
        public TenantApplicationLicenseController(
            ITenantApplicationLicenseService tenantApplicationLicenseService
            )
        {
            _tenantApplicationLicenseService = tenantApplicationLicenseService;
        }

        public IActionResult List()
        {
            var list = _tenantApplicationLicenseService.GetUnDeletedEntitiesByTenantId(CurrentTenantId);
            return View(list);
        }

        public IActionResult AllTenantList()
        {
            var list = _tenantApplicationLicenseService.GetEntitiesUnDeleted();
            return View(list);
        }

        public IActionResult Add()
        {
            ViewData["ApplicationIdNameDic"] = _tenantApplicationLicenseService.GetApplicationIdNameDic();
            return View(ResponseModel.Success(new TenantApplicationLicense()));
        }

        //系统管理员进行添加
        public IActionResult AddLogic(TenantApplicationLicense entity)
        {
            var result = Result.Success("添加成功")
                .Continue(re =>
                {
                    entity.CreateBy = CurrentUserId;
                    return _tenantApplicationLicenseService.Add(entity);
                });

            if (result.IsSuccess)
            {
                return Redirect("/TenantApplicationLicense/AllTenantList");
            }

            ViewData["ApplicationIdNameDic"] = _tenantApplicationLicenseService.GetApplicationIdNameDic();
            return View("Add", ResponseModel.Error(result.Message, entity));
        }

        public IActionResult EnableApplication(int id)
        {
            var entity = _tenantApplicationLicenseService.EnableApplication(id);
            return JsonResultModel.Success("启用成功"); ;
        }

        public IActionResult DisableApplication(int id)
        {
            var entity = _tenantApplicationLicenseService.DisableApplication(id);
            return JsonResultModel.Success("停用成功"); ;
        }

        public IActionResult Delete(int id)
        {
            _tenantApplicationLicenseService.Delete(id);
            return JsonResultModel.Success("删除成功");
        }
    }
}