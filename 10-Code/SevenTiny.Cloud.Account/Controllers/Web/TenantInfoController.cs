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
    public class TenantInfoController : WebControllerBase
    {
        ITenantInfoService _tenantInfoService;
        public TenantInfoController(
            ITenantInfoService tenantInfoService
            )
        {
            _tenantInfoService = tenantInfoService;
        }

        public IActionResult List()
        {
            var list = _tenantInfoService.GetEntitiesUnDeleted();
            return View(list);
        }

        public IActionResult Add()
        {
            return View(ResponseModel.Success(new TenantInfo()));
        }

        public IActionResult AddLogic(TenantInfo entity)
        {
            var result = Result.Success("添加成功")
                .Continue(re =>
                {
                    if (string.IsNullOrEmpty(entity.OperatorName))
                        entity.OperatorName = CurrentUserName;
                    entity.CreateBy = CurrentUserId;
                    //默认激活
                    entity.IsActive = (int)TrueFalse.True;
                    return _tenantInfoService.Add(entity);
                });

            if (result.IsSuccess)
            {
                return Redirect("/TenantInfo/List");
            }
            return View("Add", ResponseModel.Error(result.Message, entity));
        }

        public IActionResult Update(int id)
        {
            var entity = _tenantInfoService.GetById(id);
            return View(ResponseModel.Success(entity));
        }

        public IActionResult UpdateLogic(TenantInfo entity)
        {
            var result = Result.Success("修改成功")
                .Continue(re =>
                {
                    entity.ModifyBy = CurrentUserId;
                    return _tenantInfoService.Update(entity);
                });

            if (result.IsSuccess)
            {
                return Redirect("/TenantInfo/List");
            }
            return View(ResponseModel.Error(result.Message, entity));
        }

        public IActionResult LogicDelete(int id)
        {
            _tenantInfoService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }
    }
}