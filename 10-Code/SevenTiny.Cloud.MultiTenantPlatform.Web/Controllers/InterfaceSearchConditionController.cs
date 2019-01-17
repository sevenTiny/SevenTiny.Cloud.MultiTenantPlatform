//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using SevenTiny.Bantina;
//using SevenTiny.Bantina.Extensions;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;
//using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;

//namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
//{
//    public class InterfaceSearchConditionController : Controller
//    {
//        private readonly IInterfaceSearchConditionRepository _interfaceSearchConditionRepository;
//        private readonly IConditionAggregationRepository _conditionAggregationRepository;
//        private readonly IMetaFieldRepository _metaFieldRepository;

//        public InterfaceSearchConditionController(IMetaFieldRepository metaFieldRepository, IInterfaceSearchConditionRepository interfaceSearchConditionRepository, IConditionAggregationRepository conditionAggregationRepository)
//        {
//            this._interfaceSearchConditionRepository = interfaceSearchConditionRepository;
//            this._conditionAggregationRepository = conditionAggregationRepository;
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
//            return View(_interfaceSearchConditionRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.NotDeleted));
//        }

//        public IActionResult DeleteList()
//        {
//            return View(_interfaceSearchConditionRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId && t.IsDeleted == (int)IsDeleted.Deleted));
//        }

//        public IActionResult Add()
//        {
//            return View();
//        }

//        public IActionResult AddLogic(InterfaceSearchCondition entity)
//        {
//            if (string.IsNullOrEmpty(entity.Name))
//            {
//                return View("Add", new ActionResultModel<InterfaceSearchCondition>(false, "Interface Name Can Not Be Null！", entity));
//            }
//            if (string.IsNullOrEmpty(entity.Code))
//            {
//                return View("Add", new ActionResultModel<InterfaceSearchCondition>(false, "Interface Code Can Not Be Null！", entity));
//            }
//            InterfaceSearchCondition obj = _interfaceSearchConditionRepository.GetEntity(t => (t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(entity.Name)) || (t.MetaObjectId == CurrentMetaObjectId && t.Code.Equals(entity.Code)));
//            if (obj != null)
//            {
//                if (obj.Code.Equals(entity.Code))
//                {
//                    return View("Add", new ActionResultModel<InterfaceSearchCondition>(false, "Interface Code Has Been Exist！", entity));
//                }
//                if (obj.Name.Equals(entity.Name))
//                {
//                    return View("Add", new ActionResultModel<InterfaceSearchCondition>(false, "Interface Name Has Been Exist！", entity));
//                }
//            }
//            entity.MetaObjectId = CurrentMetaObjectId;
//            _interfaceSearchConditionRepository.Add(entity);
//            return RedirectToAction("List");
//        }

//        public IActionResult Update(int id)
//        {
//            var metaObject = _interfaceSearchConditionRepository.GetEntity(t => t.Id == id);
//            return View(new ActionResultModel<InterfaceSearchCondition>(true, string.Empty, metaObject));

//        }

//        public IActionResult UpdateLogic(InterfaceSearchCondition entity)
//        {
//            if (entity.Id == 0)
//            {
//                return View("Update", new ActionResultModel<InterfaceSearchCondition>(false, "InterfaceSearchCondition Id Can Not Be Null！", entity));
//            }
//            if (string.IsNullOrEmpty(entity.Name))
//            {
//                return View("Update", new ActionResultModel<InterfaceSearchCondition>(false, "InterfaceSearchCondition Name Can Not Be Null！", entity));
//            }
//            if (string.IsNullOrEmpty(entity.Code))
//            {
//                return View("Update", new ActionResultModel<InterfaceSearchCondition>(false, "InterfaceSearchCondition Code Can Not Be Null！", entity));
//            }
//            if (_interfaceSearchConditionRepository.Exist(t => t.MetaObjectId == CurrentMetaObjectId && t.Name.Equals(entity.Name) && t.Id != entity.Id))
//            {
//                return View("Add", new ActionResultModel<InterfaceSearchCondition>(false, "InterfaceSearchCondition Name Has Been Exist！", entity));
//            }
//            InterfaceSearchCondition myfield = _interfaceSearchConditionRepository.GetEntity(t => t.Id == entity.Id);
//            if (myfield != null)
//            {
//                myfield.Name = entity.Name;
//                myfield.Group = entity.Group;
//                myfield.SortNumber = entity.SortNumber;
//                myfield.Description = entity.Description;
//                myfield.ModifyBy = -1;
//                myfield.ModifyTime = DateTime.Now;
//            }
//            _interfaceSearchConditionRepository.Update(t => t.Id == entity.Id, myfield);
//            return RedirectToAction("List");
//        }

//        public IActionResult Delete(int id)
//        {
//            _interfaceSearchConditionRepository.Delete(t => t.Id == id);
//            return JsonResultModel.Success("删除成功");
//        }

