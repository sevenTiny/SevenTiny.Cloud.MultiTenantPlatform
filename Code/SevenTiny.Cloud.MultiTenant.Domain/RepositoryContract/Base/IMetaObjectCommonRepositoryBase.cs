using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract
{
    public interface IMetaObjectCommonRepositoryBase<TEntity> : ICommonRepositoryBase<TEntity> where TEntity : MetaObjectCommonBase
    {
        void LogicDeleteByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetEntitiesByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetEntitiesDeletedByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetEntitiesUnDeletedByMetaObjectId(Guid metaObjectId);
        TEntity GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(Guid metaObjectId, Guid id, string code, string name);
    }
}
