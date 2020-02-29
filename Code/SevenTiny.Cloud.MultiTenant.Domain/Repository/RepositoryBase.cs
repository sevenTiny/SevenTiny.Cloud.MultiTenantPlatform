using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DataAccess;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        public RepositoryBase(MultiTenantPlatformDbContext multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public Result<TEntity> Add(TEntity entity)
        {
            dbContext.Add(entity);
            return Result<TEntity>.Success();
        }

        public Result<IList<TEntity>> BatchAdd(IList<TEntity> entities)
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
