using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IMetaObjectService : ICommonServiceBase<MetaObject>
    {
        Result Add(Guid applicationId, string applicationCode, MetaObject metaObject);
        List<MetaObject> GetMetaObjectListUnDeletedByApplicationId(Guid applicationId);
        List<MetaObject> GetMetaObjectListDeletedByApplicationId(Guid applicationId);
        MetaObject GetMetaObjectByCodeOrNameWithApplicationId(Guid applicationId, string code, string name);
        MetaObject GetMetaObjectByCodeAndApplicationId(Guid applicationId, string code);
    }
}
