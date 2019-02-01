using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web.Controllers
{
    public class FieldListController : ControllerBase
    {
        readonly IFieldListService fieldListService;
        readonly IMetaFieldService metaFieldService;
        readonly IInterfaceAggregationService interfaceAggregationService;
        readonly IFieldListAggregationService fieldAggregationService;

        public FieldListController(
            IFieldListService _interfaceFieldService,
            IMetaFieldService _metaFieldService,
            IInterfaceAggregationService _interfaceAggregationService,
            IFieldListAggregationService _fieldAggregationService
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
            var aggregateMetaFields = fieldAggregationService.GetByFieldListId(id)?.Select(t => t.MetaFieldId)?.ToList() ?? new List<int>();
            //获取到本对象的所有字段
            var metaFields = metaFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            //过滤出已配置过的字段
            var aggregateFields = metaFields.Where(t => aggregateMetaFields.Any(n => n == t.Id)).ToList();//get aggregateFields which metafield.id in aggregateMetaFields
            //剩下的是未配置的待选字段
            var waitingForSelection = metaFields.Where(t => !aggregateMetaFields.Contains(t.Id)).ToList();

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
            FieldListAggregation fieldAggregation = new FieldListAggregation { FieldListId = id };
            string metaFieldIdsString = Request.Form["metaFieldIds"];
            //get metafield ids
            int[] metaFieldIds = metaFieldIdsString.Split(',').Select(t => Convert.ToInt32(t)).ToArray();
            int[] fieldAggregationIds = fieldAggregationService.GetByFieldListId(id)?.Select(t => t.MetaFieldId)?.ToArray() ?? new int[0];
            IEnumerable<int> addIds = metaFieldIds.Except(fieldAggregationIds); //ids will add
            IEnumerable<int> deleteIds = fieldAggregationIds.Except(metaFieldIds);  //ids will delete

            IList<FieldListAggregation> fieldAggregations = new List<FieldListAggregation>();
            foreach (var item in addIds)
            {
                fieldAggregations.Add(new FieldListAggregation { FieldListId = id, MetaFieldId = item });
            }
            fieldAggregationService.Add(fieldAggregations);

            foreach (var item in deleteIds)
            {
                fieldAggregationService.DeleteByMetaFieldId(item);
            }
            return JsonResultModel.Success("保存成功！");
        }
    }
}