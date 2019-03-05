using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.Core.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Core.UIMetaData.ListView;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
{
    public class DataAccessService : IDataAccessService
    {
        readonly MultiTenantDataDbContext db;
        readonly IMetaFieldService metaFieldService;
        readonly IMetaObjectService metaObjectService;
        readonly IFieldBizDataService fieldBizDataService;
        readonly IFieldListAggregationService fieldListAggregationService;

        public DataAccessService(
            MultiTenantDataDbContext _db,
            IMetaFieldService _metaFieldService,
            IMetaObjectService _metaObjectService,
            IFieldBizDataService _fieldBizDataService,
            IFieldListAggregationService _fieldListAggregationService
            )
        {
            db = _db;
            metaFieldService = _metaFieldService;
            metaObjectService = _metaObjectService;
            fieldBizDataService = _fieldBizDataService;
            fieldListAggregationService = _fieldListAggregationService;
        }

        public Result Add(string metaObjectCode, BsonDocument bsons)
        {
            if (string.IsNullOrEmpty(metaObjectCode))
                return Result.Error("实体编码不能为空");

            var metaObject = metaObjectService.GetByCode(metaObjectCode);

            if (metaObject == null)
                return Result.Error("没有找到该实体编码对应的实体信息");

            return Add(metaObject, bsons);
        }
        public Result Add(MetaObject metaObject, BsonDocument bsons)
        {
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
            //bsons.SetElement(new BsonElement("_id", Guid.NewGuid().ToString()));//id已经补充到预置字段
            bsons.SetElement(new BsonElement("MetaObjectCode", metaObject.Code));

            db.GetCollectionBson(metaObject.Code).InsertOne(bsons);

            return Result.Success($"插入成功，日志：{string.Join(",", ErrorInfo)}");
        }

        public Result BatchAdd(string metaObjectCode, List<BsonDocument> bsons)
        {
            if (string.IsNullOrEmpty(metaObjectCode))
                return Result.Error("实体编码不能为空");

            var metaObject = metaObjectService.GetByCode(metaObjectCode);

            if (metaObject == null)
                return Result.Error("没有找到该实体编码对应的实体信息");

            return BatchAdd(metaObject, bsons);
        }
        public Result BatchAdd(MetaObject metaObject, List<BsonDocument> bsonsList)
        {
            if (bsonsList != null && bsonsList.Any())
            {
                List<BsonDocument> insertBsonsList = new List<BsonDocument>();

                //获取到字段列表
                var metaFields = metaFieldService.GetMetaFieldUpperKeyDicUnDeleted(metaObject.Id);

                for (int j = 0; j < bsonsList.Count; j++)
                {
                    var bsons = bsonsList[j];
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
                    //bsons.SetElement(new BsonElement("_id", Guid.NewGuid().ToString()));//id已经补充到预置字段
                    bsons.SetElement(new BsonElement("MetaObjectCode", metaObject.Code));

                    insertBsonsList.Add(bsons);
                }
                db.GetCollectionBson(metaObject.Code).InsertMany(insertBsonsList);
            }
            return Result.Success($"插入成功");
        }

        public Result Update(string metaObjectCode, FilterDefinition<BsonDocument> condition, BsonDocument bsons)
        {
            if (string.IsNullOrEmpty(metaObjectCode))
                return Result.Error("实体编码不能为空");

            var metaObject = metaObjectService.GetByCode(metaObjectCode);

            if (metaObject == null)
                return Result.Error("没有找到该实体编码对应的实体信息");

            return Update(metaObject, condition, bsons);
        }
        public Result Update(int metaObjectId, FilterDefinition<BsonDocument> condition, BsonDocument bsons)
        {
            var metaObject = metaObjectService.GetById(metaObjectId);

            if (metaObject == null)
                return Result.Error("没有找到该实体编码对应的实体信息");

            return Update(metaObject, condition, bsons);
        }
        public Result Update(MetaObject metaObject, FilterDefinition<BsonDocument> condition, BsonDocument bsons)
        {
            //错误信息返回值
            HashSet<string> ErrorInfo = new HashSet<string>();

            if (bsons != null && bsons.Any())
            {
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

                var collection = db.GetCollectionBson(metaObject.Code);
                var bu = Builders<BsonDocument>.Update;
                foreach (var item in bsons)
                {
                    collection.UpdateMany(condition, bu.Set(item.Name, item.Value));
                }
            }
            return Result.Success($"修改成功，日志：{string.Join(",", ErrorInfo)}");
        }

        public Result Delete(string metaObjectCode, FilterDefinition<BsonDocument> condition)
        {
            if (string.IsNullOrEmpty(metaObjectCode))
                return Result.Error("实体编码不能为空");

            var metaObject = metaObjectService.GetByCode(metaObjectCode);

            if (metaObject == null)
                return Result.Error("没有找到该实体编码对应的实体信息");

            return Delete(metaObject, condition);
        }
        public Result Delete(int metaObjectId, FilterDefinition<BsonDocument> condition)
        {
            var metaObject = metaObjectService.GetById(metaObjectId);

            if (metaObject == null)
                return Result.Error("没有找到该实体编码对应的实体信息");

            return Delete(metaObject, condition);
        }
        public Result Delete(MetaObject metaObject, FilterDefinition<BsonDocument> condition)
        {
            db.GetCollectionBson(metaObject.Code).DeleteMany(condition);

            return Result.Success($"删除成功");
        }

        public BsonDocument Get(string metaObjectCode, FilterDefinition<BsonDocument> condition)
        {
            if (string.IsNullOrEmpty(metaObjectCode))
                throw new Exception("实体编码不能为空");
            var metaObject = metaObjectService.GetByCode(metaObjectCode);
            if (metaObject == null)
                throw new Exception("没有找到该实体编码对应的实体信息");

            return Get(metaObject, condition);
        }
        public BsonDocument Get(int metaObjectId, FilterDefinition<BsonDocument> condition)
        {
            var metaObject = metaObjectService.GetById(metaObjectId);
            if (metaObject == null)
                throw new Exception("没有找到该实体编码对应的实体信息");

            return Get(metaObject, condition);
        }
        public BsonDocument Get(MetaObject metaObject, FilterDefinition<BsonDocument> condition)
        {
            return db.GetCollectionBson(metaObject.Code).Find<BsonDocument>(condition)?.FirstOrDefault();
        }
        public SingleObjectComponent GetSingleObjectComponent(int metaObjectId, int InterfaceFieldId, FilterDefinition<BsonDocument> condition)
        {
            var document = Get(metaObjectId, condition);
            SingleObjectComponent singleObjectComponent = new SingleObjectComponent
            {
                BizData = fieldBizDataService.ToBizDataDictionary(InterfaceFieldId, document),
                ColunmDatas = fieldListAggregationService.GetColumnDataByFieldListId(InterfaceFieldId).OrderBy(t => t.CmpData.ShowIndex).ToList()
            };
            return singleObjectComponent;
        }

        public BsonDocument GetById(string metaObjectCode, string _id)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", _id);
            return Get(metaObjectCode, filter);
        }
        public List<BsonDocument> GetByIds(string metaObjectCode, string[] _ids, SortDefinition<BsonDocument> sort)
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.In("_id", _ids);
            return GetList(metaObjectCode, filter, sort);
        }
        public List<BsonDocument> GetList(string metaObjectCode, FilterDefinition<BsonDocument> condition, SortDefinition<BsonDocument> sort)
        {
            return GetList(metaObjectCode, condition, 0, 0, sort);
        }

        public List<BsonDocument> GetList(int metaObjectId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort)
        {
            var metaObject = metaObjectService.GetById(metaObjectId);
            if (metaObject == null)
                throw new Exception("没有找到该实体编码对应的实体信息");

            return GetList(metaObject, condition, pageIndex, pageSize, sort);
        }
        public List<BsonDocument> GetList(string metaObjectCode, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort)
        {
            if (string.IsNullOrEmpty(metaObjectCode))
                throw new Exception("实体编码不能为空");
            var metaObject = metaObjectService.GetByCode(metaObjectCode);
            if (metaObject == null)
                throw new Exception("没有找到该实体编码对应的实体信息");

            return GetList(metaObject, condition, pageIndex, pageSize, sort);
        }
        public List<BsonDocument> GetList(MetaObject metaObject, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort)
        {
            List<BsonDocument> bson = new List<BsonDocument>();
            if (pageSize == 0)
            {
                //无条件默认取10000条
                pageSize = 10000;
            }

            //这里等升级完包之后全部替换成orm的方法
            int skipSize = (pageIndex - 1) > 0 ? ((pageIndex - 1) * pageSize) : 0;
            bson = db.GetCollectionBson(metaObject.Code).Find(condition).Skip(skipSize).Limit(pageSize).Sort(sort).ToList();
            return bson;
        }

        public List<BsonDocument> GetList(int metaObjectId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count)
        {
            var metaObject = metaObjectService.GetById(metaObjectId);
            if (metaObject == null)
                throw new Exception("没有找到该实体编码对应的实体信息");
            return GetList(metaObject, condition, pageIndex, pageSize, sort, out count);
        }
        public List<BsonDocument> GetList(string metaObjectCode, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count)
        {
            if (string.IsNullOrEmpty(metaObjectCode))
                throw new Exception("实体编码不能为空");
            var metaObject = metaObjectService.GetByCode(metaObjectCode);
            if (metaObject == null)
                throw new Exception("没有找到该实体编码对应的实体信息");
            return GetList(metaObject, condition, pageIndex, pageSize, sort, out count);
        }
        public List<BsonDocument> GetList(MetaObject metaObject, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count)
        {
            List<BsonDocument> bson = GetList(metaObject.Code, condition, pageIndex, pageSize, sort);

            if (pageSize == 0)
                count = bson?.Count ?? 0;
            else
                count = Convert.ToInt32(db.GetCollectionBson(metaObject.Code).Count(condition));

            return bson;
        }
        public TableListComponent GetTableListComponent(int metaObjectId, int InterfaceFieldId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count)
        {
            var documents = GetList(metaObjectId, condition, pageIndex, pageSize, sort, out count);
            TableListComponent tableListComponent = new TableListComponent
            {
                BizData = fieldBizDataService.ToBizDataDictionaryList(InterfaceFieldId, documents),
                BizDataTotalCount = count,
                Columns = fieldListAggregationService.GetColumnDataByFieldListId(InterfaceFieldId).OrderBy(t => t.CmpData.ShowIndex).ToList()
            };

            if (pageSize != 0)
                tableListComponent.PageCount = count / pageSize;

            return tableListComponent;
        }

        public int GetCount(int metaObjectId, FilterDefinition<BsonDocument> condition)
        {
            var metaObject = metaObjectService.GetById(metaObjectId);
            if (metaObject == null)
                throw new Exception("没有找到该实体编码对应的实体信息");

            return GetCount(metaObject, condition);
        }
        public int GetCount(string metaObjectCode, FilterDefinition<BsonDocument> condition)
        {
            if (string.IsNullOrEmpty(metaObjectCode))
                throw new Exception("实体编码不能为空");
            var metaObject = metaObjectService.GetByCode(metaObjectCode);
            if (metaObject == null)
                throw new Exception("没有找到该实体编码对应的实体信息");

            return GetCount(metaObject, condition);
        }
        public int GetCount(MetaObject metaObject, FilterDefinition<BsonDocument> condition)
        {
            return Convert.ToInt32(db.GetCollectionBson(metaObject.Code).Count(condition));
        }
    }
}
