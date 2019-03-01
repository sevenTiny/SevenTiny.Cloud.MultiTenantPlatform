using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class IndexViewService : MetaObjectManageRepository<IndexView>, IIndexViewService
    {
        public IndexViewService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IFieldListService _interfaceFieldService,
            ISearchConditionService _searchConditionService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            this.interfaceFieldService = _interfaceFieldService;
            this.searchConditionService = _searchConditionService;
        }

        readonly MultiTenantPlatformDbContext dbContext;
        readonly IFieldListService interfaceFieldService;
        readonly ISearchConditionService searchConditionService;

        //新增
        public new ResultModel Add(IndexView entity)
        {
            //查询并将名字赋予字段
            var interfaceField = interfaceFieldService.GetById(entity.FieldListId);
            var searchCondition = searchConditionService.GetById(entity.SearchConditionId);
            entity.FieldListName = interfaceField.Name;
            entity.SearchConditionName = searchCondition.Name;
            entity.Title = entity.Name;

            base.Add(entity);
            return ResultModel.Success();
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity"></param>
        public new ResultModel Update(IndexView entity)
        {
            IndexView myEntity = GetById(entity.Id);
            if (myEntity != null)
            {
                var interfaceField = interfaceFieldService.GetById(entity.FieldListId);
                var searchCondition = searchConditionService.GetById(entity.SearchConditionId);

                myEntity.FieldListId = entity.FieldListId;
                myEntity.FieldListName = interfaceField.Name;

                myEntity.SearchConditionId = entity.SearchConditionId;
                myEntity.SearchConditionName = searchCondition.Name;

                //编码不允许修改
                myEntity.Name = entity.Name;
                myEntity.Group = entity.Group;
                myEntity.SortNumber = entity.SortNumber;
                myEntity.Description = entity.Description;
                myEntity.ModifyBy = -1;
                myEntity.ModifyTime = DateTime.Now;
                myEntity.Title = entity.Title;
                myEntity.Icon = entity.Icon;
            }
            base.Update(myEntity);
            return ResultModel.Success();
        }
    }
}
