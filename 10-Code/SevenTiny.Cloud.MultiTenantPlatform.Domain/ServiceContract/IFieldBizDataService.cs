using MongoDB.Bson;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.UIMetaData;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IFieldBizDataService
    {
        List<Dictionary<string, FieldBizData>> ToBizDataDictionaryList(int InterfaceFieldId, List<BsonDocument> bsonElements);
        Dictionary<string, FieldBizData> ToBizDataDictionary(int InterfaceFieldId, BsonDocument bsonElement);
    }
}