//        public IActionResult Recover(int id)
//        {
//            InterfaceSearchCondition entity = _interfaceSearchConditionRepository.GetEntity(t => t.Id == id);
//            _interfaceSearchConditionRepository.Recover(t => t.Id == id, entity);
//            return JsonResultModel.Success("恢复成功");
//        }

//        public IActionResult AggregationCondition(int id)
//        {
//            ViewData["AggregationConditions"] = _conditionAggregationRepository.GetList(t => t.InterfaceSearchConditionId == id);
//            ViewData["MetaFields"] = _metaFieldRepository.GetList(t => t.MetaObjectId == CurrentMetaObjectId);
//            ViewData["InterfaceSearchConditionId"] = id;
//            return View();
//        }

//        /// <summary>
//        /// 添加组织条件树
//        /// </summary>
//        /// <param name="id">搜索条件id，指明该条件组织树是属于那个搜索条件的</param>
//        /// <returns></returns>
//        public IActionResult AggregateConditionAddLogic(int id)
//        {
//            int brotherNodeId = int.Parse(Request.Form["brotherNodeId"]);
//            int conditionJointTypeId = int.Parse(Request.Form["conditionJointTypeId"]);
//            int fieldId = int.Parse(Request.Form["fieldId"]);
//            int conditionTypeId = int.Parse(Request.Form["conditionTypeId"]);
//            int conditionValueTypeId = int.Parse(Request.Form["conditionValueTypeId"]);
//            string conditionValue = Request.Form["conditionValue"];

//            return TransactionHelper.Transaction(() =>
//            {
//                int parentId = brotherNodeId;
//                //如果兄弟节点!=-1，说明当前树有值。反之，则构建新树
//                if (parentId != -1)
//                {
//                    //判断是否有树存在
//                    List<ConditionAggregation> conditionListExist = _conditionAggregationRepository.GetList(t => t.InterfaceSearchConditionId == id);
//                    //查看当前兄弟节点的父节点id
//                    ConditionAggregation brotherCondition = conditionListExist.FirstOrDefault(t => t.Id == brotherNodeId);
//                    parentId = brotherCondition.ParentId;
//                    //拿到父节点的信息
//                    ConditionAggregation parentCondition = conditionListExist.FirstOrDefault(t => t.Id == brotherCondition.ParentId);
//                    //如果父节点的连接条件和当前新建的条件一致，则不需要新建条件节点，直接附加在已有的条件下面
//                    if (parentCondition == null || parentCondition.ConditionType != conditionJointTypeId)
//                    {
//                        //先添加一个父节点，然后把兄弟节点的父节点指向新建的父节点
//                        string tempKey = DateTime.Now.ToString("yyyyMMddhhmmss");
//                        ConditionAggregation newParentCondition = new ConditionAggregation
//                        {
//                            InterfaceSearchConditionId = id,
//                            ParentId = conditionListExist.Count > 0 ? parentId : -1,//如果有树，则插入节点的父节点为刚才的兄弟节点的父节点，否则，插入-1作为根节点
//                            FieldId = -1,//连接节点没有field
//                            FieldCode = "-1",
//                            FieldName = tempKey,
//                            ConditionType = conditionJointTypeId,
//                            Name = EnumsTranslaterUseInProgram.Tran_ConditionJoint(conditionJointTypeId),
//                            Value = "-1",
//                            ValueType = -1
//                        };
//                        _conditionAggregationRepository.Add(newParentCondition);
//                        //查询刚才插入的节点
//                        newParentCondition = _conditionAggregationRepository.GetEntity(t => t.FieldName.Contains(tempKey));

//                        //将兄弟节点的父节点指向新插入的节点
//                        brotherCondition.ParentId = newParentCondition.Id;
//                        _conditionAggregationRepository.Update(t => t.Id == brotherCondition.Id, brotherCondition);

//                        //重新赋值parentId
//                        parentId = newParentCondition.Id;
//                    }
//                }
//                //检验是否没有条件节点
//                if (parentId == -1)
//                {
//                    if (_conditionAggregationRepository.Exist(t => t.Id == parentId))
//                    {
//                        return JsonResultModel.Error("已经存在条件节点，请查证后操作！");
//                    }
//                }
//                //新增节点
//                MetaField metaField = _metaFieldRepository.GetEntity(t => t.Id == fieldId);
//                ConditionAggregation newCondition = new ConditionAggregation
//                {
//                    InterfaceSearchConditionId = id,
//                    ParentId = parentId,
//                    FieldId = fieldId,
//                    FieldName = metaField.Name,
//                    FieldCode = metaField.Code,
//                    ConditionType = conditionTypeId,
//                    Name = $"{metaField.Name} {EnumsTranslaterUseInProgram.Tran_ConditionType(conditionTypeId)} {conditionValue}",
//                    Value = conditionValue,
//                    ValueType = conditionValueTypeId
//                };
//                _conditionAggregationRepository.Add(newCondition);

//                return JsonResultModel.Success("保存成功！");
//            });
//        }

