using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.Application;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using SevenTiny.Bantina;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class MetaObjectController : Controller
    {
        private readonly IRepository<MetaField> _metaFieldRepository;
        private readonly IRepository<MetaObject> _metaObjectRepository;
        private readonly IRepository<DomainModel.Entities.Application> _applicationRepository;
        private readonly IMetaFieldService _metaFieldService;

        public MetaObjectController(
            IRepository<MetaObject> metaObjectRepository,
            IRepository<DomainModel.Entities.Application> applicationRepository,
            IMetaFieldService metaFieldService,
            IRepository<MetaField> metaFieldRepository
            )
        {
            this._metaObjectRepository = metaObjectRepository;
            this._applicationRepository = applicationRepository;
            this._metaFieldService = metaFieldService;
            this._metaFieldRepository = metaFieldRepository;
        }

        private int CurrentApplicationId
        {
            get
            {
                int applicationId = HttpContext.Session.GetInt32("ApplicationId") ?? default(int);
                if (applicationId == default(int))
                {
                    throw new ArgumentNullException("ApplicationId is null,please select application first!");
                }
                return applicationId;
            }
        }

        public IActionResult List()
        {
            return View(_metaObjectRepository.GetList(t => t.ApplicationId == CurrentApplicationId && t.IsDeleted == (int)IsDeleted.NotDeleted));
        }

        public IActionResult DeleteList()
        {
            return View(_metaObjectRepository.GetList(t => t.ApplicationId == CurrentApplicationId && t.IsDeleted == (int)IsDeleted.Deleted));
        }

        public IActionResult Add()
        {
            MetaObject metaObject = new MetaObject();
            return View(new ActionResultModel<MetaObject>(true, string.Empty, metaObject));
        }

        public IActionResult AddLogic(MetaObject metaObject)
        {
            if (string.IsNullOrEmpty(metaObject.Name))
            {
                return View("Add", new ActionResultModel<MetaObject>(false, "MetaObject Name Can Not Be Null！", metaObject));
            }
            if (string.IsNullOrEmpty(metaObject.Code))
            {
                return View("Add", new ActionResultModel<MetaObject>(false, "MetaObject Code Can Not Be Null！", metaObject));
            }
            //check metaobject of name or code exist?
            MetaObject obj = _metaObjectRepository.GetEntity(t => t.Name.Equals(metaObject.Name) || t.Code.Equals(metaObject.Code));
            if (obj != null)
            {
                if (obj.Code.Equals(metaObject.Code))
                {
                    return View("Add", new ActionResultModel<MetaObject>(false, "MetaObject Code Has Been Exist！", metaObject));
                }
                if (obj.Name.Equals(metaObject.Name))
                {
                    return View("Add", new ActionResultModel<MetaObject>(false, "MetaObject Name Has Been Exist！", metaObject));
                }
            }
            int applicationId = HttpContext.Session.GetInt32("ApplicationId") ?? default(int);
            if (applicationId == default(int))
            {
                return View("Add", new ActionResultModel<MetaObject>(false, "Application Id Can Not Be Null！", metaObject));
            }
            metaObject.ApplicationId = applicationId;
            _metaObjectRepository.Add(metaObject);
            obj = _metaObjectRepository.GetEntity(t => t.Code.Equals(metaObject.Code));
            if (obj == null)
            {
                return View("Add", new ActionResultModel<MetaObject>(false, "add metaobject then query faild！", metaObject));
            }
            //预置字段数据
            _metaFieldService.PresetFields(obj.Id);
            return RedirectToAction("List");
        }


        public IActionResult Update(int id)
        {
            var metaObject = _metaObjectRepository.GetEntity(t => t.Id == id);
            return View(new ActionResultModel<MetaObject>(true, string.Empty, metaObject));

        }

        public IActionResult UpdateLogic(MetaObject metaObject)
        {
            if (metaObject.Id == 0)
            {
                return View("Update", new ActionResultModel<MetaObject>(false, "MetaObject Id Can Not Be Null！", metaObject));
            }
            if (string.IsNullOrEmpty(metaObject.Name))
            {
                return View("Update", new ActionResultModel<MetaObject>(false, "MetaObject Name Can Not Be Null！", metaObject));
            }
            if (string.IsNullOrEmpty(metaObject.Code))
            {
                return View("Update", new ActionResultModel<MetaObject>(false, "MetaObject Code Can Not Be Null！", metaObject));
            }
            if (_metaObjectRepository.Exist(t => t.Name.Equals(metaObject.Name) && t.Id != metaObject.Id))
            {
                return View("Add", new ActionResultModel<MetaObject>(false, "MetaObject Name Has Been Exist！", metaObject));
            }
            MetaObject app = _metaObjectRepository.GetEntity(t => t.Id == metaObject.Id);
            if (app != null)
            {
                app.Name = metaObject.Name;
                app.Group = metaObject.Group;
                app.SortNumber = metaObject.SortNumber;
                app.Description = metaObject.Description;
                app.ModifyBy = -1;
                app.ModifyTime = DateTime.Now;
            }
            _metaObjectRepository.Update(t => t.Id == metaObject.Id, app);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            MetaObject metaObject = _metaObjectRepository.GetEntity(t => t.Id == id);
            //todo:添加事务支持
            _metaFieldRepository.Delete(t => t.MetaObjectId == id);
            _metaObjectRepository.Delete(t => t.Id == id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult LogicDelete(int id)
        {
            MetaObject metaObject = _metaObjectRepository.GetEntity(t => t.Id == id);
            _metaObjectRepository.LogicDelete(t => t.Id == id, metaObject);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            MetaObject metaObject = _metaObjectRepository.GetEntity(t => t.Id == id);
            _metaObjectRepository.Recover(t => t.Id == id, metaObject);
            return JsonResultModel.Success("恢复成功");
        }

        public IActionResult Switch(int id)
        {
            HttpContext.Session.SetInt32("MetaObjectId", id);
            //这里换成当前MetaObject的MetaFields列表
            return Redirect("/MetaField/List");
        }
    }
}