using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class MetaObjectController : Controller
    {
        private readonly IRepository<MetaObject> _metaObjectRepository;
        private readonly IRepository<DomainModel.Entities.Application> _applicationRepository;

        public MetaObjectController(IRepository<MetaObject> metaObjectRepository, IRepository<DomainModel.Entities.Application> applicationRepository)
        {
            this._metaObjectRepository = metaObjectRepository;
            this._applicationRepository = applicationRepository;
        }

        public IActionResult List()
        {
            return View(_metaObjectRepository.GetList(t => t.IsDeleted == (int)IsDeleted.NotDeleted));
        }

        public IActionResult DeleteList()
        {
            return View(_metaObjectRepository.GetList(t => t.IsDeleted == (int)IsDeleted.Deleted));
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
            MetaObject obj = _metaObjectRepository.GetEntity(t => t.Name.Equals(metaObject.Name) || t.Code.Equals(metaObject.Code));
            if (obj.Code.Equals(metaObject.Code))
            {
                return View("Add", new ActionResultModel<MetaObject>(false, "MetaObject Code Has Been Exist！", metaObject));
            }
            if (obj.Name.Equals(metaObject.Name))
            {
                return View("Add", new ActionResultModel<MetaObject>(false, "MetaObject Name Has Been Exist！", metaObject));
            }
            string applicationCode = HttpContext.Session.GetString("ApplicationCode");
            DomainModel.Entities.Application application = _applicationRepository.GetEntity(t => t.Code.Equals(applicationCode));
            metaObject.ApplicationId = application.Id;
            _metaObjectRepository.Add(metaObject);
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
            return RedirectToAction("List");
        }
    }
}