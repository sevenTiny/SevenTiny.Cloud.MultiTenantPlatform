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
    public class FormController : WebControllerBase
    {
        public FormController(IFormViewService formViewService)
        {
            _formViewService = formViewService;
        }

        readonly IFormViewService _formViewService;

        public IActionResult List()
        {
            return View(_formViewService.GetListUnDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(FormView entity)
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
                   entity.Code = $"{CurrentMetaObjectCode}.FormView.{entity.Code}";
                   entity.CreateBy = CurrentUserId;

                   return _formViewService.Add(entity);
               });


            if (!result.IsSuccess)
                return View("Add", result.ToResponseModel(entity));


            return RedirectToAction("List");
        }

        public IActionResult Update(Guid id)
        {
            var entity = _formViewService.GetById(id);
            return View(ResponseModel.Success(entity));
        }

        public IActionResult UpdateLogic(FormView entity)
        {
            var result = Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.Name, nameof(entity.Name))
               .ContinueAssert(_ => entity.Id != Guid.Empty, "entity Id Can Not Be Null")
               .Continue(_ =>
               {
                   entity.ModifyBy = CurrentUserId;
                   return _formViewService.UpdateWithOutCode(entity);
               });

            if (!result.IsSuccess)
                return View("Update", result.ToResponseModel(entity));

            return RedirectToAction("List");
        }

        public IActionResult LogicDelete(Guid id)
        {
            _formViewService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }

        ///// <summary>
        ///// 组织字段
        ///// </summary>
        ///// <param name="id">字段配置对象id</param>
        ///// <returns></returns>
        //public IActionResult AggregateField(int id)
        //{
        //    //获取组织字段里面本字段配置下的所有字段
        //    var fieldListAggregations = _formMetaFieldService.GetByFormId(id) ?? new List<FormMetaField>();
        //    var aggregateMetaFieldIds = fieldListAggregations.Select(t => t.MetaFieldId).ToArray();
        //    //获取到本对象的所有字段
        //    var metaFields = metaFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
        //    //过滤出已配置过的字段
        //    var aggregateFields = metaFields.Where(t => fieldListAggregations.Any(n => n.MetaFieldId == t.Id))
        //        .Select(t =>
        //        {
        //            var fieldAggregation = fieldListAggregations.FirstOrDefault(f => f.MetaFieldId == t.Id);
        //            return new MetaFieldViewModel
        //            {
        //                Id = t.Id,
        //                Code = t.Code,
        //                Name = t.Name,
        //                FieldAggregationId = fieldAggregation.Id,
        //                SortNumber = fieldAggregation.SortNumber
        //            };
        //        }).OrderBy(t => t.SortNumber).ToList();
        //    //剩下的是未配置的待选字段
        //    var waitingForSelection = metaFields.Where(t => !aggregateMetaFieldIds.Contains(t.Id)).ToList();

        //    //【BUG】下面这种从查询到的内存数据中排除的写法是错误的，会导致内存做的缓存中的数据根据引用地址跟着修改，导致缓存数据被改动，下次查询出错
        //    //如果使用序列化做的缓存应该不会有如下语句出现的缓存问题
        //    //aggregateMetaFields.ForEach(t => metaFields.RemoveAll(n => n.Id == t));//remove metafield aggregateField exist.

        //    ViewData["AggregateFields"] = aggregateFields;
        //    ViewData["MetaFields"] = waitingForSelection;
        //    ViewData["FormId"] = id;
        //    return View();
        //}

        ///// <summary>
        ///// 组织字段添加逻辑
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public IActionResult AggregateFieldAddLogic(int id)
        //{
        //    string metaFieldIdsString = Request.Form["metaFieldIds"];
        //    //get metafield ids
        //    var metaFieldIdsSplit = !string.IsNullOrEmpty(metaFieldIdsString) ? metaFieldIdsString.Split(',') : Array.Empty<string>();

        //    int[] metaFieldIds = (metaFieldIdsSplit != null && metaFieldIdsSplit.Any()) ? metaFieldIdsSplit.Select(t => Convert.ToInt32(t)).ToArray() : new int[0];
        //    int[] fieldAggregationIds = _formMetaFieldService.GetByFormId(id)?.Select(t => t.MetaFieldId)?.ToArray() ?? new int[0];
        //    IEnumerable<int> addIds = metaFieldIds.Except(fieldAggregationIds); //ids will add
        //    IEnumerable<int> deleteIds = fieldAggregationIds.Except(metaFieldIds);  //ids will delete

        //    IList<FormMetaField> formMetaFields = new List<FormMetaField>();
        //    foreach (var item in addIds)
        //    {
        //        formMetaFields.Add(new FormMetaField { FormId = id, MetaFieldId = item });
        //    }

        //    if (formMetaFields.Any())
        //        _formMetaFieldService.Add(CurrentMetaObjectId, formMetaFields);

        //    foreach (var item in deleteIds)
        //    {
        //        _formMetaFieldService.DeleteByMetaFieldId(item);
        //    }

        //    //对当前列表配置的顺序进行重新排序
        //    _formMetaFieldService.SortFields(id, metaFieldIds);

        //    return JsonResultModel.Success("保存成功！");
        //}

        ///// <summary>
        ///// 列表字段配置编辑页面
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public IActionResult FormMetaFieldUpdate(int id)
        //{
        //    var formMetaField = _formMetaFieldService.GetById(id);
        //    return View(ResponseModel.Success(formMetaField));
        //}
        //public IActionResult FormMetaFieldUpdateLogic(FormMetaField formMetaField)
        //{
        //    var result = _formMetaFieldService.Update(formMetaField);
        //    if (result.IsSuccess)
        //    {
        //        return View("FormMetaFieldUpdate", ResponseModel.Success(1, "修改成功"));
        //    }
        //    return View("FormMetaFieldUpdate", result.ToResponseModel());
        //}
    }
}