using System.Collections.Generic;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public Repository(MultiTenantPlatformDbContext multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public void Add(TEntity entity)
            => dbContext.Add(entity);

        public void Add(IList<TEntity> entities)
            => dbContext.Add<TEntity>(entities);

        public void Update(TEntity entity)
            => dbContext.Update(entity);

        public void Delete(TEntity entity)
            => dbContext.Delete<TEntity>(entity);
    }
}
