using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.CloudModel;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Application.Service
{
    public class MultitenantDataService : IMultitenantDataService
    {
        public void Add(int tenantId, BsonDocument bsons)
        {
            bsons.Add(new BsonElement("tenantId", tenantId));
            using (var db = new MultiTenantDataDbContext())
            {
                db.Add<BsonDocument>(bsons);
            }
        }

        public List<ObjectData> GetObjectDatasByCondition(int tenantId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize)
        {
            var bf = Builders<BsonDocument>.Filter;
            condition = bf.And(bf.Eq("tenantId", tenantId), condition);

            using (var db = new MultiTenantDataDbContext())
            {
                var bson = db.QueryListBson<BsonDocument>(condition);
            }
            return null;
        }
    }
}
