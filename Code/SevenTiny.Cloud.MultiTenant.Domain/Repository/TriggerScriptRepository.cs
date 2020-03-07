using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Repository
{
    internal class TriggerScriptRepository : MetaObjectCommonRepositoryBase<TriggerScript>, ITriggerScriptRepository
    {
        public TriggerScriptRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
        }

        public List<TriggerScript> GetUnDeletedListByMetaObjectIdAndServiceType(Guid metaObjectId, int serviceType)
            => _dbContext.Queryable<TriggerScript>().Where(t => t.MetaObjectId == metaObjectId && t.ServiceType == serviceType).ToList();
    }
}
