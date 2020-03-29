using SevenTiny.Bantina;
using SevenTiny.Cloud.Account.Core.DataAccess;
using System.Collections.Generic;

namespace SevenTiny.Cloud.Account.Core.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public Repository(AccountDbContext accountDbContext)
        {
            dbContext = accountDbContext;
        }

        AccountDbContext dbContext;

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
