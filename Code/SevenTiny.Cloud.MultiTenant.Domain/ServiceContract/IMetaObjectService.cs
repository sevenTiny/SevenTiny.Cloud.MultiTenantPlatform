using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IMetaObjectService : ICommonServiceBase<MetaObject>
    {
        Result Add(Guid applicationId, string applicationCode, MetaObject metaObject);
    }
}
