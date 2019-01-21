using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class InterfaceAggregationController : ControllerBase
    {
        private readonly IInterfaceAggregationService interfaceAggregationService;
        private readonly IInterfaceFieldService interfaceFieldService;
        private readonly IInterfaceSearchConditionService interfaceSearchConditionService;

        public InterfaceAggregationController(
            IInterfaceAggregationService _interfaceAggregationService,
            IInterfaceFieldService _interfaceFieldService,
            IInterfaceSearchConditionService _interfaceSearchConditionService
            )
        {
            this.interfaceAggregationService = _interfaceAggregationService;
            this.interfaceFieldService = _interfaceFieldService;
            this.interfaceSearchConditionService = _interfaceSearchConditionService;
        }

        public IActionResult List()
        {
            return View(interfaceAggregationService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult DeleteList()
        {
            return View(interfaceAggregationService.GetEntitiesDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult Add()
        {
            ViewData["InterfaceFields"] = interfaceFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            ViewData["SearchConditions"] = interfaceSearchConditionService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            return View();
        }

        public IActionResult AddLogic(InterfaceAggregation entity)
        {
            ViewData["InterfaceFields"] = interfaceFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            ViewData["SearchConditions"] = interfaceSearchConditionService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);

            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Add", ResponseModel.Error("名称不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Add", ResponseModel.Error("编码不能为空", entity));
            }

            //校验code格式
            if (!entity.Code.IsAlnum(4, 50))
            {
                return View("Add", ResponseModel.Error("编码不合法，4-50位且只能包含字母和数字（字母开头）", entity));
            }

            //检查编码或名称重复
            var checkResult = interfaceAggregationService.CheckSameCodeOrName(CurrentMetaObjectId, entity);
            if (!checkResult.IsSuccess)
            {
                return View("Add", checkResult.ToResponseModel());
            }

            if (entity.InterfaceFieldId == default(int))
            {
                return View("Add", ResponseModel.Error("接口字段不能为空", entity));
            }
            if (entity.InterfaceSearchConditionId == default(int))
            {
                return View("Add", ResponseModel.Error("条件不能为空", entity));
            }

            entity.MetaObjectId = CurrentMetaObjectId;
            interfaceAggregationService.Add(entity);

            return RedirectToAction("List");
        }

        public IActionResult Update(int id)
        {
            ViewData["InterfaceFields"] = interfaceFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            ViewData["SearchConditions"] = interfaceSearchConditionService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);

            var metaObject = interfaceAggregationService.GetById(id);
            return View(ResponseModel.Success(metaObject));
        }

        public IActionResult UpdateLogic(InterfaceAggregation entity)
        {
            ViewData["InterfaceFields"] = interfaceFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            ViewData["SearchConditions"] = interfaceSearchConditionService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);

            if (entity.Id == 0)
            {
                return View("Update", ResponseModel.Error("修改的id传递错误", entity));
            }
            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Add", ResponseModel.Error("名称不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Add", ResponseModel.Error("编码不能为空", entity));
            }

            //校验code格式
            if (!entity.Code.IsAlnum(4, 50))
            {
                return View("Add", ResponseModel.Error("编码不合法，4-50位且只能包含字母和数字（字母开头）", entity));
            }

            //检查编码或名称重复
            var checkResult = interfaceAggregationService.CheckSameCodeOrName(CurrentMetaObjectId, entity);
            if (!checkResult.IsSuccess)
            {
                return View("Add", checkResult.ToResponseModel());
            }

            if (entity.InterfaceFieldId == default(int))
            {
                return View("Add", ResponseModel.Error("接口字段不能为空", entity));
            }
            if (entity.InterfaceSearchConditionId == default(int))
            {
                return View("Add", ResponseModel.Error("条件不能为空", entity));
            }

            interfaceAggregationService.Update(entity);

            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            interfaceAggregationService.Delete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult LogicDelete(int id)
        {
            interfaceAggregationService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            interfaceAggregationService.Recover(id);
            return JsonResultModel.Success("恢复成功");
        }
    }
}