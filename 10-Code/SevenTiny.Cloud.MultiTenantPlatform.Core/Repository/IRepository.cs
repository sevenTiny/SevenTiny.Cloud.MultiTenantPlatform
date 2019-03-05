using SevenTiny.Cloud.MultiTenantPlatform.Core.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        ResultModel Add(TEntity entity);
        ResultModel Add(IList<TEntity> entities);
        ResultModel Update(TEntity entity);
        ResultModel Delete(TEntity entity);
    }
}
