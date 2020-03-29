using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.Repository
{
    internal class CloudInterfaceRepository : MetaObjectCommonRepositoryBase<CloudInterface>, ICloudInterfaceRepository
    {
        public CloudInterfaceRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {

        }

        public bool CheckFormIdExist(Guid formViewId)
            => _dbContext.CloudInterface.Where(t => t.FormViewId == formViewId).Any();
    }
}
