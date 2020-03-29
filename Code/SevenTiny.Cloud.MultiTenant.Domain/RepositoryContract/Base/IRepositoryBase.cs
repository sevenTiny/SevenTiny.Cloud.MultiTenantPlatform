using SevenTiny.Bantina;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract
{
    internal interface IRepositoryBase<TEntity> where TEntity : class
    {
        Result Add(TEntity entity);
        Result<IList<TEntity>> BatchAdd(IList<TEntity> entities);
        Result Update(TEntity entity);
        Result Delete(TEntity entity);

        void TransactionBegin();
        void TransactionCommit();
        void TransactionRollback();
    }
}
