using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.DataAccess;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.Repository
{
    public class ApplicationRepository : CommonRepositoryBase<Application>, IApplicationRepository
    {
        public ApplicationRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            _dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext _dbContext;
    }
}
