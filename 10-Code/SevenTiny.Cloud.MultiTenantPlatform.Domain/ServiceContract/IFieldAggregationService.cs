using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IFieldAggregationService : IRepository<FieldAggregation>
    {
        List<FieldAggregation> GetByInterfaceFieldId(int interfaceFieldId);
        void DeleteByMetaFieldId(int metaFieldId);
    }
}
