using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository
{
    public class MetaObjectManageRepository<TEntity> : CommonInfoRepository<TEntity>, IMetaObjectManageRepository<TEntity> where TEntity : MetaObjectManageInfo
    {
        public MetaObjectManageRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public void DeleteByMetaObjectId(int metaObjectId)
        {
            dbContext.Delete<TEntity>(t => t.MetaObjectId == metaObjectId);
        }

        public List<TEntity> GetEntitiesDeletedByMetaObjectId(int metaObjectId)
            => dbContext.QueryList<TEntity>(t => t.IsDeleted == (int)IsDeleted.Deleted && t.MetaObjectId == metaObjectId);

        public List<TEntity> GetEntitiesUnDeletedByMetaObjectId(int metaObjectId)
        {
            return dbContext.QueryList<TEntity>(t => t.IsDeleted == (int)IsDeleted.UnDeleted && t.MetaObjectId == metaObjectId);
        }
    }
}
