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
    public class InterfaceAggregationService : MetaObjectManageRepository<InterfaceAggregation>, IInterfaceAggregationService
    {
        public InterfaceAggregationService(
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

        //新增组织接口
        public new ResultModel Add(InterfaceAggregation entity)
        {
            if (entity.InterfaceType == (int)InterfaceType.TriggerScriptDataSource)
            {
                entity.FieldListName = "-";
                entity.SearchConditionName = "-";
            }
            else
            {
                //查询并将名字赋予接口的字段
                var interfaceField = interfaceFieldService.GetById(entity.FieldListId);
                var searchCondition = searchConditionService.GetById(entity.SearchConditionId);
                entity.FieldListName = interfaceField.Name;
                entity.SearchConditionName = searchCondition.Name;
            }

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
                if (interfaceAggregation.InterfaceType != (int)InterfaceType.TriggerScriptDataSource)
                {
                    var interfaceField = interfaceFieldService.GetById(interfaceAggregation.FieldListId);
                    var searchCondition = searchConditionService.GetById(interfaceAggregation.SearchConditionId);

                    myEntity.FieldListId = interfaceAggregation.FieldListId;
                    myEntity.FieldListName = interfaceField.Name;

                    myEntity.SearchConditionId = interfaceAggregation.SearchConditionId;
                    myEntity.SearchConditionName = searchCondition.Name;
                }

                myEntity.InterfaceType = interfaceAggregation.InterfaceType;

                //如果脚本有改动，则清空脚本缓存
                if (myEntity.Script != null && !myEntity.Script.Equals(interfaceAggregation.Script))
                    TriggerScriptCache.ClearCache(interfaceAggregation.Script);
                myEntity.Script = interfaceAggregation.Script;

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

        public InterfaceAggregation GetByInterfaceAggregationCode(string interfaceAggregationCode)
        {
            var interfaceAggregation = dbContext.QueryOne<InterfaceAggregation>(t => t.Code.Equals(interfaceAggregationCode));
            return interfaceAggregation;
        }
    }
}
