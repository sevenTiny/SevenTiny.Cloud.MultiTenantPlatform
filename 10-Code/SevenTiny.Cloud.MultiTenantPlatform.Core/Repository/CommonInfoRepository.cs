using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Repository
{
    public class CommonInfoRepository<TEntity> : Repository<TEntity>, ICommonInfoRepository<TEntity> where TEntity : CommonInfo
    {
        public CommonInfoRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public new Result Update(TEntity entity)
        {
            entity.ModifyTime = DateTime.Now;

            base.Update(entity);
            return Result.Success();
        }

        public Result Delete(int id)
        {
            dbContext.Delete<TEntity>(t => t.Id.Equals(id));
            return Result.Success();
        }

        public Result LogicDelete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.Deleted;
                dbContext.Update(entity);
            }
            return Result.Success();
        }

        public Result Recover(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.UnDeleted;
                dbContext.Update(entity);
            }
            return Result.Success();
        }

        public TEntity GetById(int id)
            => dbContext.QueryOne<TEntity>(t => t.Id.Equals(id));

        public TEntity GetByCode(string code)
            => dbContext.QueryOne<TEntity>(t => t.Code.Equals(code));

        public List<TEntity> GetEntitiesDeleted()
            => dbContext.QueryList<TEntity>(t => t.IsDeleted == (int)IsDeleted.Deleted);

        public List<TEntity> GetEntitiesUnDeleted()
            => dbContext.QueryList<TEntity>(t => t.IsDeleted == (int)IsDeleted.UnDeleted);
    }
}
