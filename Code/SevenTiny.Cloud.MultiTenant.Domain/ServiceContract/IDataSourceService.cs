using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IDataSourceService : ICommonServiceBase<DataSource>
    {
        Result<DataSource> Update(DataSource entity);
    }
}
