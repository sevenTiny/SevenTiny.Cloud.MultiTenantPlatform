using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Core.DataAccess;
using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.Enum;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Core.Repository
{
    public class CommonInfoRepository<TEntity> : Repository<TEntity>, ICommonInfoRepository<TEntity> where TEntity : CommonInfo
    {
        public CommonInfoRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public new Result<TEntity> Update(TEntity entity)
        {
            entity.ModifyTime = DateTime.Now;

            base.Update(entity);
            return Result<TEntity>.Success();
        }

        public Result<TEntity> Delete(int id)
        {
            dbContext.Delete<TEntity>(t => t.Id.Equals(id));
            return Result<TEntity>.Success();
        }

        public Result<TEntity> LogicDelete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.Deleted;
                dbContext.Update(entity);
            }
            return Result<TEntity>.Success();
        }

        public Result<TEntity> Recover(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.UnDeleted;
                dbContext.Update(entity);
            }
            return Result<TEntity>.Success();
        }

        public TEntity GetById(int id)
            => dbContext.Queryable<TEntity>().Where(t => t.Id.Equals(id)).ToOne();

        public TEntity GetByCode(string code)
            => dbContext.Queryable<TEntity>().Where(t => t.Code.Equals(code)).ToOne();

        public List<TEntity> GetEntitiesDeleted()
            => dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.Deleted).ToList();

        public List<TEntity> GetEntitiesUnDeleted()
            => dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.UnDeleted).ToList();
    }
}
