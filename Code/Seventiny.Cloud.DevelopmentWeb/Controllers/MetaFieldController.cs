using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;

namespace SevenTiny.Cloud.DevelopmentWeb.Controllers
{
    public class MetaFieldController : WebControllerBase
    {
        readonly IMetaFieldService metaFieldService;
        readonly IMetaObjectService metaObjectService;

        public MetaFieldController(
            IMetaFieldService _metaFieldService,
            IMetaObjectService _metaObjectService
            )
        {
            metaFieldService = _metaFieldService;
            metaObjectService = _metaObjectService;
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
            var obj = metaObjectService.GetById(metaObjectId);
            if (obj != null)
            {
                HttpContext.Session.SetString("MetaObjectCode", obj.Code);
            }

            return View(metaFieldService.GetEntitiesUnDeletedByMetaObjectId(metaObjectId));
        }

        public IActionResult DeleteList()
        {
            return View(metaFieldService.GetEntitiesDeletedByMetaObjectId(CurrentMetaObjectId));
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
            if (!metaField.Code.IsAlnum(2, 50))
            {
                return View("Add", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", metaField));
            }

            //检查编码或名称重复
            var checkResult = metaFieldService.CheckSameCodeOrName(CurrentMetaObjectId, metaField);
            if (!checkResult.IsSuccess)
            {
                return View("Add", checkResult.ToResponseModel());
            }

            metaField.MetaObjectId = CurrentMetaObjectId;
            metaField.CreateBy = CurrentUserId;
            metaFieldService.Add(metaField);

            return Redirect("/MetaField/List?metaObjectId=" + CurrentMetaObjectId);
        }

        public IActionResult Update(int id)
        {
            var metaObject = metaFieldService.GetById(id);
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
            //校验code格式，编码不允许修改，这里无需判断
            //if (!metaField.Code.IsAlnum(2, 50))
            //{
            //    return View("Update", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", metaField));
            //}

            //检查编码或名称重复
            var checkResult = metaFieldService.CheckSameCodeOrName(CurrentMetaObjectId, metaField);
            if (!checkResult.IsSuccess)
            {
                return View("Update", checkResult.ToResponseModel());
            }

            metaField.ModifyBy = CurrentUserId;
            //更新操作
            metaFieldService.Update(metaField);

            return Redirect("/MetaField/List?metaObjectId=" + CurrentMetaObjectId);
        }

        public IActionResult Delete(int id)
        {
            return metaFieldService.Delete(id).ToJsonResultModel();
        }

        public IActionResult Recover(int id)
        {
            metaFieldService.Recover(id);
            return JsonResultModel.Success("恢复成功");
        }
    }
}