using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class DataAccessService : IDataAccessService
    {
        readonly MultiTenantDataDbContext db;
        readonly IMetaFieldService metaFieldService;
        readonly IMetaObjectService metaObjectService;
        public DataAccessService(
            MultiTenantDataDbContext _db,
            IMetaFieldService _metaFieldService,
            IMetaObjectService _metaObjectService
            )
        {
            db = _db;
            metaFieldService = _metaFieldService;
            metaObjectService = _metaObjectService;
        }

        public ResultModel Add(string metaObjectCode, BsonDocument bsons)
        {
            if (string.IsNullOrEmpty(metaObjectCode))
                return ResultModel.Error("实体编码不能为空");

            var metaObject = metaObjectService.GetByCode(metaObjectCode);

            if (metaObject == null)
                return ResultModel.Error("没有找到该实体编码对应的实体信息");

            //获取到字段列表
            var metaFields = metaFieldService.GetMetaFieldUpperKeyDicUnDeleted(metaObject.Id);
            for (int i = bsons.ElementCount - 1; i >= 0; i--)
            {
                var item = bsons.GetElement(i);
                string upperKey = item.Name.ToUpperInvariant();
                if (metaFields.ContainsKey(upperKey))
                {
                    //todo:对传入的数据进行校验
                    //...
                    //如果大小写不匹配，则都转化成配置的字段Code形式
                    if (!item.Name.Equals(metaFields[upperKey].Code))
                    {
                        var element = new BsonElement(metaFields[upperKey].Code, item.Value);
                        bsons.RemoveElement(item);
                        bsons.Add(element);
                    }
                }
                else
                {
                    //如果字段不在配置字段中，则不进行添加
                    bsons.RemoveElement(item);
                }
            }

            //预置字段及其默认值
            foreach (var item in metaFieldService.GetPresetFieldBsonElements())
            {
                //如果传入的字段已经有了，那么这里就不预置了
                if (!bsons.Contains(item.Name))
                {
                    bsons.Add(item);
                }
            }

            //补充字段
            bsons.SetElement(new BsonElement("_id", Guid.NewGuid().ToString()));

            db.Add(bsons);

            return ResultModel.Success();
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
