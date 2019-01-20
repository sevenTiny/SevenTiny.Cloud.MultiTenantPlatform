using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Add(IList<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
