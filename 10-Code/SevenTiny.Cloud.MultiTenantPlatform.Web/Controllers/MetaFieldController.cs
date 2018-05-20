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
    public class MetaFieldController : Controller
    {
        private readonly IRepository<MetaField> _metaFieldRepository;

        public MetaFieldController(IRepository<MetaField> metaFieldRepository)
        {
            this._metaFieldRepository = metaFieldRepository;
        }

        private int CurrentMetaObjectId
        {
            get
            {
                int metaObjectId = HttpContext.Session.GetInt32("MetaObjectId") ?? default(int);
                if (metaObjectId == default(int))
                {
                    throw new ArgumentNullException("MetaObjectId is null,please select application first!");
                }
                return metaObjectId;
            }
        }

        public IActionResult List()
        {
            return View(_metaFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted));
        }

        public IActionResult DeleteList()
        {
            return View(_metaFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.Deleted));
        }

        public IActionResult Add()
        {
            MetaField metaObject = new MetaField();
            return View(new ActionResultModel<MetaField>(true, string.Empty, metaObject));
        }

        public IActionResult AddLogic(MetaField metaObject)
        {
            if (string.IsNullOrEmpty(metaObject.Name))
            {
                return View("Add", new ActionResultModel<MetaField>(false, "MetaField Name Can Not Be Null！", metaObject));
            }
            if (string.IsNullOrEmpty(metaObject.Code))
            {
                return View("Add", new ActionResultModel<MetaField>(false, "MetaField Code Can Not Be Null！", metaObject));
            }
            MetaField obj = _metaFieldRepository.GetEntity(t => t.Name.Equals(metaObject.Name) || t.Code.Equals(metaObject.Code));
            if (obj.Code.Equals(metaObject.Code))
            {
                return View("Add", new ActionResultModel<MetaField>(false, "MetaField Code Has Been Exist！", metaObject));
            }
            if (obj.Name.Equals(metaObject.Name))
            {
                return View("Add", new ActionResultModel<MetaField>(false, "MetaField Name Has Been Exist！", metaObject));
            }
            int applicationId = HttpContext.Session.GetInt32("MetaObjectId") ?? default(int);
            if (applicationId == default(int))
            {
                return View("Add", new ActionResultModel<MetaField>(false, "MetaObject Id Can Not Be Null！", metaObject));
            }
            metaObject.MetaObjectId = applicationId;
            _metaFieldRepository.Add(metaObject);
            return RedirectToAction("List");
        }


        public IActionResult Update(int id)
        {
            var metaObject = _metaFieldRepository.GetEntity(t => t.Id == id);
            return View(new ActionResultModel<MetaField>(true, string.Empty, metaObject));

        }

        public IActionResult UpdateLogic(MetaField metaObject)
        {
            if (metaObject.Id == 0)
            {
                return View("Update", new ActionResultModel<MetaField>(false, "MetaField Id Can Not Be Null！", metaObject));
            }
            if (string.IsNullOrEmpty(metaObject.Name))
            {
                return View("Update", new ActionResultModel<MetaField>(false, "MetaField Name Can Not Be Null！", metaObject));
            }
            if (string.IsNullOrEmpty(metaObject.Code))
            {
                return View("Update", new ActionResultModel<MetaField>(false, "MetaField Code Can Not Be Null！", metaObject));
            }
            if (_metaFieldRepository.Exist(t => t.Name.Equals(metaObject.Name) && t.Id != metaObject.Id))
            {
                return View("Add", new ActionResultModel<MetaField>(false, "MetaField Name Has Been Exist！", metaObject));
            }
            MetaField app = _metaFieldRepository.GetEntity(t => t.Id == metaObject.Id);
            if (app != null)
            {
                app.Name = metaObject.Name;
                app.Group = metaObject.Group;
                app.SortNumber = metaObject.SortNumber;
                app.Description = metaObject.Description;
                app.ModifyBy = -1;
                app.ModifyTime = DateTime.Now;
            }
            _metaFieldRepository.Update(t => t.Id == metaObject.Id, app);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            MetaField metaObject = _metaFieldRepository.GetEntity(t => t.Id == id);
            _metaFieldRepository.Delete(t => t.Id == id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            MetaField metaObject = _metaFieldRepository.GetEntity(t => t.Id == id);
            _metaFieldRepository.Recover(t => t.Id == id, metaObject);
            return JsonResultModel.Success("恢复成功");
        }
    }
}