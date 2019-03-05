using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Result Add(TEntity entity);
        Result Add(IList<TEntity> entities);
        Result Update(TEntity entity);
        Result Delete(TEntity entity);
    }
}
