using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ValueObject;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.ListView;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
{
    public interface IDataAccessService
    {
        Result Add(string metaObjectCode, BsonDocument bsons);
        Result Add(MetaObject metaObject, BsonDocument bsons);

        Result BatchAdd(string metaObjectCode, List<BsonDocument> bsons);
        Result BatchAdd(MetaObject metaObject, List<BsonDocument> bsons);

        Result Update(int metaObjectId, FilterDefinition<BsonDocument> condition, BsonDocument bsons);
        Result Update(string metaObjectCode, FilterDefinition<BsonDocument> condition, BsonDocument bsons);
        Result Update(MetaObject metaObject, FilterDefinition<BsonDocument> condition, BsonDocument bsons);

        Result Delete(int metaObjectId, FilterDefinition<BsonDocument> condition);
        Result Delete(string metaObjectCode, FilterDefinition<BsonDocument> condition);
        Result Delete(MetaObject metaObject, FilterDefinition<BsonDocument> condition);

        BsonDocument Get(string metaObjectCode, FilterDefinition<BsonDocument> condition, string[] columns = null);
        BsonDocument Get(int metaObjectId, FilterDefinition<BsonDocument> condition, string[] columns = null);
        BsonDocument Get(MetaObject metaObject, FilterDefinition<BsonDocument> condition, string[] columns = null);
        SingleObjectComponent GetSingleObjectComponent(QueryPiplineContext queryPiplineContext, FilterDefinition<BsonDocument> condition);

        BsonDocument GetById(string metaObjectCode, string _id);
        List<BsonDocument> GetByIds(string metaObjectCode, string[] _ids, SortDefinition<BsonDocument> sort, string[] columns = null);

        List<BsonDocument> GetList(int metaObjectId, FilterDefinition<BsonDocument> condition, SortDefinition<BsonDocument> sort, string[] columns = null);
        List<BsonDocument> GetList(string metaObjectCode, FilterDefinition<BsonDocument> condition, SortDefinition<BsonDocument> sort, string[] columns = null);

        List<BsonDocument> GetList(string metaObjectCode, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, string[] columns = null);
        List<BsonDocument> GetList(int metaObjectId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, string[] columns = null);
        List<BsonDocument> GetList(MetaObject metaObject, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, string[] columns = null);

        List<BsonDocument> GetList(int metaObjectId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count, string[] columns = null);
        List<BsonDocument> GetList(string metaObjectCode, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count, string[] columns = null);
        List<BsonDocument> GetList(MetaObject metaObject, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count, string[] columns = null);
        TableListComponent GetTableListComponent(QueryPiplineContext queryPiplineContext, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count);

        int GetCount(int metaObjectId, FilterDefinition<BsonDocument> condition);
        int GetCount(string metaObjectCode, FilterDefinition<BsonDocument> condition);
        int GetCount(MetaObject metaObject, FilterDefinition<BsonDocument> condition);
    }
}
