using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.Enum;
using SevenTiny.Cloud.MultiTenant.Core.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Core.ServiceContract
{
    public interface IDataSourceService : ICommonInfoRepository<DataSource>
    {
        List<DataSource> GetListByAppIdAndDataSourceType(int applicationId, DataSourceType dataSourceType);
        Result CheckSameCodeOrName(DataSource entity);
    }
}
