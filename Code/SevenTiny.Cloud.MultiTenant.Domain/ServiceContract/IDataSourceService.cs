using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IDataSourceService : ICommonInfoRepository<DataSource>
    {
        List<DataSource> GetListByAppIdAndDataSourceType(int applicationId, DataSourceType dataSourceType);
        Result CheckSameCodeOrName(DataSource entity);
    }
}
