using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IMetaObjectCommonServiceBase<TEntity> : ICommonServiceBase<TEntity> where TEntity : MetaObjectCommonBase
    {
        Result<TEntity> CheckHasSameCodeOrNameWithSameMetaObjectId(Guid metaObjectId, TEntity entity);
    }
}
