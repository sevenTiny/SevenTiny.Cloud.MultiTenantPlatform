using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.DataApi.Models;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi.Controllers
{
    [EnableCors("AllowSameDomain")]
    [Route("api/UI/[controller]")]
    [ApiController]
    public class TableListController : ControllerBase
    {
        public TableListController(
            IDataAccessService dataAccessService,
            ISearchConditionNodeService conditionAggregationService,
            IIndexViewService indexViewService,
            IMetaFieldService metaFieldService
            )
        {
            _dataAccessService = dataAccessService;
            _conditionAggregationService = conditionAggregationService;
            _indexViewService = indexViewService;
            _metaFieldService = metaFieldService;
        }

        readonly IDataAccessService _dataAccessService;
        readonly IIndexViewService _indexViewService;
        readonly ISearchConditionNodeService _conditionAggregationService;
        readonly IMetaFieldService _metaFieldService;


        /// <summary>
        /// 1.预处理统一的参数验证逻辑
        /// 2.根据视图信息创建查询管道上下文
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        private QueryPiplineContext PretreatmentAndCreateQueryPiplineContext(UITableListQueryArgs queryArgs)
        {
            //args check
            if (queryArgs == null)
                throw new Exception($"Parameter invalid:queryArgs = null");

            queryArgs.ArgsCheck();

            //argumentsDic generate
            Dictionary<string, object> argumentsDic = new Dictionary<string, object>();
            foreach (var item in Request.Query)
                argumentsDic.AddOrUpdate(item.Key.ToUpperInvariant(), item.Value);

            //get interface info
            var indexView = _indexViewService.GetByCode(queryArgs.ViewName);
            if (indexView == null)
                throw new Exception($"未能找到视图编码为[{queryArgs.ViewName}]对应的视图信息");

            if (indexView.LayoutType != (int)LayoutType.SearchForm_TableList)
                throw new Exception("该视图编码对应的视图非列表布局，不能用于展示列表");
            
            //create queryContext
            QueryPiplineContext queryContext = new QueryPiplineContext();
            queryContext.ArgumentsDic = argumentsDic;
            queryContext.ApplicationCode = indexView.Code.Split('.')[0];
            queryContext.MetaObjectId = indexView.MetaObjectId;
            queryContext.SearchConditionId = indexView.SearchConditionId;
            queryContext.FieldListId = indexView.FieldListId;

            return queryContext;
        }

        /// <summary>
        /// 这块的逻辑为：
        /// 根据视图找到搜索表单和列表
        /// 依据搜索表单中的
        /// </summary>
        /// <param name="queryArgs"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]UITableListQueryArgs queryArgs)
        {
            try
            {
                var pipelineContext = PretreatmentAndCreateQueryPiplineContext(queryArgs);

                //search data条件参数提供
                if (queryArgs.SearchData?.Items != null)
                    foreach (var item in queryArgs.SearchData.Items)
                        pipelineContext.ArgumentsDic.AddOrUpdate(item.Code.ToUpperInvariant(), item.Value);

                //分析搜索条件，是否忽略参数校验为true，如果参数没传递则不抛出异常且处理为忽略参数
                var filter = _conditionAggregationService.AnalysisConditionToFilterDefinitionByConditionId(pipelineContext, true);
                //如果参数都没传递或者其他原因导致条件没有，则直接返回全部
                if (filter == null)
                    filter = Builders<BsonDocument>.Filter.Empty;

                //filter = triggerScriptEngineService.TableListBefore(indexView.MetaObjectId, indexView.Code, filter);
                var sort = _metaFieldService.GetSortDefinitionBySortFields(pipelineContext, queryArgs.SortFields);
                var tableListComponent = _dataAccessService.GetTableListComponent(pipelineContext, filter, queryArgs.PageIndex, queryArgs.PageSize, sort, out int totalCount);
                //tableListComponent = _triggerScriptEngineService.TableListAfter(indexView.MetaObjectId, indexView.Code, tableListComponent);

                return JsonResultModel.Success("get table component success", tableListComponent);
            }
            catch (ArgumentNullException argNullEx)
            {
                return JsonResultModel.Error(argNullEx.Message);
            }
            catch (ArgumentException argEx)
            {
                return JsonResultModel.Error(argEx.Message);
            }
            catch (Exception ex)
            {
                return JsonResultModel.Error(ex.Message);
            }
        }
    }
}