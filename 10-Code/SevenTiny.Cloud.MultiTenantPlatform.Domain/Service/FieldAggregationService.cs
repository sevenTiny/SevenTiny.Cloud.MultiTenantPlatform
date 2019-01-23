using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class FieldAggregationService : Repository<FieldAggregation>, IFieldAggregationService
    {
        public FieldAggregationService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IMetaFieldService _metaFieldService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            metaFieldService = _metaFieldService;
        }

        readonly MultiTenantPlatformDbContext dbContext;
        readonly IMetaFieldService metaFieldService;

        public List<FieldAggregation> GetByInterfaceFieldId(int interfaceFieldId)
        {
            return dbContext.QueryList<FieldAggregation>(t => t.InterfaceFieldId == interfaceFieldId);
        }

        public void DeleteByMetaFieldId(int metaFieldId)
        {
            dbContext.Delete<FieldAggregation>(t => t.MetaFieldId == metaFieldId);
        }

        public List<MetaField> GetMetaFieldsByInterfaceFieldId(int interfaceFieldId)
        {
            var fieldAggregationList = GetByInterfaceFieldId(interfaceFieldId);
            if (fieldAggregationList != null && fieldAggregationList.Any())
            {
                var fieldIds = fieldAggregationList.Select(t => t.MetaFieldId).ToArray();
                return metaFieldService.GetByIds(fieldIds);
            }
            return null;
        }

        public Dictionary<string, MetaField> GetMetaFieldsDicByInterfaceFieldId(int interfaceFieldId)
        {
            return GetMetaFieldsByInterfaceFieldId(interfaceFieldId)?.ToDictionary(t => t.Code, t => t);
        }
    }
}
