using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface ICommonServiceBase<TEntity> where TEntity : CommonBase
    {
    }
}
