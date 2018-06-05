using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryInterface;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class InterfaceSearchConditionController : Controller
    {
        private readonly IInterfaceSearchConditionRepository _interfaceSearchConditionRepository;
        private readonly IConditionAggregationRepository _conditionAggregationRepository;
        private readonly IMetaFieldRepository _metaFieldRepository;

        public InterfaceSearchConditionController(IMetaFieldRepository metaFieldRepository,IInterfaceSearchConditionRepository interfaceSearchConditionRepository, IConditionAggregationRepository conditionAggregationRepository)
        {
            this._interfaceSearchConditionRepository = interfaceSearchConditionRepository;
            this._conditionAggregationRepository = conditionAggregationRepository;
            this._metaFieldRepository = metaFieldRepository;
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
            return View(_interfaceSearchConditionRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted));
        }

        public IActionResult DeleteList()
        {
            return View(_interfaceSearchConditionRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.Deleted));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(InterfaceSearchCondition entity)
        {
            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Add", new ActionResultModel<InterfaceSearchCondition>(false, "Interface Name Can Not Be Null！", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Add", new ActionResultModel<InterfaceSearchCondition>(false, "Interface Code Can Not Be Null！", entity));
            }
            InterfaceSearchCondition obj = _interfaceSearchConditionRepository.GetEntity(t => (t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(entity.Name)) || (t.MetaObjectId == CurrentMetaObjectId && t.Code.Equals(entity.Code)));
            if (obj != null)
            {
                if (obj.Code.Equals(entity.Code))
                {
                    return View("Add", new ActionResultModel<InterfaceSearchCondition>(false, "Interface Code Has Been Exist！", entity));
                }
                if (obj.Name.Equals(entity.Name))
                {
                    return View("Add", new ActionResultModel<InterfaceSearchCondition>(false, "Interface Name Has Been Exist！", entity));
                }
            }
            entity.MetaObjectId = CurrentMetaObjectId;
            _interfaceSearchConditionRepository.Add(entity);
            return RedirectToAction("List");
        }


        public IActionResult Update(int id)
        {
            var metaObject = _interfaceSearchConditionRepository.GetEntity(t => t.Id == id);
            return View(new ActionResultModel<InterfaceSearchCondition>(true, string.Empty, metaObject));

        }

        public IActionResult UpdateLogic(InterfaceSearchCondition entity)
        {
            if (entity.Id == 0)
            {
                return View("Update", new ActionResultModel<InterfaceSearchCondition>(false, "InterfaceSearchCondition Id Can Not Be Null！", entity));
            }
            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Update", new ActionResultModel<InterfaceSearchCondition>(false, "InterfaceSearchCondition Name Can Not Be Null！", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Update", new ActionResultModel<InterfaceSearchCondition>(false, "InterfaceSearchCondition Code Can Not Be Null！", entity));
            }
            if (_interfaceSearchConditionRepository.Exist(t => t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(entity.Name) && t.Id != entity.Id))
            {
                return View("Add", new ActionResultModel<InterfaceSearchCondition>(false, "InterfaceSearchCondition Name Has Been Exist！", entity));
            }
            InterfaceSearchCondition myfield = _interfaceSearchConditionRepository.GetEntity(t => t.Id == entity.Id);
            if (myfield != null)
            {
                myfield.Name = entity.Name;
                myfield.Group = entity.Group;
                myfield.SortNumber = entity.SortNumber;
                myfield.Description = entity.Description;
                myfield.ModifyBy = -1;
                myfield.ModifyTime = DateTime.Now;
            }
            _interfaceSearchConditionRepository.Update(t => t.Id == entity.Id, myfield);
            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            _interfaceSearchConditionRepository.Delete(t => t.Id == id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            InterfaceSearchCondition entity = _interfaceSearchConditionRepository.GetEntity(t => t.Id == id);
            _interfaceSearchConditionRepository.Recover(t => t.Id == id, entity);
            return JsonResultModel.Success("恢复成功");
        }

        public IActionResult AggregationCondition(int id)
        {            
            ViewData["AggregationConditions"] = _conditionAggregationRepository.GetList(t=>t.InterfaceSearchConditionId==id);
            ViewData["MetaFields"] = _metaFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId);
            ViewData["InterfaceSearchConditionId"] = id;
            return View();
        }
    }
}