using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository
{
    public class CommonInfoRepository<TEntity> : IRepository<TEntity> where TEntity : CommonInfo
    {
        public CommonInfoRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public void Add(TEntity entity)
            => dbContext.Add(entity);

        public void Update(TEntity entity)
            => dbContext.Update(t => t.Id == entity.Id, entity);

        public void Delete(int id)
            => dbContext.Delete<TEntity>(t => t.Id.Equals(id));

        public void LogicDelete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.Deleted;
                dbContext.Update(t => t.Id == id, entity);
            }
        }

        public void Recover(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.UnDeleted;
                dbContext.Update(t => t.Id == id, entity);
            }
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
