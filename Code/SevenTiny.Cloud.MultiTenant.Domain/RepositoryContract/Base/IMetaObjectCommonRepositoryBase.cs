using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract
{
    internal interface IMetaObjectCommonRepositoryBase<TEntity> : ICommonRepositoryBase<TEntity> where TEntity : MetaObjectCommonBase
    {
        void LogicDeleteByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetListByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetListDeletedByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetListUnDeletedByMetaObjectId(Guid metaObjectId);
        TEntity GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(Guid metaObjectId, Guid id, string code, string name);
    }
}
