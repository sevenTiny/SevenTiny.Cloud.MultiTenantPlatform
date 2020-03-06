using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.Repository
{
    internal class DataSourceRepository : CommonRepositoryBase<DataSource>, IDataSourceRepository
    {
        public DataSourceRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
        }

        public List<DataSource> GetListByApplicationIdAndDataSourceType(Guid applicationId, DataSourceType dataSourceType)
        {
            return _dbContext.Queryable<DataSource>().Where(t => t.ApplicationId == applicationId && t.DataSourceType == (int)dataSourceType).ToList();
        }
    }
}
