using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Core.DataAccess;
using SevenTiny.Cloud.MultiTenant.Core.Entity;
using SevenTiny.Cloud.MultiTenant.Core.Enum;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Core.Repository
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

        public List<TEntity> GetEntitiesByMetaObjectId(int metaObjectId)
            => dbContext.Queryable<TEntity>().Where(t => t.MetaObjectId == metaObjectId).ToList();

        public List<TEntity> GetEntitiesDeletedByMetaObjectId(int metaObjectId)
            => dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.Deleted && t.MetaObjectId == metaObjectId).ToList();

        public List<TEntity> GetEntitiesUnDeletedByMetaObjectId(int metaObjectId)
            => dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.UnDeleted && t.MetaObjectId == metaObjectId).ToList();

        /// <summary>
        /// 检查是否有相同名称的编码或名称
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Result<TEntity> CheckSameCodeOrName(int metaObjectId, TEntity entity)
        {
            var obj = dbContext.Queryable<TEntity>().Where(t => t.MetaObjectId == metaObjectId && t.Id != entity.Id && (t.Code.Equals(entity.Code) || t.Name.Equals(entity.Name))).ToOne();
            if (obj != null)
            {
                if (obj.Code.Equals(entity.Code))
                    return Result<TEntity>.Error($"编码[{obj.Code}]已存在", entity);
                else if (obj.Name.Equals(entity.Name))
                    return Result<TEntity>.Error($"名称[{obj.Name}]已存在", entity);
            }
            return Result<TEntity>.Success();
        }
    }
}
