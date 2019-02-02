using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
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

        public List<TEntity> GetEntitiesByMetaObjectId(int metaObjectId)
                  => dbContext.QueryList<TEntity>(t =>t.MetaObjectId == metaObjectId);

        public List<TEntity> GetEntitiesDeletedByMetaObjectId(int metaObjectId)
            => dbContext.QueryList<TEntity>(t => t.IsDeleted == (int)IsDeleted.Deleted && t.MetaObjectId == metaObjectId);

        public List<TEntity> GetEntitiesUnDeletedByMetaObjectId(int metaObjectId)
        {
            return dbContext.QueryList<TEntity>(t => t.IsDeleted == (int)IsDeleted.UnDeleted && t.MetaObjectId == metaObjectId);
        }
        
        /// <summary>
        /// 检查是否有相同名称的编码或名称
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ResultModel CheckSameCodeOrName(int metaObjectId, TEntity entity)
        {
            var obj = dbContext.QueryOne<TEntity>(t => t.MetaObjectId == metaObjectId && t.Id != entity.Id && (t.Code.Equals(entity.Code) || t.Name.Equals(entity.Name)));
            if (obj != null)
            {
                if (obj.Code.Equals(entity.Code))
                    return ResultModel.Error($"编码[{obj.Code}]已存在", entity);
                else if (obj.Name.Equals(entity.Name))
                    return ResultModel.Error($"名称[{obj.Name}]已存", entity);
            }
            return ResultModel.Success();
        }
    }
}
