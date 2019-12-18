using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.Enum;
using SevenTiny.Cloud.MultiTenant.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;
using System;

namespace SevenTiny.Cloud.DevelopmentWeb.Controllers
{
    public class IndexViewController : WebControllerBase
    {
        readonly IIndexViewService indexViewService;
        readonly IFieldListService interfaceFieldService;
        readonly ISearchConditionService searchConditionService;

        public IndexViewController(
            IIndexViewService _indexViewService,
            IFieldListService _interfaceFieldService,
            ISearchConditionService _searchConditionService
            )
        {
            this.indexViewService = _indexViewService;
            this.interfaceFieldService = _interfaceFieldService;
            this.searchConditionService = _searchConditionService;
        }

        public IActionResult List()
        {
            return View(indexViewService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult DeleteList()
        {
            return View(indexViewService.GetEntitiesDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult Add()
        {
            var indexView = new SevenTiny.Cloud.MultiTenant.Core.Entity.IndexView();
            indexView.Icon = "bars";
            ViewData["InterfaceFields"] = interfaceFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            ViewData["SearchConditions"] = searchConditionService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            return View(ResponseModel.Success(indexView));
        }

        public IActionResult AddLogic(IndexView entity)
        {
            ViewData["InterfaceFields"] = interfaceFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            ViewData["SearchConditions"] = searchConditionService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);

            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Add", ResponseModel.Error("名称不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Add", ResponseModel.Error("编码不能为空", entity));
            }

            //校验code格式
            if (!entity.Code.IsAlnum(2, 50))
            {
                return View("Add", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", entity));
            }

            //检查编码或名称重复
            var checkResult = indexViewService.CheckSameCodeOrName(CurrentMetaObjectId, entity);
            if (!checkResult.IsSuccess)
            {
                return View("Add", checkResult.ToResponseModel());
            }

            if (entity.FieldListId == default(int))
            {
                return View("Add", ResponseModel.Error("接口字段不能为空", entity));
            }
            if (entity.SearchConditionId == default(int))
            {
                return View("Add", ResponseModel.Error("条件不能为空", entity));
            }

            entity.MetaObjectId = CurrentMetaObjectId;
            //组合编码
            entity.Code = $"{CurrentMetaObjectCode}.IndexView.{entity.Code}";
            entity.CreateBy = CurrentUserId;
            indexViewService.Add(entity);

            return RedirectToAction("List");
        }

        public IActionResult Update(int id)
        {
            ViewData["InterfaceFields"] = interfaceFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            ViewData["SearchConditions"] = searchConditionService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);

            var metaObject = indexViewService.GetById(id);
            return View(ResponseModel.Success(metaObject));
        }

        public IActionResult UpdateLogic(IndexView entity)
        {
            ViewData["InterfaceFields"] = interfaceFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            ViewData["SearchConditions"] = searchConditionService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);

            if (entity.Id == 0)
            {
                return View("Update", ResponseModel.Error("修改的id传递错误", entity));
            }
            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Update", ResponseModel.Error("名称不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Update", ResponseModel.Error("编码不能为空", entity));
            }

            ////校验code格式
            //if (!entity.Code.IsAlnum(2, 50))
            //{
            //    return View("Add", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", entity));
            //}

            //检查编码或名称重复
            var checkResult = indexViewService.CheckSameCodeOrName(CurrentMetaObjectId, entity);
            if (!checkResult.IsSuccess)
            {
                return View("Update", checkResult.ToResponseModel());
            }

            if (entity.FieldListId == default(int))
            {
                return View("Add", ResponseModel.Error("接口字段不能为空", entity));
            }
            if (entity.SearchConditionId == default(int))
            {
                return View("Add", ResponseModel.Error("条件不能为空", entity));
            }
            entity.ModifyBy = CurrentUserId;
            indexViewService.Update(entity);

            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            indexViewService.Delete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult LogicDelete(int id)
        {
            indexViewService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            indexViewService.Recover(id);
            return JsonResultModel.Success("恢复成功");
        }
    }
}