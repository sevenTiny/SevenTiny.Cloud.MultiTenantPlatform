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

            return View(_metaFieldService.GetMetaFeildsUnDeletedByMetaObjectId(metaObjectId));
        }

        public IActionResult DeleteList()
        {
            return View(_metaFieldService.GetMetaFeildsDeletedByMetaObjectId(CurrentMetaObjectId));
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


        //public IActionResult Update(int id)
        //{
        //    var metaObject = _metaFieldRepository.GetEntity(t => t.Id == id);
        //    return View(new ActionResultModel<MetaField>(true, string.Empty, metaObject));

        //}

        //public IActionResult UpdateLogic(MetaField metaField)
        //{
        //    if (metaField.Id == 0)
        //    {
        //        return View("Update", ResponseModel.Error( "MetaField Id 不能为空", metaField));
        //    }
        //    if (string.IsNullOrEmpty(metaField.Name))
        //    {
        //        return View("Update", ResponseModel.Error( "MetaField Name 不能为空", metaField));
        //    }
        //    if (string.IsNullOrEmpty(metaField.Code))
        //    {
        //        return View("Update", ResponseModel.Error( "MetaField Code 不能为空", metaField));
        //    }
        //    if (_metaFieldRepository.Exist(t => t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(metaField.Name) && t.Id != metaField.Id))
        //    {
        //        return View("Add", ResponseModel.Error( "MetaField Name Has Been Exist", metaField));
        //    }
        //    MetaField myfield = _metaFieldRepository.GetEntity(t => t.Id == metaField.Id);
        //    if (myfield != null)
        //    {
        //        myfield.Name = metaField.Name;
        //        myfield.Group = metaField.Group;
        //        myfield.SortNumber = metaField.SortNumber;
        //        myfield.Description = metaField.Description;
        //        myfield.FieldType = metaField.FieldType;
        //        myfield.DataSourceId = metaField.DataSourceId;
        //        myfield.ModifyBy = -1;
        //        myfield.ModifyTime = DateTime.Now;
        //    }
        //    _metaFieldRepository.Update(t => t.Id == metaField.Id, myfield);
        //    return RedirectToAction("List");
        //}

        //public IActionResult Delete(int id)
        //{
        //    MetaField metaObject = _metaFieldRepository.GetEntity(t => t.Id == id);
        //    _metaFieldRepository.Delete(t => t.Id == id);
        //    return JsonResultModel.Success("删除成功");
        //}

        //public IActionResult Recover(int id)
        //{
        //    MetaField metaObject = _metaFieldRepository.GetEntity(t => t.Id == id);
        //    _metaFieldRepository.Recover(t => t.Id == id, metaObject);
        //    return JsonResultModel.Success("恢复成功");
        //}
    }
}