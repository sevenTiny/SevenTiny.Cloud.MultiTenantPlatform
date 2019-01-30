using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;

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

            //错误信息返回值
            HashSet<string> ErrorInfo = new HashSet<string>();

            //获取到字段列表
            var metaFields = metaFieldService.GetMetaFieldUpperKeyDicUnDeleted(metaObject.Id);
            for (int i = bsons.ElementCount - 1; i >= 0; i--)
            {
                var item = bsons.GetElement(i);
                string upperKey = item.Name.ToUpperInvariant();
                if (metaFields.ContainsKey(upperKey))
                {
                    //检查字段的值是否符合字段类型
                    var checkResult = metaFieldService.CheckAndGetFieldValueByFieldType(metaFields[upperKey], item.Value);
                    if (checkResult.IsSuccess)
                    {
                        //如果大小写不匹配，则都转化成配置的字段Code形式
                        if (!item.Name.Equals(metaFields[upperKey].Code))
                        {
                            bsons.RemoveElement(item);
                            bsons.Add(new BsonElement(metaFields[upperKey].Code, BsonValue.Create(checkResult.Data)));
                        }
                        else
                        {
                            //重置字段的真实类型的值
                            bsons.SetElement(new BsonElement(metaFields[upperKey].Code, BsonValue.Create(checkResult.Data)));
                        }
                    }
                    else
                    {
                        bsons.RemoveElement(item);
                        ErrorInfo.Add($"字段[{item.Name}]传递的值[{item.Value}]不符合字段定义的类型");
                    }
                }
                else
                {
                    //如果字段不在配置字段中，则不进行添加
                    bsons.RemoveElement(item);
                    ErrorInfo.Add($"字段[{item.Name}]不属于对象[{metaObject.Code}({metaObject.Name})]定义的字段");
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
            bsons.SetElement(new BsonElement("MetaObjectCode", metaObjectCode));

            db.Add(bsons);

            return ResultModel.Success($"插入成功，日志：{string.Join(",", ErrorInfo)}");
        }

        public List<ObjectData> GetObjectDatasByCondition(int tenantId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize)
        {
            var bf = Builders<BsonDocument>.Filter;
            condition = bf.And(bf.Eq("tenantId", tenantId), condition);

            var bson = db.QueryListBson<BsonDocument>(condition);
            return null;
        }

        public List<BsonDocument> GetBsonDocumentsByCondition(FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, out int count)
        {
            List<BsonDocument> bson = new List<BsonDocument>();
            if (pageSize == 0)
            {
                bson = db.QueryListBson<BsonDocument>(condition);
                count = bson.Count;
            }
            else
            {
                bson = db.QueryListBson<BsonDocument>(condition, pageIndex, pageSize);
                count = db.QueryCount<BsonDocument>(condition);
            }
            return bson;
        }

        public int GetBsonDocumentCountByCondition(FilterDefinition<BsonDocument> condition)
        {
            return db.QueryCount<BsonDocument>(condition);
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
    }
}
