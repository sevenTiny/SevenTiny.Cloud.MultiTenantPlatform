using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IMetaObjectCommonServiceBase<TEntity> : ICommonServiceBase<TEntity> where TEntity : MetaObjectCommonBase
    {
        Result CheckHasSameCodeOrNameWithSameMetaObjectId(Guid metaObjectId, TEntity entity);
        void LogicDeleteByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetListByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetListDeletedByMetaObjectId(Guid metaObjectId);
        List<TEntity> GetListUnDeletedByMetaObjectId(Guid metaObjectId);
        TEntity GetByCodeOrNameWithSameMetaObjectIdAndNotSameId(Guid metaObjectId, Guid id, string code, string name);
    }
}
