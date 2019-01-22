using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class DataAccessService : IDataAccessService
    {
        readonly MultiTenantDataDbContext db;
        readonly IMetaFieldService metaFieldService;
        public DataAccessService(
            MultiTenantDataDbContext _db,
            IMetaFieldService _metaFieldService
            )
        {
            db = _db;
            metaFieldService = _metaFieldService;
        }

        public void Add(int tenantId, BsonDocument bsons)
        {
            BsonDocument doc = new BsonDocument();
            //补充字段及其默认值
            bsons.AddRange(metaFieldService.GetPresetFieldDic());
            //存入字段
            doc.Add(new BsonElement("MetaFields", bsons));
            //补充字段
            doc.Add(new BsonElement("_id", Guid.NewGuid().ToString()));
            doc.Add(new BsonElement("TenantId", tenantId));
            db.Add<BsonDocument>(doc);
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
