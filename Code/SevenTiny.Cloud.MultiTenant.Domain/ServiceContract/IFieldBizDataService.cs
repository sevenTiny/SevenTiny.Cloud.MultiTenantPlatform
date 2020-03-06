using MongoDB.Bson;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IFieldBizDataService
    {
        List<Dictionary<string, FieldBizData>> ToBizDataDictionaryList(QueryPiplineContext queryPiplineContext, List<BsonDocument> bsonElements);
        Dictionary<string, FieldBizData> ToBizDataDictionary(QueryPiplineContext queryPiplineContext, BsonDocument bsonElement);
    }
}
