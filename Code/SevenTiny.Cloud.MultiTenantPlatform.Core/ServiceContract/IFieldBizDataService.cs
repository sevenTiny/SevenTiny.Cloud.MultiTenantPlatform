using MongoDB.Bson;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ValueObject;
using SevenTiny.Cloud.MultiTenantPlatform.UI.UIMetaData;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
{
    public interface IFieldBizDataService
    {
        List<Dictionary<string, FieldBizData>> ToBizDataDictionaryList(QueryPiplineContext queryPiplineContext, List<BsonDocument> bsonElements);
        Dictionary<string, FieldBizData> ToBizDataDictionary(QueryPiplineContext queryPiplineContext, BsonDocument bsonElement);
    }
}
