using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class SearchConditionController : WebControllerBase
    {
        ISearchConditionService _searchConditionService;
        ISearchConditionNodeService _searchConditionNodeService;
        IMetaFieldService _metaFieldService;

        public SearchConditionController(IMetaFieldService metaFieldService, ISearchConditionNodeService searchConditionNodeService, ISearchConditionService searchConditionService, ISearchConditionNodeService _conditionAggregationService)
        {
            _metaFieldService = metaFieldService;
            _searchConditionService = searchConditionService;
            _searchConditionNodeService = searchConditionNodeService;
        }

        public IActionResult List()
        {
            return View(_searchConditionService.GetListUnDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult SearchItemList(Guid id)
        {
            return View(_searchConditionNodeService.GetListBySearchConditionId(id));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(SearchCondition entity)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                .ContinueAssert(_ => entity.Code.IsAlnum(2, 50), "编码不合法，2-50位且只能包含字母和数字（字母开头）")
                .Continue(_ =>
                {
                    entity.MetaObjectId = CurrentMetaObjectId;
                    //组合编码
                    entity.Code = $"{CurrentMetaObjectCode}.SearchCondition.{entity.Code}";
                    entity.CreateBy = CurrentUserId;
                    return _searchConditionService.Add(entity);
                });

            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult Update(Guid id)
        {
            var entity = _searchConditionService.GetById(id);
            return View(ResponseModel.Success(entity));
        }

        public IActionResult UpdateLogic(SearchCondition entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _searchConditionService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult SearchItemUpdate(Guid id)
        {
            var entity = _searchConditionNodeService.GetById(id);
            return View(ResponseModel.Success(entity));
        }

        public IActionResult SearchItemUpdateLogic(SearchConditionNode entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;

                   if (string.IsNullOrEmpty(entity.Text))
                       entity.Text = entity.Name;

                   return _searchConditionNodeService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("SearchItemUpdate", result.ToResponseModel());

            return View("SearchItemUpdate", ResponseModel.Success(1, "修改成功"));
        }

        public IActionResult Delete(Guid id)
        {
            return _searchConditionService.Delete(id).ToJsonResultModel();
        }

        public IActionResult AggregationCondition(Guid id)
        {
            ViewData["AggregationConditions"] = _searchConditionNodeService.GetListBySearchConditionId(id);
            ViewData["MetaFields"] = _metaFieldService.GetListUnDeletedByMetaObjectId(CurrentMetaObjectId);
            ViewData["searchConditionId"] = id;
            return View();
        }

        /// <summary>
        /// 添加组织条件树
        /// </summary>
        /// <param name="id">搜索条件id，指明该条件组织树是属于那个搜索条件的</param>
        /// <returns></returns>
        public IActionResult AggregateConditionAddLogic(Guid id)
        {
            var brotherNodeId = Guid.Parse(Request.Form["brotherNodeId"]);
            var conditionJointTypeId = int.Parse(Request.Form["conditionJointTypeId"]);
            var fieldId = Guid.Parse(Request.Form["fieldId"]);
            var conditionTypeId = int.Parse(Request.Form["conditionTypeId"]);
            var conditionValueTypeId = int.Parse(Request.Form["conditionValueTypeId"]);
            string conditionValue = Request.Form["conditionValue"];

            var result = _searchConditionNodeService.AggregateCondition(id, brotherNodeId, conditionJointTypeId, fieldId, conditionTypeId, conditionValue, conditionValueTypeId);
            return result.ToJsonResultModel();
        }

        /// <summary>
        /// 删除组织条件树
        /// </summary>
        /// <param name="id">节点id</param>
        /// <param name="searchConditionId">搜索条件id</param>
        /// <returns></returns>
        public IActionResult AggregateConditionDeleteLogic(Guid id, Guid searchConditionId)
        {
            //将要删除的节点id集合
            return _searchConditionNodeService.DeleteAggregateCondition(id, searchConditionId).ToJsonResultModel();
        }

        [HttpGet]
        public IActionResult AggregateConditionTreeView(Guid id)
        {
            List<SearchConditionNode> conditions = _searchConditionNodeService.GetListBySearchConditionId(id);

            SearchConditionNode condition = conditions?.FirstOrDefault(t => t.ParentId == Guid.Empty);
            if (condition != null)
            {
                condition.Children = GetTree(conditions, condition.Id);
            }

            //Tree Search
            List<SearchConditionNode> GetTree(List<SearchConditionNode> source, Guid parentId)
            {
                var childs = source.Where(t => t.ParentId == parentId).ToList();
                if (childs == null)
                {
                    return new List<SearchConditionNode>();
                }
                else
                {
                    childs.ForEach(t => t.Children = GetTree(source, t.Id));
                }
                return childs;
            }

            if (condition != null)
            {
                return JsonResultModel.Success("构造树成功！", new List<SearchConditionNode> { condition });
            }
            else
            {
                return JsonResultModel.Success("构造树成功！", new List<SearchConditionNode>());
            }
        }
    }
}