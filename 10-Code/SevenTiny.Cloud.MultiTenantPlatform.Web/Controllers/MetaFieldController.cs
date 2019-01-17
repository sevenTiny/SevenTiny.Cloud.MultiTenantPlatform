//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;
//using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
//using System;

//namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
//{
//    public class MetaFieldController : Controller
//    {
//        private readonly IMetaFieldRepository _metaFieldRepository;

//        public MetaFieldController(IMetaFieldRepository metaFieldRepository)
//        {
//            this._metaFieldRepository = metaFieldRepository;
//        }

//        private int CurrentMetaObjectId
//        {
//            get
//            {
//                int metaObjectId = HttpContext.Session.GetInt32("MetaObjectId") ?? default(int);
//                if (metaObjectId == default(int))
//                {
//                    throw new ArgumentNullException("MetaObjectId is null,please select MetaObject first!");
//                }
//                return metaObjectId;
//            }
//        }

//        public IActionResult List()
//        {
//            return View(_metaFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted));
//        }

//        public IActionResult DeleteList()
//        {
//            return View(_metaFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.Deleted));
//        }

//        public IActionResult Add()
//        {
//            return View();
//        }

//        public IActionResult AddLogic(MetaField metaField)
//        {
//            if (string.IsNullOrEmpty(metaField.Name))
//            {
//                return View("Add", new ActionResultModel<MetaField>(false, "MetaField Name Can Not Be Null！", metaField));
//            }
//            if (string.IsNullOrEmpty(metaField.Code))
//            {
//                return View("Add", new ActionResultModel<MetaField>(false, "MetaField Code Can Not Be Null！", metaField));
//            }
//            MetaField obj = _metaFieldRepository.GetEntity(t => (t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(metaField.Name)) || (t.MetaObjectId == CurrentMetaObjectId && t.Code.Equals(metaField.Code)));
//            if (obj != null)
//            {
//                if (obj.Code.Equals(metaField.Code))
//                {
//                    return View("Add", new ActionResultModel<MetaField>(false, "MetaField Code Has Been Exist！", metaField));
//                }
//                if (obj.Name.Equals(metaField.Name))
//                {
//                    return View("Add", new ActionResultModel<MetaField>(false, "MetaField Name Has Been Exist！", metaField));
//                }
//            }
//            metaField.MetaObjectId = CurrentMetaObjectId;
//            _metaFieldRepository.Add(metaField);
//            return RedirectToAction("List");
//        }


//        public IActionResult Update(int id)
//        {
//            var metaObject = _metaFieldRepository.GetEntity(t => t.Id == id);
//            return View(new ActionResultModel<MetaField>(true, string.Empty, metaObject));

//        }

//        public IActionResult UpdateLogic(MetaField metaField)
//        {
//            if (metaField.Id == 0)
//            {
//                return View("Update", new ActionResultModel<MetaField>(false, "MetaField Id Can Not Be Null！", metaField));
//            }
//            if (string.IsNullOrEmpty(metaField.Name))
//            {
//                return View("Update", new ActionResultModel<MetaField>(false, "MetaField Name Can Not Be Null！", metaField));
//            }
//            if (string.IsNullOrEmpty(metaField.Code))
//            {
//                return View("Update", new ActionResultModel<MetaField>(false, "MetaField Code Can Not Be Null！", metaField));
//            }
//            if (_metaFieldRepository.Exist(t => t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(metaField.Name) && t.Id != metaField.Id))
//            {
//                return View("Add", new ActionResultModel<MetaField>(false, "MetaField Name Has Been Exist！", metaField));
//            }
//            MetaField myfield = _metaFieldRepository.GetEntity(t => t.Id == metaField.Id);
//            if (myfield != null)
//            {
//                myfield.Name = metaField.Name;
//                myfield.Group = metaField.Group;
//                myfield.SortNumber = metaField.SortNumber;
//                myfield.Description = metaField.Description;
//                myfield.FieldType = metaField.FieldType;
//                myfield.DataSourceId = metaField.DataSourceId;
//                myfield.ModifyBy = -1;
//                myfield.ModifyTime = DateTime.Now;
//            }
//            _metaFieldRepository.Update(t => t.Id == metaField.Id, myfield);
//            return RedirectToAction("List");
//        }

//        public IActionResult Delete(int id)
//        {
//            MetaField metaObject = _metaFieldRepository.GetEntity(t => t.Id == id);
//            _metaFieldRepository.Delete(t => t.Id == id);
//            return JsonResultModel.Success("删除成功");
//        }

//        public IActionResult Recover(int id)
//        {
//            MetaField metaObject = _metaFieldRepository.GetEntity(t => t.Id == id);
//            _metaFieldRepository.Recover(t => t.Id == id, metaObject);
//            return JsonResultModel.Success("恢复成功");
//        }
//    }
//}