using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IMetaFieldService : IRepository<MetaField>
    {
        void DeleteByMetaObjectId(int metaObjectId);
        List<MetaField> GetMetaFieldsUnDeletedByMetaObjectId(int metaObjectId);
        List<MetaField> GetMetaFieldsDeletedByMetaObjectId(int metaObjectId);
        ResultModel CheckSameCodeOrName(int metaObjectId, MetaField metaField);
    }
}
