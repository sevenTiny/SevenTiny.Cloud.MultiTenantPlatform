using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IDataAccessService
    {
        ResultModel Add(string metaObjectCode, BsonDocument bsons);
        List<ObjectData> GetObjectDatasByCondition(int tenantId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize);
        List<BsonDocument> GetBsonDocumentsByCondition(FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, out int count);
        int GetBsonDocumentCountByCondition(FilterDefinition<BsonDocument> condition);
    }
}
