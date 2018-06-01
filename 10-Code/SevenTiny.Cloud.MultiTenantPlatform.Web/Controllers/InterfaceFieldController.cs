using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryInterface;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class InterfaceFieldController : Controller
    {
        private readonly IInterfaceFieldRepository _interfaceFieldRepository;

        public InterfaceFieldController(IInterfaceFieldRepository interfaceFieldRepository)
        {
            this._interfaceFieldRepository = interfaceFieldRepository;
        }

        private int CurrentMetaObjectId
        {
            get
            {
                int metaObjectId = HttpContext.Session.GetInt32("MetaObjectId") ?? default(int);
                if (metaObjectId == default(int))
                {
                    throw new ArgumentNullException("MetaObjectId is null,please select MetaObject first!");
                }
                return metaObjectId;
            }
        }

        public IActionResult List()
        {
            return View(_interfaceFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted));
        }

        public IActionResult DeleteList()
        {
            return View(_interfaceFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.Deleted));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(InterfaceField entity)
        {
            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Add", new ActionResultModel<InterfaceField>(false, "Interface Name Can Not Be Null！", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Add", new ActionResultModel<InterfaceField>(false, "Interface Code Can Not Be Null！", entity));
            }
            InterfaceField obj = _interfaceFieldRepository.GetEntity(t => (t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(entity.Name)) || (t.MetaObjectId == CurrentMetaObjectId && t.Code.Equals(entity.Code)));
            if (obj != null)
            {
                if (obj.Code.Equals(entity.Code))
                {
                    return View("Add", new ActionResultModel<InterfaceField>(false, "Interface Code Has Been Exist！", entity));
                }
                if (obj.Name.Equals(entity.Name))
                {
                    return View("Add", new ActionResultModel<InterfaceField>(false, "Interface Name Has Been Exist！", entity));
                }
            }
            entity.MetaObjectId = CurrentMetaObjectId;
            _interfaceFieldRepository.Add(entity);
            return RedirectToAction("List");
        }


        public IActionResult Update(int id)
        {
            var metaObject = _interfaceFieldRepository.GetEntity(t => t.Id == id);
            return View(new ActionResultModel<InterfaceField>(true, string.Empty, metaObject));

        }

        public IActionResult UpdateLogic(InterfaceField entity)
        {
            if (entity.Id == 0)
            {
                return View("Update", new ActionResultModel<InterfaceField>(false, "InterfaceField Id Can Not Be Null！", entity));
            }
            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Update", new ActionResultModel<InterfaceField>(false, "InterfaceField Name Can Not Be Null！", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Update", new ActionResultModel<InterfaceField>(false, "InterfaceField Code Can Not Be Null！", entity));
            }
            if (_interfaceFieldRepository.Exist(t => t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(entity.Name) && t.Id != entity.Id))
            {
                return View("Add", new ActionResultModel<InterfaceField>(false, "InterfaceField Name Has Been Exist！", entity));
            }
            InterfaceField myfield = _interfaceFieldRepository.GetEntity(t => t.Id == entity.Id);
            if (myfield != null)
            {
                myfield.Name = entity.Name;
                myfield.Group = entity.Group;
                myfield.SortNumber = entity.SortNumber;
                myfield.Description = entity.Description;
                myfield.ModifyBy = -1;
                myfield.ModifyTime = DateTime.Now;
            }
            _interfaceFieldRepository.Update(t => t.Id == entity.Id, myfield);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            _interfaceFieldRepository.Delete(t => t.Id == id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            InterfaceField entity = _interfaceFieldRepository.GetEntity(t => t.Id == id);
            _interfaceFieldRepository.Recover(t => t.Id == id, entity);
            return JsonResultModel.Success("恢复成功");
        }
    }
}