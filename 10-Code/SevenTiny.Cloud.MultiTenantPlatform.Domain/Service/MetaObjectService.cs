using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class MetaObjectService : CommonInfoRepository<MetaObject>, IMetaObjectService
    {
        public MetaObjectService(MultiTenantPlatformDbContext multiTenantPlatformDbContext, IMetaFieldService _metaFieldService) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            metaFieldService = _metaFieldService;
        }

        MultiTenantPlatformDbContext dbContext;
        IMetaFieldService metaFieldService;

        public List<MetaObject> GetMetaObjectsUnDeletedByApplicationId(int applicationId)
            => dbContext.QueryList<MetaObject>(t => t.ApplicationId == applicationId && t.IsDeleted == (int)IsDeleted.UnDeleted);

        public List<MetaObject> GetMetaObjectsDeletedByApplicationId(int applicationId)
            => dbContext.QueryList<MetaObject>(t => t.ApplicationId == applicationId && t.IsDeleted == (int)IsDeleted.Deleted);

        public MetaObject GetMetaObjectByCodeOrNameWithApplicationId(int applicationId, string code, string name)
            => dbContext.QueryOne<MetaObject>(t => (t.ApplicationId == applicationId && t.Name.Equals(name)) || (t.ApplicationId == applicationId && t.Code.Equals(code)));

        public MetaObject GetMetaObjectByCodeAndApplicationId(int applicationId, string code)
            => dbContext.QueryOne<MetaObject>(t => t.ApplicationId == applicationId && t.Code.Equals(code));

        public new void Update(MetaObject metaObject)
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
            dbContext.Update(app);
        }

        public bool ExistSameNameWithOtherIdByApplicationId(int applicationId, int id, string name)
            => dbContext.QueryExist<MetaObject>(t => t.ApplicationId != applicationId && t.Name.Equals(name) && t.Id != id);

        public ResultModel AddMetaObject(int applicationId, string applicationCode, MetaObject metaObject)
        {
            //check metaobject of name or code exist?
            MetaObject obj = GetMetaObjectByCodeOrNameWithApplicationId(applicationId, metaObject.Code, metaObject.Name);
            if (obj != null)
            {
                if (obj.Code.Equals(metaObject.Code))
                {
                    return ResultModel.Error("MetaObject Code Has Been Exist！", metaObject);
                }
                if (obj.Name.Equals(metaObject.Name))
                {
                    return ResultModel.Error("MetaObject Name Has Been Exist！", metaObject);
                }
            }
            if (applicationId == default(int))
            {
                return ResultModel.Error("Application Id Can Not Be Null！", metaObject);
            }

            metaObject.ApplicationId = applicationId;
            metaObject.Code = $"{applicationCode}.{metaObject.Code}";

            return TransactionHelper.Transaction(() =>
            {
                Add(metaObject);

                obj = GetMetaObjectByCodeAndApplicationId(metaObject.ApplicationId, metaObject.Code);

                if (obj == null)
                {
                    return ResultModel.Error("add metaobject then query faild！", metaObject);
                }

                //预置字段数据
                metaFieldService.PresetFields(obj.Id);

                return ResultModel.Success();
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public new void Delete(int id)
        {
            var metaObject = GetById(id);
            TransactionHelper.Transaction(() =>
            {
                //把相关字段一并删除
                metaFieldService.DeleteByMetaObjectId(id);
                //删除相关接口配置字段
                //...
                Delete(id);
            });
        }
    }
}
