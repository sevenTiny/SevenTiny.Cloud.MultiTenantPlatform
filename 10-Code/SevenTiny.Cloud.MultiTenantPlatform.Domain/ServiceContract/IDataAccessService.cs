using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IDataAccessService
    {
        void Add(int tenantId, BsonDocument bsons);
        List<ObjectData> GetObjectDatasByCondition(int tenantId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize);
    }
}
