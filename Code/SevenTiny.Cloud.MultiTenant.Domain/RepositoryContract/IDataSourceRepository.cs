using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract
{
    internal interface IDataSourceRepository : ICommonRepositoryBase<DataSource>
    {
        List<DataSource> GetListByApplicationIdAndDataSourceType(Guid applicationId, DataSourceType dataSourceType);
    }
}
