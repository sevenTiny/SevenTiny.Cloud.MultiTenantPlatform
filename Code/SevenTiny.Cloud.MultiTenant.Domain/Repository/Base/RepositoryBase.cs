using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Repository
{
    internal abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        public RepositoryBase(MultiTenantPlatformDbContext multiTenantPlatformDbContext)
        {
            _dbContext = multiTenantPlatformDbContext;
        }

        protected MultiTenantPlatformDbContext _dbContext;

        public Result Add(TEntity entity)
        {
            _dbContext.Add(entity);
            return Result.Success();
        }

        public Result<IList<TEntity>> BatchAdd(IList<TEntity> entities)
        {
            _dbContext.Add<TEntity>(entities);
            return Result<IList<TEntity>>.Success(data: entities);
        }

        public Result Update(TEntity entity)
        {
            _dbContext.Update(entity);
            return Result.Success();
        }

        public Result Delete(TEntity entity)
        {
            _dbContext.Delete<TEntity>(entity);
            return Result.Success();
        }

        public void TransactionBegin()
        {
            _dbContext.TransactionBegin();
        }

        public void TransactionCommit()
        {
            _dbContext.TransactionCommit();
        }

        public void TransactionRollback()
        {
            _dbContext.TransactionRollback();
        }
    }
}
