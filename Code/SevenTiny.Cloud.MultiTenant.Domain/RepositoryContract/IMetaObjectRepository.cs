using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract
{
    internal interface IMetaObjectRepository : ICommonRepositoryBase<MetaObject>
    {
        List<MetaObject> GetMetaObjectListUnDeletedByApplicationId(Guid applicationId);
        List<MetaObject> GetMetaObjectListDeletedByApplicationId(Guid applicationId);
        MetaObject GetMetaObjectByCodeOrNameWithApplicationId(Guid applicationId, string code, string name);
        MetaObject GetMetaObjectByCodeAndApplicationId(Guid applicationId, string code);
    }
}
