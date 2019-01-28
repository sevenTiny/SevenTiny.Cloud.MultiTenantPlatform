using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class InterfaceAggregationService : MetaObjectManageRepository<InterfaceAggregation>, IInterfaceAggregationService
    {
        public InterfaceAggregationService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IInterfaceFieldService _interfaceFieldService,
            ISearchConditionService _searchConditionService,
            IMetaObjectService _metaObjectService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            this.interfaceFieldService = _interfaceFieldService;
            this.searchConditionService = _searchConditionService;
            metaObjectService = _metaObjectService;
        }

        readonly MultiTenantPlatformDbContext dbContext;
        readonly IInterfaceFieldService interfaceFieldService;
        readonly ISearchConditionService searchConditionService;
        readonly IMetaObjectService metaObjectService;

        //新增组织接口
        public new ResultModel Add(InterfaceAggregation entity)
        {
            //查询并将名字赋予接口的字段
            var interfaceField = interfaceFieldService.GetById(entity.InterfaceFieldId);
            var searchCondition = searchConditionService.GetById(entity.SearchConditionId);
            entity.InterfaceFieldName = interfaceField.Name;
            entity.SearchConditionName = searchCondition.Name;

            base.Add(entity);
            return ResultModel.Success();
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="interfaceAggregation"></param>
        public new ResultModel Update(InterfaceAggregation interfaceAggregation)
        {
            InterfaceAggregation myEntity = GetById(interfaceAggregation.Id);
            if (myEntity != null)
            {
                var interfaceField = interfaceFieldService.GetById(interfaceAggregation.InterfaceFieldId);
                var searchCondition = searchConditionService.GetById(interfaceAggregation.SearchConditionId);

                myEntity.InterfaceFieldName = interfaceField.Name;
                myEntity.SearchConditionName = searchCondition.Name;
                //编码不允许修改
                myEntity.Name = interfaceAggregation.Name;
                myEntity.Group = interfaceAggregation.Group;
                myEntity.SortNumber = interfaceAggregation.SortNumber;
                myEntity.Description = interfaceAggregation.Description;
                myEntity.ModifyBy = -1;
                myEntity.ModifyTime = DateTime.Now;
            }
            base.Update(myEntity);
            return ResultModel.Success();
        }

        public InterfaceAggregation GetByMetaObjectIdAndInterfaceAggregationCode(int metaObjectId, string interfaceAggregationCode)
        {
            var interfaceAggregation = dbContext.QueryOne<InterfaceAggregation>(t => t.MetaObjectId == metaObjectId && t.Code.Equals(interfaceAggregationCode));
            if (interfaceAggregation == null)
            {
                throw new KeyNotFoundException("该编码对应的接口未找到");
            }
            return interfaceAggregation;
        }
    }
}
