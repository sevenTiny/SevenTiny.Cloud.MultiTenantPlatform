//using SevenTiny.Cloud.MultiTenant.Domain.Entity;
//using SevenTiny.Cloud.MultiTenant.Domain.Enum;
//using SevenTiny.Cloud.MultiTenant.Domain.Repository;
//using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
//using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.IndexPage;
//using SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject;
//using SevenTiny.Cloud.MultiTenant.Infrastructure.Caching;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SevenTiny.Cloud.MultiTenant.Domain.Enum;
//using SevenTiny.Bantina;
//using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
//using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView;

//namespace SevenTiny.Cloud.MultiTenant.Domain.Service
//{
//    internal class IndexViewService : MetaObjectCommonRepositoryBase<IndexView>, IIndexViewService
//    {
//        public IndexViewService(
//            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
//            IListViewService fieldListService,
//            ISearchConditionService _searchConditionService,
//            ISearchConditionNodeService _searchConditionAggregationService
//            ) : base(multiTenantPlatformDbContext)
//        {
//            dbContext = multiTenantPlatformDbContext;
//            this._fieldListService = fieldListService;
//            this.searchConditionService = _searchConditionService;
//            searchConditionAggregationService = _searchConditionAggregationService;
//        }

//        readonly MultiTenantPlatformDbContext dbContext;
//        readonly IListViewService _fieldListService;
//        readonly ISearchConditionService searchConditionService;
//        readonly ISearchConditionNodeService searchConditionAggregationService;

//        //新增
//        public new Result<IndexView> Add(IndexView entity)
//        {
//            //查询并将名字赋予字段
//            var interfaceField = _fieldListService.GetById(entity.FieldListId);
//            var searchCondition = searchConditionService.GetById(entity.SearchConditionId);
//            entity.FieldListName = interfaceField.Name;
//            entity.SearchConditionName = searchCondition.Name;
//            entity.Title = entity.Name;

//            base.Add(entity);
//            return Result<IndexView>.Success();
//        }

//        /// <summary>
//        /// 更新对象
//        /// </summary>
//        /// <param name="entity"></param>
//        public new Result<IndexView> Update(IndexView entity)
//        {

//            IndexView myEntity = GetById(entity.Id);
//            if (myEntity != null)
//            {
//                var interfaceField = _fieldListService.GetById(entity.FieldListId);
//                var searchCondition = searchConditionService.GetById(entity.SearchConditionId);

//                myEntity.FieldListId = entity.FieldListId;
//                myEntity.FieldListName = interfaceField.Name;

//                myEntity.SearchConditionId = entity.SearchConditionId;
//                myEntity.SearchConditionName = searchCondition.Name;

//                //编码不允许修改
//                myEntity.Name = entity.Name;
//                myEntity.Group = entity.Group;
//                myEntity.SortNumber = entity.SortNumber;
//                myEntity.Description = entity.Description;
//                myEntity.ModifyBy = -1;
//                myEntity.ModifyTime = DateTime.Now;
//                myEntity.Title = entity.Title;
//                myEntity.Icon = entity.Icon;
//            }
//            base.Update(myEntity);
//            return Result<IndexView>.Success();
//        }

//        /// <summary>
//        /// 构建视图UI组件
//        /// </summary>
//        /// <param name="indexView"></param>
//        /// <returns></returns>
//        public IndexPageComponent GetIndexPageComponentByIndexView(IndexView indexView)
//        {
//            var indexPage = new IndexPageComponent();
//            indexPage.Title = indexView.Title;
//            indexPage.Icon = indexView.Icon;
//            //通过配置IndexView来分析
//            indexPage.LayoutType = indexView.LayoutType;

//            switch ((LayoutType)indexPage.LayoutType)
//            {
//                case LayoutType.SearchForm_TableList:
//                    #region 构建SearchForm
//                    var conditionAggregations = searchConditionAggregationService.GetConditionItemsBySearchConditionId(indexView.SearchConditionId);
//                    if (conditionAggregations != null && conditionAggregations.Any())
//                    {
//                        List<SearchItem> searchItems = new List<SearchItem>();
//                        foreach (var item in conditionAggregations)
//                        {
//                            searchItems.Add(new SearchItem
//                            {
//                                Name = item.FieldCode,
//                                Text = item.Text,
//                                Visible = TrueFalseTranslator.ToBoolean(item.Visible),
//                                Value = item.Value,
//                                //字段类型
//                                Type = item.MetaFieldType,
//                                ValueType = item.ValueType
//                            });
//                        }
//                        indexPage.SearchFormComponent = new SearchFormComponent { SearchItems = searchItems.ToArray() };
//                    }
//                    #endregion

//                    #region 构建TableList
//                    var fieldList = _fieldListService.GetById(indexView.FieldListId);
//                    if (fieldList != null)
//                    {
//                        indexPage.ListViewComponent = new ListViewComponent { Code = fieldList.Code };//title icon等需要完善补充tablelist
//                    }
//                    #endregion
//                    break;
//                default:
//                    break;
//            }

//            #region 构建ButtonList
//            #endregion

//            return indexPage;
//        }
//    }
//}
