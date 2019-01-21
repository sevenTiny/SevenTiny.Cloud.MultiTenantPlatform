using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository
{
    public class CommonInfoRepository<TEntity> : Repository<TEntity>, ICommonInfoRepository<TEntity> where TEntity : CommonInfo
    {
        public CommonInfoRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public ResultModel Delete(int id)
        {
            dbContext.Delete<TEntity>(t => t.Id.Equals(id));
            return ResultModel.Success();
        }

        public ResultModel LogicDelete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.Deleted;
                dbContext.Update(entity);
            }
            return ResultModel.Success();
        }

        public ResultModel Recover(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.UnDeleted;
                dbContext.Update(entity);
            }
            return ResultModel.Success();
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
