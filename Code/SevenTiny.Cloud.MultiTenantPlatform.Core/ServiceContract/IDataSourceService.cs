using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
{
    public interface IDataSourceService : ICommonInfoRepository<DataSource>
    {
        List<DataSource> GetListByAppIdAndDataSourceType(int applicationId, DataSourceType dataSourceType);
        Result CheckSameCodeOrName(DataSource entity);
    }
}
