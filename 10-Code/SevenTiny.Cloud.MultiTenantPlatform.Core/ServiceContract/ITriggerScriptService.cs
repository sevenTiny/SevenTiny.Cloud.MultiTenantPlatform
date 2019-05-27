using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.ListView;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
{
    public interface ITriggerScriptService : IMetaObjectManageRepository<TriggerScript>
    {
        Result CompilateAndCheckScript(string script, string applicationCode);
        List<TriggerScript> GetTriggerScriptsUnDeletedByMetaObjectIdAndServiceType(int metaObjectId, int scriptType);
        string GetDefaultMetaObjectTriggerScriptByServiceTypeBefore(int serviceType);
        string GetDefaultMetaObjectTriggerScriptByServiceTypeAfter(int serviceType);
        string GetDefaultDataSourceTriggerScript();

        #region Execute Script
        BsonDocument Run_MetaObject_Interface_Add_Before(int metaObjectId, string interfaceCode, BsonDocument bsonElements);
        BsonDocument Run_MetaObject_Interface_Add_After(int metaObjectId, string interfaceCode, BsonDocument bsonElements);
        List<BsonDocument> Run_MetaObject_Interface_BatchAdd_Before(int metaObjectId, string interfaceCode, List<BsonDocument> bsonElementsList);
        List<BsonDocument> Run_MetaObject_Interface_BatchAdd_After(int metaObjectId, string interfaceCode, List<BsonDocument> bsonElementsList);
        FilterDefinition<BsonDocument> Run_MetaObject_Interface_Update_Before(int metaObjectId, string interfaceCode, FilterDefinition<BsonDocument> condition);
        FilterDefinition<BsonDocument> Run_MetaObject_Interface_Delete_Before(int metaObjectId, string interfaceCode, FilterDefinition<BsonDocument> condition);

        FilterDefinition<BsonDocument> Run_MetaObject_Interface_TableList_Before(int metaObjectId, string interfaceCode, FilterDefinition<BsonDocument> condition);
        TableListComponent Run_MetaObject_Interface_TableList_After(int metaObjectId, string interfaceCode, TableListComponent tableListComponent);

        FilterDefinition<BsonDocument> Run_MetaObject_Interface_SingleObject_Before(int metaObjectId, string interfaceCode, FilterDefinition<BsonDocument> condition);
        SingleObjectComponent Run_MetaObject_Interface_SingleObject_After(int metaObjectId, string interfaceCode, SingleObjectComponent singleObjectComponent);

        FilterDefinition<BsonDocument> Run_MetaObject_Interface_Count_Before(int metaObjectId, string interfaceCode, FilterDefinition<BsonDocument> condition);
        int Run_MetaObject_Interface_Count_After(int metaObjectId, string interfaceCode, int count);

        object Run_DataSource(string interfaceCode, Dictionary<string, object> argumentsDic);
        #endregion
    }
}
