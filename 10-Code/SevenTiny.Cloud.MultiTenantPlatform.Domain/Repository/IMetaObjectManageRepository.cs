using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository
{
    public interface IMetaObjectManageRepository<TEntity> : ICommonInfoRepository<TEntity> where TEntity : MetaObjectManageInfo
    {
        void DeleteByMetaObjectId(int metaObjectId);
        List<TEntity> GetEntitiesDeletedByMetaObjectId(int metaObjectId);
        List<TEntity> GetEntitiesUnDeletedByMetaObjectId(int metaObjectId);
    }
}
