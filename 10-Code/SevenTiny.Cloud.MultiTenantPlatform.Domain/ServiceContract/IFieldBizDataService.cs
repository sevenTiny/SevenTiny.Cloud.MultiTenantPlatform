using MongoDB.Bson;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IFieldBizDataService
    {
        List<Dictionary<string, FieldBizData>> ConvertToDictionaryList(int InterfaceFieldId, List<BsonDocument> bsonElements);
        Dictionary<string, FieldBizData> ConvertToDictionary(int InterfaceFieldId, BsonDocument bsonElement);
    }
}
