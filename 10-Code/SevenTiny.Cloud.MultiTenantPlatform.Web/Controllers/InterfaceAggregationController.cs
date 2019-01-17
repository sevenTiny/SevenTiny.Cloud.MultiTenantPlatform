//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;
//using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
//using System;

//namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
//{
//    public class InterfaceAggregationController : Controller
//    {
//        private readonly IInterfaceAggregationRepository _interfaceAggregationRepository;
//        private readonly IInterfaceFieldRepository _interfaceFieldRepository;
//        private readonly IInterfaceSearchConditionRepository _interfaceSearchConditionRepository;

//        public InterfaceAggregationController(IInterfaceSearchConditionRepository interfaceSearchConditionRepository, IInterfaceFieldRepository interfaceFieldRepository, IInterfaceAggregationRepository interfaceAggregationRepository)
//        {
//            this._interfaceAggregationRepository = interfaceAggregationRepository;
//            this._interfaceFieldRepository = interfaceFieldRepository;
//            this._interfaceSearchConditionRepository = interfaceSearchConditionRepository;
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
//            return View(_interfaceAggregationRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted));
//        }

//        public IActionResult DeleteList()
//        {
//            return View(_interfaceAggregationRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.Deleted));
//        }

//        public IActionResult Add()
//        {
//            ViewData["InterfaceFields"] = _interfaceFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted);
//            ViewData["SearchConditions"] = _interfaceSearchConditionRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted);
//            return View();
//        }

//        public IActionResult AddLogic(InterfaceAggregation entity)
//        {
//            ViewData["InterfaceFields"] = _interfaceFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted);
//            ViewData["SearchConditions"] = _interfaceSearchConditionRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted);

//            if (string.IsNullOrEmpty(entity.Name))
//            {
//                return View("Add", new ActionResultModel<InterfaceAggregation>(false, "Interface Name Can Not Be Null！", entity));
//            }
//            if (string.IsNullOrEmpty(entity.Code))
//            {
//                return View("Add", new ActionResultModel<InterfaceAggregation>(false, "Interface Code Can Not Be Null！", entity));
//            }
//            InterfaceAggregation obj = _interfaceAggregationRepository.GetEntity(t => (t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(entity.Name)) || (t.MetaObjectId == CurrentMetaObjectId && t.Code.Equals(entity.Code)));
//            if (obj != null)
//            {
//                if (obj.Code.Equals(entity.Code))
//                {
//                    return View("Add", new ActionResultModel<InterfaceAggregation>(false, "Interface Code Has Been Exist！", entity));
//                }
//                if (obj.Name.Equals(entity.Name))
//                {
//                    return View("Add", new ActionResultModel<InterfaceAggregation>(false, "Interface Name Has Been Exist！", entity));
//                }
//            }
//            if (entity.InterfaceFieldId == default(int))
//            {
//                return View("Add", new ActionResultModel<InterfaceAggregation>(false, "InterfaceField Can Not Be Null！", entity));
//            }
//            if (entity.InterfaceSearchConditionId == default(int))
//            {
//                return View("Add", new ActionResultModel<InterfaceAggregation>(false, "InterfaceField Condition Not Be Null！", entity));
//            }
//            //查询并将名字赋予接口的字段
//            var interfaceField = _interfaceFieldRepository.GetEntity(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted && t.Id == entity.InterfaceFieldId);
//            var interfaceSearchCondition = _interfaceSearchConditionRepository.GetEntity(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted && t.Id == entity.InterfaceSearchConditionId);
//            entity.InterfaceFieldName = interfaceField.Name;
//            entity.InterfaceSearchConditionName = interfaceSearchCondition.Name;
//            entity.MetaObjectId = CurrentMetaObjectId;
//            _interfaceAggregationRepository.Add(entity);
//            return RedirectToAction("List");
//        }

//        public IActionResult Update(int id)
//        {
//            ViewData["InterfaceFields"] = _interfaceFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted);
//            ViewData["SearchConditions"] = _interfaceSearchConditionRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted);
//            var metaObject = _interfaceAggregationRepository.GetEntity(t => t.Id == id);
//            return View(new ActionResultModel<InterfaceAggregation>(true, string.Empty, metaObject));

//        }

//        public IActionResult UpdateLogic(InterfaceAggregation entity)
//        {
//            ViewData["InterfaceFields"] = _interfaceFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted);
//            ViewData["SearchConditions"] = _interfaceSearchConditionRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted);

//            if (entity.Id == 0)
//            {
//                return View("Update", new ActionResultModel<InterfaceAggregation>(false, "InterfaceAggregation Id Can Not Be Null！", entity));
//            }
//            if (string.IsNullOrEmpty(entity.Name))
//            {
//                return View("Update", new ActionResultModel<InterfaceAggregation>(false, "InterfaceAggregation Name Can Not Be Null！", entity));
//            }
//            if (string.IsNullOrEmpty(entity.Code))
//            {
//                return View("Update", new ActionResultModel<InterfaceAggregation>(false, "InterfaceAggregation Code Can Not Be Null！", entity));
//            }
//            if (_interfaceAggregationRepository.Exist(t => t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(entity.Name) && t.Id != entity.Id))
//            {
//                return View("Add", new ActionResultModel<InterfaceAggregation>(false, "InterfaceAggregation Name Has Been Exist！", entity));
//            }
//            if (entity.InterfaceFieldId == default(int))
//            {
//                return View("Add", new ActionResultModel<InterfaceAggregation>(false, "InterfaceField Can Not Be Null！", entity));
//            }
//            if (entity.InterfaceSearchConditionId == default(int))
//            {
//                return View("Add", new ActionResultModel<InterfaceAggregation>(false, "InterfaceField Condition Not Be Null！", entity));
//            }

//            InterfaceAggregation myEntity = _interfaceAggregationRepository.GetEntity(t => t.Id == entity.Id);
//            //查询并将名字赋予接口的字段
//            if (myEntity != null)
//            {
//                var interfaceField = _interfaceFieldRepository.GetEntity(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted && t.Id == entity.InterfaceFieldId);
//                var interfaceSearchCondition = _interfaceSearchConditionRepository.GetEntity(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted && t.Id == entity.InterfaceSearchConditionId);
//                myEntity.InterfaceFieldName = interfaceField.Name;
//                myEntity.InterfaceSearchConditionName = interfaceSearchCondition.Name;
//                myEntity.Name = entity.Name;
//                myEntity.Group = entity.Group;
//                myEntity.SortNumber = entity.SortNumber;
//                myEntity.Description = entity.Description;
//                myEntity.ModifyBy = -1;
//                myEntity.ModifyTime = DateTime.Now;
//            }
//            _interfaceAggregationRepository.Update(t => t.Id == entity.Id, myEntity);
//            return RedirectToAction("List");
//        }

//        public IActionResult Delete(int id)
//        {
//            _interfaceAggregationRepository.Delete(t => t.Id == id);
//            return JsonResultModel.Success("删除成功");
//        }

//        public IActionResult LogicDelete(int id)
//        {
//            InterfaceAggregation entity = _interfaceAggregationRepository.GetEntity(t => t.Id == id);
//            _interfaceAggregationRepository.LogicDelete(t => t.Id == id, entity);
//            return JsonResultModel.Success("删除成功");
//        }

//        public IActionResult Recover(int id)
//        {
//            InterfaceAggregation entity = _interfaceAggregationRepository.GetEntity(t => t.Id == id);
//            _interfaceAggregationRepository.Recover(t => t.Id == id, entity);
//            return JsonResultModel.Success("恢复成功");
//        }
//    }
//}