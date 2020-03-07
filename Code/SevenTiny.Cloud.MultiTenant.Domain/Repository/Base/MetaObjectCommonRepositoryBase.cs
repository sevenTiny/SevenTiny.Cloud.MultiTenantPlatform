using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Repository
{
    internal abstract class MetaObjectCommonRepositoryBase<TEntity> : CommonRepositoryBase<TEntity>, IMetaObjectCommonRepositoryBase<TEntity> where TEntity : MetaObjectCommonBase
    {
        public MetaObjectCommonRepositoryBase(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
        }

        public void LogicDeleteByMetaObjectId(Guid metaObjectId)
        {
            var entity = _dbContext.Queryable<TEntity>().Where(t => t.MetaObjectId == metaObjectId).FirstOrDefault();
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.Deleted;
                _dbContext.Update(entity);
            }
        }

        public List<TEntity> GetListByMetaObjectId(Guid metaObjectId)
            => _dbContext.Queryable<TEntity>().Where(t => t.MetaObjectId == metaObjectId).ToList();

        public List<TEntity> GetListDeletedByMetaObjectId(Guid metaObjectId)
            => _dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.Deleted && t.MetaObjectId == metaObjectId).ToList();

        public List<TEntity> GetListUnDeletedByMetaObjectId(Guid metaObjectId)
            => _dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.UnDeleted && t.MetaObjectId == metaObjectId).ToList();

        /// <summary>
        /// 获取同对象下的编码或者名称相同的数据
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(Guid metaObjectId, Guid id, string code, string name)
            => _dbContext.Queryable<TEntity>().Where(t => t.MetaObjectId == metaObjectId && t.Id != id && (t.Code.Equals(code) || t.Name.Equals(name))).FirstOrDefault();
    }
}
