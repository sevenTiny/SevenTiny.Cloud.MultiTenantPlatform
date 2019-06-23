using SevenTiny.Bantina;
using System.Collections.Generic;

namespace SevenTiny.Cloud.Account.Core.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Result<TEntity> Add(TEntity entity);
        Result<IList<TEntity>> Add(IList<TEntity> entities);
        Result<TEntity> Update(TEntity entity);
        Result<TEntity> Delete(TEntity entity);
    }
}
