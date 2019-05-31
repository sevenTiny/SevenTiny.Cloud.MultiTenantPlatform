using MongoDB.Bson;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
{
    public interface IFieldBizDataService
    {
        List<Dictionary<string, FieldBizData>> ToBizDataDictionaryList(int metaObjectId, int InterfaceFieldId, List<BsonDocument> bsonElements);
        Dictionary<string, FieldBizData> ToBizDataDictionary(int metaObjectId, int InterfaceFieldId, BsonDocument bsonElement);
    }
}
