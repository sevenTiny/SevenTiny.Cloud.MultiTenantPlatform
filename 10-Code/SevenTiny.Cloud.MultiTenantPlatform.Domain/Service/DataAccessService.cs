using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class DataAccessService : IDataAccessService
    {
        readonly MultiTenantDataDbContext db;
        public DataAccessService(MultiTenantDataDbContext _db)
        {
            db = _db;
        }

        public void Add(int tenantId, BsonDocument bsons)
        {
            bsons.Add(new BsonElement("tenantId", tenantId));
            db.Add<BsonDocument>(bsons);
        }

        public List<ObjectData> GetObjectDatasByCondition(int tenantId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize)
        {
            var bf = Builders<BsonDocument>.Filter;
            condition = bf.And(bf.Eq("tenantId", tenantId), condition);

            var bson = db.QueryListBson<BsonDocument>(condition);
            return null;
        }

        public void Insert(ObjectData objectData)
        {
            using (var fact = new MultiTenantDataDbContext())
            {
                //todu : 强类型对象转bson 的方法
                string json = JsonConvert.SerializeObject(objectData);
                BsonDocument bson = BsonDocument.Parse(json);
                //bson["_id"] = objectData.Id;
                fact.Add<ObjectData>(bson);
            }
        }

        public void Update(ObjectData objectData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 通过filter查询数据并转成Json字符串
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public string QueryJsonByFilter(FilterDefinition<BsonDocument> filter)
        {
            using (var fact = new MultiTenantDataDbContext())
            {
                List<BsonDocument> documents = fact.QueryListBson<ObjectData>(filter);
                return documents?.ToJson();
            }
        }
    }
}
