using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Repository
{
    internal class MetaObjectRepository : CommonRepositoryBase<MetaObject>, IMetaObjectRepository
    {
        public MetaObjectRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
        }

        public List<MetaObject> GetMetaObjectListUnDeletedByApplicationId(Guid applicationId)
            => _dbContext.Queryable<MetaObject>().Where(t => t.ApplicationId == applicationId && t.IsDeleted == (int)IsDeleted.UnDeleted).ToList();

        public List<MetaObject> GetMetaObjectListDeletedByApplicationId(Guid applicationId)
            => _dbContext.Queryable<MetaObject>().Where(t => t.ApplicationId == applicationId && t.IsDeleted == (int)IsDeleted.Deleted).ToList();

        public MetaObject GetMetaObjectByCodeOrNameWithApplicationId(Guid applicationId, string code, string name)
            => _dbContext.Queryable<MetaObject>().Where(t => t.ApplicationId == applicationId && (t.Name.Equals(name) || t.Code.Equals(code))).FirstOrDefault();

        public MetaObject GetMetaObjectByCodeAndApplicationId(Guid applicationId, string code)
            => _dbContext.Queryable<MetaObject>().Where(t => t.ApplicationId == applicationId && t.Code.Equals(code)).FirstOrDefault();
    }
}
