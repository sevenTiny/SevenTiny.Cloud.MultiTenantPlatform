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
            IInterfaceSearchConditionService _interfaceSearchConditionService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            this.interfaceFieldService = _interfaceFieldService;
            this.interfaceSearchConditionService = _interfaceSearchConditionService;
        }

        readonly MultiTenantPlatformDbContext dbContext;
        readonly IInterfaceFieldService interfaceFieldService;
        readonly IInterfaceSearchConditionService interfaceSearchConditionService;

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
                var interfaceSearchCondition = interfaceSearchConditionService.GetById(interfaceAggregation.InterfaceSearchConditionId);

                myEntity.InterfaceFieldName = interfaceField.Name;
                myEntity.InterfaceSearchConditionName = interfaceSearchCondition.Name;
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
    }
}
