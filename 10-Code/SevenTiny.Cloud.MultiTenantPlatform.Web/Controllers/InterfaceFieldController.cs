//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using SevenTiny.Bantina;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;
//using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
//{
//    public class InterfaceFieldController : Controller
//    {
//        private readonly IInterfaceFieldRepository _interfaceFieldRepository;
//        private readonly IFieldAggregationRepository _fieldAggregationRepository;
//        private readonly IMetaFieldRepository _metaFieldRepository;

//        public InterfaceFieldController(IInterfaceFieldRepository interfaceFieldRepository, IFieldAggregationRepository fieldAggregationRepository, IMetaFieldRepository metaFieldRepository)
//        {
//            this._interfaceFieldRepository = interfaceFieldRepository;
//            this._fieldAggregationRepository = fieldAggregationRepository;
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
//            return View(_interfaceFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted));
//        }

//        public IActionResult DeleteList()
//        {
//            return View(_interfaceFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.Deleted));
//        }

//        public IActionResult Add()
//        {
//            return View();
//        }

//        public IActionResult AddLogic(InterfaceField entity)
//        {
//            if (string.IsNullOrEmpty(entity.Name))
//            {
//                return View("Add", new ActionResultModel<InterfaceField>(false, "Interface Name Can Not Be Null！", entity));
//            }
//            if (string.IsNullOrEmpty(entity.Code))
//            {
//                return View("Add", new ActionResultModel<InterfaceField>(false, "Interface Code Can Not Be Null！", entity));
//            }
//            InterfaceField obj = _interfaceFieldRepository.GetEntity(t => (t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(entity.Name)) || (t.MetaObjectId == CurrentMetaObjectId && t.Code.Equals(entity.Code)));
//            if (obj != null)
//            {
//                if (obj.Code.Equals(entity.Code))
//                {
//                    return View("Add", new ActionResultModel<InterfaceField>(false, "Interface Code Has Been Exist！", entity));
//                }
//                if (obj.Name.Equals(entity.Name))
//                {
//                    return View("Add", new ActionResultModel<InterfaceField>(false, "Interface Name Has Been Exist！", entity));
//                }
//            }
//            entity.MetaObjectId = CurrentMetaObjectId;
//            _interfaceFieldRepository.Add(entity);
//            return RedirectToAction("List");
//        }

//        public IActionResult Update(int id)
//        {
//            var interfaceField = _interfaceFieldRepository.GetEntity(t => t.Id == id);
//            return View(new ActionResultModel<InterfaceField>(true, string.Empty, interfaceField));

//        }

//        public IActionResult UpdateLogic(InterfaceField entity)
//        {
//            if (entity.Id == 0)
//            {
//                return View("Update", new ActionResultModel<InterfaceField>(false, "InterfaceField Id Can Not Be Null！", entity));
//            }
//            if (string.IsNullOrEmpty(entity.Name))
//            {
//                return View("Update", new ActionResultModel<InterfaceField>(false, "InterfaceField Name Can Not Be Null！", entity));
//            }
//            if (string.IsNullOrEmpty(entity.Code))
//            {
//                return View("Update", new ActionResultModel<InterfaceField>(false, "InterfaceField Code Can Not Be Null！", entity));
//            }
//            if (_interfaceFieldRepository.Exist(t => t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(entity.Name) && t.Id != entity.Id))
//            {
//                return View("Add", new ActionResultModel<InterfaceField>(false, "InterfaceField Name Has Been Exist！", entity));
//            }
//            InterfaceField myfield = _interfaceFieldRepository.GetEntity(t => t.Id == entity.Id);
//            if (myfield != null)
//            {
//                myfield.Name = entity.Name;
//                myfield.Group = entity.Group;
//                myfield.SortNumber = entity.SortNumber;
//                myfield.Description = entity.Description;
//                myfield.ModifyBy = -1;
//                myfield.ModifyTime = DateTime.Now;
//            }
//            _interfaceFieldRepository.Update(t => t.Id == entity.Id, myfield);
//            return RedirectToAction("List");
//        }

//        public IActionResult Delete(int id)
//        {
//            TransactionHelper.Transaction(() =>
//            {
//                //clear fields first
//                _fieldAggregationRepository.Delete(t => t.InterfaceFieldId == id);
//                //delete field config
//                _interfaceFieldRepository.Delete(t => t.Id == id);
//            });
//            return JsonResultModel.Success("删除成功");
//        }

//        public IActionResult LogicDelete(int id)
//        {
//            InterfaceField entity = _interfaceFieldRepository.GetEntity(t => t.Id == id);
//            _interfaceFieldRepository.LogicDelete(t => t.Id == id, entity);
//            return JsonResultModel.Success("删除成功");
//        }

//        public IActionResult Recover(int id)
//        {
//            InterfaceField entity = _interfaceFieldRepository.GetEntity(t => t.Id == id);
//            _interfaceFieldRepository.Recover(t => t.Id == id, entity);
//            return JsonResultModel.Success("恢复成功");
//        }

//        /// <summary>
//        /// 组织字段
//        /// </summary>
//        /// <param name="id">字段配置对象id</param>
//        /// <returns></returns>
//        public IActionResult AggregateField(int id)
//        {
//            //get metafield ids
//            var aggregateMetaFields = _fieldAggregationRepository.GetList(t => t.InterfaceFieldId == id)?.Select(t => t.MetaFieldId)?.ToList();
//            var metaFields = _metaFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId);
//            var aggregateFields = metaFields.Where(t => aggregateMetaFields.Any(n => n == t.Id)).ToList();//get aggregateFields which metafield.id in aggregateMetaFields
//            aggregateMetaFields.ForEach(t => metaFields.RemoveAll(n => n.Id == t));//remove metafield aggregateField exist.
//            ViewData["AggregateFields"] = aggregateFields;
//            ViewData["MetaFields"] = metaFields;
//            ViewData["InterfaceFieldId"] = id;
//            return View();
//        }
//        /// <summary>
//        /// 组织字段添加逻辑
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public IActionResult AggregateFieldAddLogic(int id)
//        {
//            FieldAggregation fieldAggregation = new FieldAggregation { InterfaceFieldId = id };
//            string metaFieldIdsString = Request.Form["metaFieldIds"];
//            //get metafield ids
//            int[] metaFieldIds = metaFieldIdsString.Split(',').Select(t => Convert.ToInt32(t)).ToArray();
//            int[] fieldAggregationIds = _fieldAggregationRepository.GetList(t => t.InterfaceFieldId == id).Select(t => t.MetaFieldId).ToArray();
//            IEnumerable<int> addIds = metaFieldIds.Except(fieldAggregationIds); //ids will add
//            IEnumerable<int> deleteIds = fieldAggregationIds.Except(metaFieldIds);  //ids will delete
//            foreach (var item in addIds)
//            {
//                _fieldAggregationRepository.Add(new FieldAggregation { InterfaceFieldId = id, MetaFieldId = item });
//            }
//            foreach (var item in deleteIds)
//            {
//                _fieldAggregationRepository.Delete(t => t.MetaFieldId == item);
//            }
//            return JsonResultModel.Success("保存成功！");
//        }
//    }
//}