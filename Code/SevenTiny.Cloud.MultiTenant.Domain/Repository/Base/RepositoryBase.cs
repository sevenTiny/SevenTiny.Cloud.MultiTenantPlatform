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

        public Result<TEntity> Add(TEntity entity)
        {
            _dbContext.Add(entity);
            return Result<TEntity>.Success();
        }

        public Result<IList<TEntity>> BatchAdd(IList<TEntity> entities)
        {
            _dbContext.Add<TEntity>(entities);
            return Result<IList<TEntity>>.Success();
        }

        public Result<TEntity> Update(TEntity entity)
        {
            _dbContext.Update(entity);
            return Result<TEntity>.Success();
        }

        public Result<TEntity> Delete(TEntity entity)
        {
            _dbContext.Delete<TEntity>(entity);
            return Result<TEntity>.Success();
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
