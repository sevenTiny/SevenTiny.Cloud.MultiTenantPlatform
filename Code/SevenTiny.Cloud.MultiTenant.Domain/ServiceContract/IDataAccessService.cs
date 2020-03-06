using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.ServiceContract
{
    public interface IDataAccessService
    {
        Result Add(int tenantId, string metaObjectCode, BsonDocument bsons);
        Result Add(int tenantId, MetaObject metaObject, BsonDocument bsons);

        Result BatchAdd(int tenantId, string metaObjectCode, List<BsonDocument> bsons);
        Result BatchAdd(int tenantId, MetaObject metaObject, List<BsonDocument> bsons);

        Result Update(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition, BsonDocument bsons);
        Result Update(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition, BsonDocument bsons);
        Result Update(int tenantId, MetaObject metaObject, FilterDefinition<BsonDocument> condition, BsonDocument bsons);

        Result Delete(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition);
        Result Delete(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition);
        Result Delete(int tenantId, MetaObject metaObject, FilterDefinition<BsonDocument> condition);

        BsonDocument Get(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition, string[] columns = null);
        BsonDocument Get(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition, string[] columns = null);
        BsonDocument Get(int tenantId, MetaObject metaObject, FilterDefinition<BsonDocument> condition, string[] columns = null);
        SingleObjectComponent GetSingleObjectComponent(QueryPiplineContext queryPiplineContext, FilterDefinition<BsonDocument> condition);

        BsonDocument GetById(int tenantId, string metaObjectCode, string _id);
        List<BsonDocument> GetByIds(int tenantId, string metaObjectCode, string[] _ids, SortDefinition<BsonDocument> sort, string[] columns = null);

        List<BsonDocument> GetList(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition, SortDefinition<BsonDocument> sort, string[] columns = null);
        List<BsonDocument> GetList(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition, SortDefinition<BsonDocument> sort, string[] columns = null);

        List<BsonDocument> GetList(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, string[] columns = null);
        List<BsonDocument> GetList(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, string[] columns = null);
        List<BsonDocument> GetList(int tenantId, MetaObject metaObject, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, string[] columns = null);

        List<BsonDocument> GetList(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count, string[] columns = null);
        List<BsonDocument> GetList(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count, string[] columns = null);
        List<BsonDocument> GetList(int tenantId, MetaObject metaObject, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count, string[] columns = null);
        TableListComponent GetTableListComponent(QueryPiplineContext queryPiplineContext, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count);

        int GetCount(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition);
        int GetCount(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition);
        int GetCount(int tenantId, MetaObject metaObject, FilterDefinition<BsonDocument> condition);
    }
}
