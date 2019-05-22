using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
{
    public class MetaObjectService : CommonInfoRepository<MetaObject>, IMetaObjectService
    {
        public MetaObjectService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IMetaFieldService _metaFieldService,
            IFieldListService _fieldListService,
            IInterfaceAggregationService _interfaceAggregationService,
            ISearchConditionService _searchConditionService,
            ITriggerScriptService _triggerScriptService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            metaFieldService = _metaFieldService;
            fieldListService = _fieldListService;
            interfaceAggregationService = _interfaceAggregationService;
            searchConditionService = _searchConditionService;
            triggerScriptService = _triggerScriptService;
        }

        MultiTenantPlatformDbContext dbContext;
        IMetaFieldService metaFieldService;
        IFieldListService fieldListService;
        IInterfaceAggregationService interfaceAggregationService;
        ISearchConditionService searchConditionService;
        ITriggerScriptService triggerScriptService;

        public List<MetaObject> GetMetaObjectsUnDeletedByApplicationId(int applicationId)
            => dbContext.Queryable<MetaObject>().Where(t => t.ApplicationId == applicationId && t.IsDeleted == (int)IsDeleted.UnDeleted).ToList();

        public List<MetaObject> GetMetaObjectsDeletedByApplicationId(int applicationId)
            => dbContext.Queryable<MetaObject>().Where(t => t.ApplicationId == applicationId && t.IsDeleted == (int)IsDeleted.Deleted).ToList();

        public MetaObject GetMetaObjectByCodeOrNameWithApplicationId(int applicationId, string code, string name)
            => dbContext.Queryable<MetaObject>().Where(t => (t.ApplicationId == applicationId && t.Name.Equals(name)) || (t.ApplicationId == applicationId && t.Code.Equals(code))).ToOne();

        public MetaObject GetMetaObjectByCodeAndApplicationId(int applicationId, string code)
            => dbContext.Queryable<MetaObject>().Where(t => t.ApplicationId == applicationId && t.Code.Equals(code)).ToOne();

        public new Result<MetaObject> Update(MetaObject metaObject)
        {
            MetaObject app = GetById(metaObject.Id);
            if (app != null)
            {
                app.Name = metaObject.Name;
                app.Group = metaObject.Group;
                app.SortNumber = metaObject.SortNumber;
                app.Description = metaObject.Description;
                app.ModifyBy = -1;
                app.ModifyTime = DateTime.Now;
            }
            base.Update(app);
            return Result<MetaObject>.Success();
        }

        public bool ExistSameNameWithOtherIdByApplicationId(int applicationId, int id, string name)
            => dbContext.Queryable<MetaObject>().Where(t => t.ApplicationId != applicationId && t.Name.Equals(name) && t.Id != id).Any();

        public Result<MetaObject> AddMetaObject(int applicationId, string applicationCode, MetaObject metaObject)
        {
            //check metaobject of name or code exist?
            MetaObject obj = GetMetaObjectByCodeOrNameWithApplicationId(applicationId, metaObject.Code, metaObject.Name);
            if (obj != null)
            {
                if (obj.Code.Equals(metaObject.Code))
                {
                    return Result<MetaObject>.Error("MetaObject Code Has Been Exist！", metaObject);
                }
                if (obj.Name.Equals(metaObject.Name))
                {
                    return Result<MetaObject>.Error("MetaObject Name Has Been Exist！", metaObject);
                }
            }
            if (applicationId == default(int))
            {
                return Result<MetaObject>.Error("Application Id Can Not Be Null！", metaObject);
            }

            metaObject.ApplicationId = applicationId;
            metaObject.Code = $"{applicationCode}.{metaObject.Code}";

            return TransactionHelper.Transaction(() =>
            {
                Add(metaObject);

                obj = GetMetaObjectByCodeAndApplicationId(metaObject.ApplicationId, metaObject.Code);

                if (obj == null)
                {
                    return Result<MetaObject>.Error("add metaobject then query faild！", metaObject);
                }

                //预置字段数据
                //将公共字段统一处理，不每个对象预置
                //metaFieldService.PresetFields(obj.Id);

                return Result<MetaObject>.Success();
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public new Result<MetaObject> Delete(int id)
        {
            var metaObject = GetById(id);
            TransactionHelper.Transaction(() =>
            {
                //把相关字段一并删除
                metaFieldService.DeleteByMetaObjectId(id);
                metaFieldService.DeleteByMetaObjectId(id);
                fieldListService.DeleteByMetaObjectId(id);//删除相关子对象
                interfaceAggregationService.DeleteByMetaObjectId(id);
                searchConditionService.DeleteByMetaObjectId(id);//删除相关子对象
                triggerScriptService.DeleteByMetaObjectId(id);
                //这里补充待删除的子对象
                //...
                base.Delete(id);
            });
            return Result<MetaObject>.Success();
        }
    }
}
