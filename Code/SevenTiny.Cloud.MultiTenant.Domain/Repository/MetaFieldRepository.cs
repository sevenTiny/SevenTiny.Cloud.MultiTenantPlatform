using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.Repository
{
    internal class MetaFieldRepository : MetaObjectCommonRepositoryBase<MetaField>, IMetaFieldRepository
    {
        public MetaFieldRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
        }

        public List<MetaField> GetSystemAndCustomListUnDeleted(Guid metaObjectId)
        => _dbContext.MetaField.Where(t => t.MetaObjectId == metaObjectId || t.MetaObjectId == Guid.Empty).ToList();
    }
}
