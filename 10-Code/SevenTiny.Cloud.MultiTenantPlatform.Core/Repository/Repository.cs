using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public Repository(MultiTenantPlatformDbContext multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public Result<TEntity> Add(TEntity entity)
        {
            dbContext.Add(entity);
            return Result<TEntity>.Success();
        }

        public Result<IList<TEntity>> Add(IList<TEntity> entities)
        {
            dbContext.Add<TEntity>(entities);
            return Result<IList<TEntity>>.Success();
        }

        public Result<TEntity> Update(TEntity entity)
        {
            dbContext.Update(entity);
            return Result<TEntity>.Success();
        }

        public Result<TEntity> Delete(TEntity entity)
        {
            dbContext.Delete<TEntity>(entity);
            return Result<TEntity>.Success();
        }
    }
}
