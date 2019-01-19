using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class MetaFieldController : ControllerBase
    {
        private readonly IMetaFieldService _metaFieldService;

        public MetaFieldController(IMetaFieldService metaFieldService)
        {
            this._metaFieldService = metaFieldService;
        }

        public IActionResult List(int metaObjectId)
        {
            //如果传递过来是0，表示没有选择对象
            if (metaObjectId == 0)
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "请先选择对象";
                return View();
            }

            //这里是选择对象的入口，预先设置Session
            HttpContext.Session.SetInt32("MetaObjectId", metaObjectId);

            return View(_metaFieldService.GetMetaFieldsUnDeletedByMetaObjectId(metaObjectId));
        }

        public IActionResult DeleteList()
        {
            return View(_metaFieldService.GetMetaFieldsDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(MetaField metaField)
        {
            if (string.IsNullOrEmpty(metaField.Name))
            {
                return View("Add", ResponseModel.Error("名称不能为空", metaField));
            }
            if (string.IsNullOrEmpty(metaField.Code))
            {
                return View("Add", ResponseModel.Error("编码不能为空", metaField));
            }
            //校验code格式
            if (!metaField.Code.IsAlnum(4, 50))
            {
                return View("Add", ResponseModel.Error("编码不合法，4-50位且只能包含字母和数字（字母开头）", metaField));
            }

            //检查编码或名称重复
            var checkResult = _metaFieldService.CheckSameCodeOrName(CurrentMetaObjectId, metaField);
            if (!checkResult.IsSuccess)
            {
                return View("Add", checkResult.ToResponseModel());
            }

            metaField.MetaObjectId = CurrentMetaObjectId;
            _metaFieldService.Add(metaField);

            return Redirect("/MetaField/List?metaObjectId=" + CurrentMetaObjectId);
        }

        public IActionResult Update(int id)
        {
            var metaObject = _metaFieldService.GetById(id);
            return View(ResponseModel.Success(metaObject));

        }

        public IActionResult UpdateLogic(MetaField metaField)
        {
            if (metaField.Id == 0)
            {
                return View("Update", ResponseModel.Error("MetaField Id 不能为空", metaField));
            }
            if (string.IsNullOrEmpty(metaField.Name))
            {
                return View("Update", ResponseModel.Error("MetaField Name 不能为空", metaField));
            }
            if (string.IsNullOrEmpty(metaField.Code))
            {
                return View("Update", ResponseModel.Error("MetaField Code 不能为空", metaField));
            }
            //校验code格式
            if (!metaField.Code.IsAlnum(4, 50))
            {
                return View("Update", ResponseModel.Error("编码不合法，4-50位且只能包含字母和数字（字母开头）", metaField));
            }

            //检查编码或名称重复
            var checkResult = _metaFieldService.CheckSameCodeOrName(CurrentMetaObjectId, metaField);
            if (!checkResult.IsSuccess)
            {
                return View("Update", checkResult.ToResponseModel());
            }

            //更新操作
            _metaFieldService.Update(metaField);

            return Redirect("/MetaField/List?metaObjectId=" + CurrentMetaObjectId);
        }

        public IActionResult Delete(int id)
        {
            _metaFieldService.Delete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            _metaFieldService.Recover(id);
            return JsonResultModel.Success("恢复成功");
        }
    }
}