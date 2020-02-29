using SevenTiny.Bantina;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Result<TEntity> Add(TEntity entity);
        Result<IList<TEntity>> BatchAdd(IList<TEntity> entities);
        Result<TEntity> Update(TEntity entity);
        Result<TEntity> Delete(TEntity entity);
    }
}