//        /// <summary>
//        /// 删除组织条件树
//        /// </summary>
//        /// <param name="id">节点id</param>
//        /// <param name="interfaceSearchConditionId">搜索条件id</param>
//        /// <returns></returns>
//        public IActionResult AggregateConditionDeleteLogic(int id, int interfaceSearchConditionId)
//        {
//            //将要删除的节点id集合
//            List<int> willBeDeleteIds = new List<int>();

//            List<ConditionAggregation> allConditions = _conditionAggregationRepository.GetList(t => t.InterfaceSearchConditionId == interfaceSearchConditionId);
//            if (allConditions == null || !allConditions.Any())
//            {
//                return JsonResultModel.Success("删除成功！");
//            }
//            ConditionAggregation conditionAggregation = allConditions.FirstOrDefault(t => t.Id == id);
//            if (conditionAggregation == null)
//            {
//                return JsonResultModel.Success("删除成功！");
//            }
//            //获取父节点id
//            int parentId = conditionAggregation.ParentId;
//            //如果是顶级节点，直接删除
//            if (parentId == -1)
//            {
//                DeleteNodeAndChildrenNodes(allConditions, id);
//                return JsonResultModel.Success("删除成功！");
//            }
//            //如果不是顶级节点，查询所有兄弟节点。
//            //如果所有兄弟节点（包含自己）多余两个，则直接删除此节点;
//            List<ConditionAggregation> conditionList = allConditions.Where(t => t.ParentId == parentId).ToList();
//            if (conditionList.Count > 2)
//            {
//                DeleteNodeAndChildrenNodes(allConditions, id);
//                return JsonResultModel.Success("删除成功！");
//            }
//            //如果兄弟节点为两个，则将父亲节点删除，将另一个兄弟节点作为父节点。
//            else if (conditionList.Count == 2)
//            {
//                //获取到父节点
//                ConditionAggregation parentNode = allConditions.FirstOrDefault(t => t.Id == parentId);
//                //找到兄弟节点（同一个父节点，id!=当前节点）
//                ConditionAggregation brotherNode = allConditions.FirstOrDefault(t => t.ParentId == parentId && t.Id != id);
//                //将兄弟节点的父节点指向父节点的父节点
//                brotherNode.ParentId = parentNode.ParentId;
//                //更新兄弟节点
//                _conditionAggregationRepository.Update(t => t.Id == brotherNode.Id, brotherNode);
//                //将父节点删除
//                _conditionAggregationRepository.Delete(t => t.Id == parentId);
//                //删除要删除的节点以及下级节点
//                DeleteNodeAndChildrenNodes(allConditions, id);
//            }
//            //如果没有兄弟节点，则直接将节点以及父节点都删除（如果数据不出问题，默认不存在此种情况，直接返回结果）
//            else
//            {
//                return JsonResultModel.Success("删除成功！");
//            }

//            return JsonResultModel.Success("删除成功！");

//            //删除节点级所有下级节点
//            void DeleteNodeAndChildrenNodes(List<ConditionAggregation> sourceConditions, int nodeId)
//            {
//                FindDeleteNodeAndChildrenNodes(sourceConditions, nodeId);
//                Expression<Func<ConditionAggregation, bool>> func = t => t.Id == id;
//                foreach (var item in willBeDeleteIds)
//                {
//                    func = func.Or(tt => tt.Id == item);
//                }
//                _conditionAggregationRepository.Delete(func);
//            }

//            //遍历整棵树，找到要删除的节点以及下级节点
//            void FindDeleteNodeAndChildrenNodes(List<ConditionAggregation> sourceConditions, int nodeId)
//            {
//                var children = sourceConditions.Where(t => t.ParentId == nodeId).ToList();
//                if (children != null && children.Any())
//                {
//                    foreach (var item in children)
//                    {
//                        willBeDeleteIds.Add(item.Id);
//                        FindDeleteNodeAndChildrenNodes(children, item.Id);
//                    }
//                }
//            }
//        }

//        [HttpGet]
//        public IActionResult AggregateConditionTreeView(int id)
//        {
//            List<ConditionAggregation> conditions = _conditionAggregationRepository.GetList(t => t.InterfaceSearchConditionId == id);

//            ConditionAggregation condition = conditions.FirstOrDefault(t => t.ParentId == -1);
//            if (condition != null)
//            {
//                condition.Children = GetTree(conditions, condition.Id);
//            }

//            //Tree Search
//            List<ConditionAggregation> GetTree(List<ConditionAggregation> source, int parentId)
//            {
//                var childs = source.Where(t => t.ParentId == parentId).ToList();
//                if (childs == null)
//                {
//                    return new List<ConditionAggregation>();
//                }
//                else
//                {
//                    childs.ForEach(t => t.Children = GetTree(source, t.Id));
//                }
//                return childs;
//            }

//            if (condition != null)
//            {
//                return JsonResultModel.Success("构造树成功！", new List<ConditionAggregation> { condition });
//            }
//            else
//            {
//                return JsonResultModel.Success("构造树成功！", new List<ConditionAggregation>());
//            }
//        }

//    }
//}