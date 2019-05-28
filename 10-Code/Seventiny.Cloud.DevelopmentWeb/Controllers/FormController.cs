using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seventiny.Cloud.DevelopmentWeb.Controllers
{
    public class FormController : ControllerBase
    {
        readonly IFormService _formService;
        readonly IMetaFieldService metaFieldService;
        readonly IFormMetaFieldService _formMetaFieldService;

        public FormController(
            IFormService formService,
            IMetaFieldService _metaFieldService,
            IFormMetaFieldService formMetaFieldService
            )
        {
            _formService = formService;
            metaFieldService = _metaFieldService;
            _formMetaFieldService = formMetaFieldService;
        }

        public IActionResult List()
        {
            return View(_formService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(Form entity)
        {
            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Add", ResponseModel.Error("名称不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Add", ResponseModel.Error("编码不能为空", entity));
            }
            //校验code格式
            if (!entity.Code.IsAlnum(2, 50))
            {
                return View("Add", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", entity));
            }

            //检查编码或名称重复
            var checkResult = _formService.CheckSameCodeOrName(CurrentMetaObjectId, entity);
            if (!checkResult.IsSuccess)
            {
                return View("Add", checkResult.ToResponseModel());
            }

            entity.MetaObjectId = CurrentMetaObjectId;
            //组合编码
            entity.Code = $"{CurrentMetaObjectCode}.Form.{entity.Code}";
            _formService.Add(entity);
            return RedirectToAction("List");
        }

        public IActionResult Update(int id)
        {
            var entity = _formService.GetById(id);
            return View(ResponseModel.Success(entity));
        }

        public IActionResult UpdateLogic(Form entity)
        {
            if (entity.Id == 0)
            {
                return View("Update", ResponseModel.Error("Id 不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Update", ResponseModel.Error("Name 不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Update", ResponseModel.Error("Code 不能为空", entity));
            }
            //校验code格式
            if (!entity.Code.IsAlnum(2, 50))
            {
                return View("Update", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", entity));
            }

            //检查编码或名称重复
            var checkResult = _formService.CheckSameCodeOrName(CurrentMetaObjectId, entity);
            if (!checkResult.IsSuccess)
            {
                return View("Update", checkResult.ToResponseModel());
            }

            //更新操作
            _formService.Update(entity);

            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            _formService.Delete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult LogicDelete(int id)
        {
            _formService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            _formService.Recover(id);
            return JsonResultModel.Success("恢复成功");
        }

        /// <summary>
        /// 组织字段
        /// </summary>
        /// <param name="id">字段配置对象id</param>
        /// <returns></returns>
        public IActionResult AggregateField(int id)
        {
            //获取组织字段里面本字段配置下的所有字段
            var fieldListAggregations = _formMetaFieldService.GetByFormId(id) ?? new List<FormMetaField>();
            var aggregateMetaFieldIds = fieldListAggregations.Select(t => t.MetaFieldId).ToArray();
            //获取到本对象的所有字段
            var metaFields = metaFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            //过滤出已配置过的字段
            var aggregateFields = metaFields.Where(t => fieldListAggregations.Any(n => n.MetaFieldId == t.Id))
                .Select(t =>
                {
                    var fieldAggregation = fieldListAggregations.FirstOrDefault(f => f.MetaFieldId == t.Id);
                    return new MetaFieldViewModel
                    {
                        Id = t.Id,
                        Code = t.Code,
                        Name = t.Name,
                        FieldAggregationId = fieldAggregation.Id,
                        SortNumber = fieldAggregation.SortNumber
                    };
                }).OrderBy(t => t.SortNumber).ToList();
            //剩下的是未配置的待选字段
            var waitingForSelection = metaFields.Where(t => !aggregateMetaFieldIds.Contains(t.Id)).ToList();

            //【BUG】下面这种从查询到的内存数据中排除的写法是错误的，会导致内存做的缓存中的数据根据引用地址跟着修改，导致缓存数据被改动，下次查询出错
            //如果使用序列化做的缓存应该不会有如下语句出现的缓存问题
            //aggregateMetaFields.ForEach(t => metaFields.RemoveAll(n => n.Id == t));//remove metafield aggregateField exist.

            ViewData["AggregateFields"] = aggregateFields;
            ViewData["MetaFields"] = waitingForSelection;
            ViewData["FormId"] = id;
            return View();
        }

        /// <summary>
        /// 组织字段添加逻辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult AggregateFieldAddLogic(int id)
        {
            string metaFieldIdsString = Request.Form["metaFieldIds"];
            //get metafield ids
            var metaFieldIdsSplit = !string.IsNullOrEmpty(metaFieldIdsString) ? metaFieldIdsString.Split(',') : Array.Empty<string>();

            int[] metaFieldIds = (metaFieldIdsSplit != null && metaFieldIdsSplit.Any()) ? metaFieldIdsSplit.Select(t => Convert.ToInt32(t)).ToArray() : new int[0];
            int[] fieldAggregationIds = _formMetaFieldService.GetByFormId(id)?.Select(t => t.MetaFieldId)?.ToArray() ?? new int[0];
            IEnumerable<int> addIds = metaFieldIds.Except(fieldAggregationIds); //ids will add
            IEnumerable<int> deleteIds = fieldAggregationIds.Except(metaFieldIds);  //ids will delete

            IList<FormMetaField> formMetaFields = new List<FormMetaField>();
            foreach (var item in addIds)
            {
                formMetaFields.Add(new FormMetaField { FormId = id, MetaFieldId = item });
            }

            if (formMetaFields.Any())
                _formMetaFieldService.Add(formMetaFields);

            foreach (var item in deleteIds)
            {
                _formMetaFieldService.DeleteByMetaFieldId(item);
            }

            //对当前列表配置的顺序进行重新排序
            _formMetaFieldService.SortFields(id, metaFieldIds);

            return JsonResultModel.Success("保存成功！");
        }

        /// <summary>
        /// 列表字段配置编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult FormMetaFieldUpdate(int id)
        {
            var fieldListAggregation = _formMetaFieldService.GetById(id);
            return View(ResponseModel.Success(fieldListAggregation));
        }
        public IActionResult FormMetaFieldUpdateLogic(FormMetaField formMetaField)
        {
            var result = _formMetaFieldService.Update(formMetaField);
            if (result.IsSuccess)
            {
                return View("FormMetaFieldUpdate", ResponseModel.Success(1, "修改成功"));
            }
            return View("FormMetaFieldUpdate", result.ToResponseModel());
        }
    }
}