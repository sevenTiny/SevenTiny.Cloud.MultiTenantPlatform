using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.ServiceContract
{
    public interface ITriggerScriptEngineService
    {
        Tuple<bool, string> CompilationAndCheckScript(string script);

        BsonDocument AddBefore(int metaObjectId, string operateCode, BsonDocument bsonElements);
        List<BsonDocument> BatchAddBefore(int metaObjectId, string operateCode, List<BsonDocument> bsonElementsList);
        FilterDefinition<BsonDocument> UpdateBefore(int metaObjectId, string operateCode, FilterDefinition<BsonDocument> condition);
        FilterDefinition<BsonDocument> DeleteBefore(int metaObjectId, string operateCode, FilterDefinition<BsonDocument> condition);

        FilterDefinition<BsonDocument> TableListBefore(int metaObjectId, string operateCode, FilterDefinition<BsonDocument> condition);
        TableListComponent TableListAfter(int metaObjectId, string operateCode, TableListComponent tableListComponent);

        FilterDefinition<BsonDocument> SingleObjectBefore(int metaObjectId, string operateCode, FilterDefinition<BsonDocument> condition);
        SingleObjectComponent SingleObjectAfter(int metaObjectId, string operateCode, SingleObjectComponent singleObjectComponent);

        FilterDefinition<BsonDocument> CountBefore(int metaObjectId, string operateCode, FilterDefinition<BsonDocument> condition);
        int CountAfter(int metaObjectId, string operateCode, int count);

        object TriggerScriptDataSource(string operateCode, Dictionary<string, object> argumentsDic, string script);
    }
}
