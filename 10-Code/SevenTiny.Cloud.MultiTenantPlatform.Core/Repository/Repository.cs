using System.Collections.Generic;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public Repository(MultiTenantPlatformDbContext multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public Result Add(TEntity entity)
        {
            dbContext.Add(entity);
            return Result.Success();
        }

        public Result Add(IList<TEntity> entities)
        {
            dbContext.Add<TEntity>(entities);
            return Result.Success();
        }

        public Result Update(TEntity entity)
        {
            dbContext.Update(entity);
            return Result.Success();
        }

        public Result Delete(TEntity entity)
        {
            dbContext.Delete<TEntity>(entity);
            return Result.Success();
        }
    }
}
