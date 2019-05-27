using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seventiny.Cloud.DevelopmentWeb.Controllers
{
    public class FieldListController : ControllerBase
    {
        readonly IFieldListService fieldListService;
        readonly IMetaFieldService metaFieldService;
        readonly IInterfaceAggregationService interfaceAggregationService;
        readonly IFieldListMetaFieldService fieldAggregationService;

        public FieldListController(
            IFieldListService _interfaceFieldService,
            IMetaFieldService _metaFieldService,
            IInterfaceAggregationService _interfaceAggregationService,
            IFieldListMetaFieldService _fieldAggregationService
            )
        {
            fieldListService = _interfaceFieldService;
            metaFieldService = _metaFieldService;
            interfaceAggregationService = _interfaceAggregationService;
            fieldAggregationService = _fieldAggregationService;
        }

        public IActionResult List()
        {
            return View(fieldListService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult DeleteList()
        {
            return View(fieldListService.GetEntitiesDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(FieldList entity)
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
            var checkResult = fieldListService.CheckSameCodeOrName(CurrentMetaObjectId, entity);
            if (!checkResult.IsSuccess)
            {
                return View("Add", checkResult.ToResponseModel());
            }

            entity.MetaObjectId = CurrentMetaObjectId;
            //组合编码
            entity.Code = $"{CurrentMetaObjectCode}.FieldList.{entity.Code}";
            fieldListService.Add(entity);
            return RedirectToAction("List");
        }

        public IActionResult Update(int id)
        {
            var interfaceField = fieldListService.GetById(id);
            return View(ResponseModel.Success(interfaceField));
        }

        public IActionResult UpdateLogic(FieldList entity)
        {
            if (entity.Id == 0)
            {
                return View("Update", ResponseModel.Error("MetaField Id 不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Update", ResponseModel.Error("MetaField Name 不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Update", ResponseModel.Error("MetaField Code 不能为空", entity));
            }
            //校验code格式
            if (!entity.Code.IsAlnum(2, 50))
            {
                return View("Update", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", entity));
            }

            //检查编码或名称重复
            var checkResult = fieldListService.CheckSameCodeOrName(CurrentMetaObjectId, entity);
            if (!checkResult.IsSuccess)
            {
                return View("Update", checkResult.ToResponseModel());
            }

            //更新操作
            fieldListService.Update(entity);

            return RedirectToAction("List");
        }

        public IActionResult Delete(int id)
        {
            fieldListService.Delete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult LogicDelete(int id)
        {
            fieldListService.LogicDelete(id);
            return JsonResultModel.Success("删除成功");
        }

        public IActionResult Recover(int id)
        {
            fieldListService.Recover(id);
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
            var fieldListAggregations = fieldAggregationService.GetByFieldListId(id) ?? new List<FieldListMetaField>();
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
            ViewData["FieldListId"] = id;
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
            int[] fieldAggregationIds = fieldAggregationService.GetByFieldListId(id)?.Select(t => t.MetaFieldId)?.ToArray() ?? new int[0];
            IEnumerable<int> addIds = metaFieldIds.Except(fieldAggregationIds); //ids will add
            IEnumerable<int> deleteIds = fieldAggregationIds.Except(metaFieldIds);  //ids will delete

            IList<FieldListMetaField> fieldAggregations = new List<FieldListMetaField>();
            foreach (var item in addIds)
            {
                fieldAggregations.Add(new FieldListMetaField { FieldListId = id, MetaFieldId = item });
            }

            if (fieldAggregations.Any())
                fieldAggregationService.Add(fieldAggregations);

            foreach (var item in deleteIds)
            {
                fieldAggregationService.DeleteByMetaFieldId(item);
            }

            //对当前列表配置的顺序进行重新排序
            fieldAggregationService.SortFields(id, metaFieldIds);

            return JsonResultModel.Success("保存成功！");
        }

        /// <summary>
        /// 列表字段配置编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult FieldListMetaFieldUpdate(int id)
        {
            var fieldListAggregation = fieldAggregationService.GetById(id);
            return View(ResponseModel.Success(fieldListAggregation));
        }
        public IActionResult FieldListAggregationUpdateLogic(FieldListMetaField fieldListAggregation)
        {
            var result = fieldAggregationService.Update(fieldListAggregation);
            if (result.IsSuccess)
            {
                return View("FieldListAggregationUpdate", ResponseModel.Success(1, "修改成功"));
            }
            return View("FieldListAggregationUpdate", result.ToResponseModel());
        }
    }
}