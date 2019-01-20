using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class FieldAggregationService : Repository<FieldAggregation>, IFieldAggregationService
    {
        public FieldAggregationService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        readonly MultiTenantPlatformDbContext dbContext;

        public List<FieldAggregation> GetByInterfaceFieldId(int interfaceFieldId)
        {
            return dbContext.QueryList<FieldAggregation>(t => t.InterfaceFieldId == interfaceFieldId);
        }

        public void DeleteByMetaFieldId(int metaFieldId)
        {
            dbContext.Delete<FieldAggregation>(t => t.MetaFieldId == metaFieldId);
        }
    }
}
