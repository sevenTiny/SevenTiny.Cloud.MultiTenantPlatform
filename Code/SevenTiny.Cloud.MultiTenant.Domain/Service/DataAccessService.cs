//using MongoDB.Bson;
//using MongoDB.Driver;
//using SevenTiny.Bantina;
//using SevenTiny.Cloud.MultiTenant.Infrastructure.Const;
//using SevenTiny.Cloud.MultiTenant.Domain.Entity;
//using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
//using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
//using SevenTiny.Cloud.MultiTenant.UI.DataAccess;
//using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData.ListView;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace SevenTiny.Cloud.MultiTenant.Domain.Service
//{
//    internal class DataAccessService : IDataAccessService
//    {
//        readonly MultiTenantDataDbContext db;
//        readonly IMetaFieldService metaFieldService;
//        readonly IMetaObjectService metaObjectService;
//        readonly IFieldBizDataService fieldBizDataService;
//        readonly IConfigFieldService fieldListAggregationService;

//        public DataAccessService(
//            MultiTenantDataDbContext _db,
//            IMetaFieldService _metaFieldService,
//            IMetaObjectService _metaObjectService,
//            IFieldBizDataService _fieldBizDataService,
//            IConfigFieldService _fieldListAggregationService
//            )
//        {
//            db = _db;
//            metaFieldService = _metaFieldService;
//            metaObjectService = _metaObjectService;
//            fieldBizDataService = _fieldBizDataService;
//            fieldListAggregationService = _fieldListAggregationService;
//        }

//        public Result Add(int tenantId, string metaObjectCode, BsonDocument bsons)
//        {
//            if (string.IsNullOrEmpty(metaObjectCode))
//                return Result.Error("实体编码不能为空");

//            var metaObject = metaObjectService.GetByCode(metaObjectCode);

//            if (metaObject == null)
//                return Result.Error("没有找到该实体编码对应的实体信息");

//            return Add(tenantId, metaObject, bsons);
//        }
//        public Result Add(int tenantId, MetaObject metaObject, BsonDocument bsons)
//        {
//            //错误信息返回值
//            HashSet<string> ErrorInfo = new HashSet<string>();

//            //获取到字段列表
//            var metaFields = metaFieldService.GetMetaFieldUpperKeyDicUnDeleted(metaObject.Id);
//            for (int i = bsons.ElementCount - 1; i >= 0; i--)
//            {
//                var item = bsons.GetElement(i);
//                string upperKey = item.Name.ToUpperInvariant();
//                if (metaFields.ContainsKey(upperKey))
//                {
//                    //检查字段的值是否符合字段类型
//                    var checkResult = metaFieldService.CheckAndGetFieldValueByFieldType(metaFields[upperKey], item.Value);
//                    if (checkResult.IsSuccess)
//                    {
//                        //如果大小写不匹配，则都转化成配置的字段Code形式
//                        if (!item.Name.Equals(metaFields[upperKey].Code))
//                        {
//                            bsons.RemoveElement(item);
//                            bsons.Add(new BsonElement(metaFields[upperKey].Code, BsonValue.Create(checkResult.Data)));
//                        }
//                        else
//                        {
//                            //重置字段的真实类型的值
//                            bsons.SetElement(new BsonElement(metaFields[upperKey].Code, BsonValue.Create(checkResult.Data)));
//                        }
//                    }
//                    else
//                    {
//                        return Result.Error($"字段[{item.Name}]传递的值[{item.Value}]不符合字段定义的类型");
//                    }
//                }
//                else
//                {
//                    //如果字段不在配置字段中，则不进行添加
//                    bsons.RemoveElement(item);
//                    ErrorInfo.Add($"字段[{item.Name}]不属于对象[{metaObject.Code}({metaObject.Name})]定义的字段");
//                }
//            }

//            //预置字段及其默认值
//            foreach (var item in metaFieldService.GetPresetFieldBsonElements())
//            {
//                //如果传入的字段已经有了，那么这里就不预置了
//                if (!bsons.Contains(item.Name))
//                {
//                    bsons.Add(item);
//                }
//            }

//            //补充字段
//            bsons.SetElement(new BsonElement(MetaDataConst.TenantId, tenantId));
//            bsons.SetElement(new BsonElement("MetaObjectCode", metaObject.Code));

//            db.GetCollectionBson(metaObject.Code).InsertOne(bsons);

//            return Result.Success($"插入成功，日志：{string.Join(",", ErrorInfo)}");
//        }

//        public Result BatchAdd(int tenantId, string metaObjectCode, List<BsonDocument> bsons)
//        {
//            if (string.IsNullOrEmpty(metaObjectCode))
//                return Result.Error("实体编码不能为空");

//            var metaObject = metaObjectService.GetByCode(metaObjectCode);

//            if (metaObject == null)
//                return Result.Error("没有找到该实体编码对应的实体信息");

//            return BatchAdd(tenantId, metaObject, bsons);
//        }
//        public Result BatchAdd(int tenantId, MetaObject metaObject, List<BsonDocument> bsonsList)
//        {
//            if (bsonsList != null && bsonsList.Any())
//            {
//                List<BsonDocument> insertBsonsList = new List<BsonDocument>();

//                //获取到字段列表
//                var metaFields = metaFieldService.GetMetaFieldUpperKeyDicUnDeleted(metaObject.Id);

//                for (int j = 0; j < bsonsList.Count; j++)
//                {
//                    var bsons = bsonsList[j];
//                    for (int i = bsons.ElementCount - 1; i >= 0; i--)
//                    {
//                        var item = bsons.GetElement(i);
//                        string upperKey = item.Name.ToUpperInvariant();
//                        if (metaFields.ContainsKey(upperKey))
//                        {
//                            //检查字段的值是否符合字段类型
//                            var checkResult = metaFieldService.CheckAndGetFieldValueByFieldType(metaFields[upperKey], item.Value);
//                            if (checkResult.IsSuccess)
//                            {
//                                //如果大小写不匹配，则都转化成配置的字段Code形式
//                                if (!item.Name.Equals(metaFields[upperKey].Code))
//                                {
//                                    bsons.RemoveElement(item);
//                                    bsons.Add(new BsonElement(metaFields[upperKey].Code, BsonValue.Create(checkResult.Data)));
//                                }
//                                else
//                                {
//                                    //重置字段的真实类型的值
//                                    bsons.SetElement(new BsonElement(metaFields[upperKey].Code, BsonValue.Create(checkResult.Data)));
//                                }
//                            }
//                            else
//                            {
//                                return Result.Error($"字段[{item.Name}]传递的值[{item.Value}]不符合字段定义的类型");
//                                //bsons.RemoveElement(item);
//                            }
//                        }
//                        else
//                        {
//                            //如果字段不在配置字段中，则不进行添加
//                            bsons.RemoveElement(item);
//                        }
//                    }

//                    //预置字段及其默认值
//                    foreach (var item in metaFieldService.GetPresetFieldBsonElements())
//                    {
//                        //如果传入的字段已经有了，那么这里就不预置了
//                        if (!bsons.Contains(item.Name))
//                        {
//                            bsons.Add(item);
//                        }
//                    }

//                    //补充字段
//                    bsons.SetElement(new BsonElement(MetaDataConst.TenantId, tenantId));
//                    bsons.SetElement(new BsonElement("MetaObjectCode", metaObject.Code));

//                    insertBsonsList.Add(bsons);
//                }

//                if (insertBsonsList.Any())
//                {
//                    db.GetCollectionBson(metaObject.Code).InsertMany(insertBsonsList);
//                }
//                return Result.Success($"插入成功! 成功{insertBsonsList.Count}条，失败{bsonsList.Count - insertBsonsList.Count}条.");
//            }
//            return Result.Success($"插入失败! 失败原因：没有任何数据需要插入.");
//        }

//        public Result Update(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition, BsonDocument bsons)
//        {
//            if (string.IsNullOrEmpty(metaObjectCode))
//                return Result.Error("实体编码不能为空");

//            var metaObject = metaObjectService.GetByCode(metaObjectCode);

//            if (metaObject == null)
//                return Result.Error("没有找到该实体编码对应的实体信息");

//            return Update(tenantId, metaObject, condition, bsons);
//        }
//        public Result Update(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition, BsonDocument bsons)
//        {
//            var metaObject = metaObjectService.GetById(metaObjectId);

//            if (metaObject == null)
//                return Result.Error("没有找到该实体编码对应的实体信息");

//            return Update(tenantId, metaObject, condition, bsons);
//        }
//        public Result Update(int tenantId, MetaObject metaObject, FilterDefinition<BsonDocument> condition, BsonDocument bsons)
//        {
//            //错误信息返回值
//            HashSet<string> ErrorInfo = new HashSet<string>();

//            if (bsons != null && bsons.Any())
//            {
//                //获取到字段列表
//                var metaFields = metaFieldService.GetMetaFieldUpperKeyDicUnDeleted(metaObject.Id);
//                for (int i = bsons.ElementCount - 1; i >= 0; i--)
//                {
//                    var item = bsons.GetElement(i);
//                    string upperKey = item.Name.ToUpperInvariant();
//                    if (metaFields.ContainsKey(upperKey))
//                    {
//                        //检查字段的值是否符合字段类型
//                        var checkResult = metaFieldService.CheckAndGetFieldValueByFieldType(metaFields[upperKey], item.Value);
//                        if (checkResult.IsSuccess)
//                        {
//                            //如果大小写不匹配，则都转化成配置的字段Code形式
//                            if (!item.Name.Equals(metaFields[upperKey].Code))
//                            {
//                                bsons.RemoveElement(item);
//                                bsons.Add(new BsonElement(metaFields[upperKey].Code, BsonValue.Create(checkResult.Data)));
//                            }
//                            else
//                            {
//                                //重置字段的真实类型的值
//                                bsons.SetElement(new BsonElement(metaFields[upperKey].Code, BsonValue.Create(checkResult.Data)));
//                            }
//                        }
//                        else
//                        {
//                            bsons.RemoveElement(item);
//                            ErrorInfo.Add($"字段[{item.Name}]传递的值[{item.Value}]不符合字段定义的类型");
//                        }
//                    }
//                    else
//                    {
//                        //如果字段不在配置字段中，则不进行添加
//                        bsons.RemoveElement(item);
//                        ErrorInfo.Add($"字段[{item.Name}]不属于对象[{metaObject.Code}({metaObject.Name})]定义的字段");
//                    }
//                }

//                var collection = db.GetCollectionBson(metaObject.Code);
//                var bu = Builders<BsonDocument>.Update;
//                foreach (var item in bsons)
//                {
//                    collection.UpdateMany(CombineTenantId(tenantId, condition), bu.Set(item.Name, item.Value));
//                }
//            }
//            return Result.Success($"修改成功，日志：{string.Join(",", ErrorInfo)}");
//        }

//        public Result Delete(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition)
//        {
//            if (string.IsNullOrEmpty(metaObjectCode))
//                return Result.Error("实体编码不能为空");

//            var metaObject = metaObjectService.GetByCode(metaObjectCode);

//            if (metaObject == null)
//                return Result.Error("没有找到该实体编码对应的实体信息");

//            return Delete(tenantId, metaObject, condition);
//        }
//        public Result Delete(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition)
//        {
//            var metaObject = metaObjectService.GetById(metaObjectId);

//            if (metaObject == null)
//                return Result.Error("没有找到该实体编码对应的实体信息");

//            return Delete(tenantId, metaObject, condition);
//        }
//        public Result Delete(int tenantId, MetaObject metaObject, FilterDefinition<BsonDocument> condition)
//        {
//            db.GetCollectionBson(metaObject.Code).DeleteMany(CombineTenantId(tenantId, condition));

//            return Result.Success($"删除成功");
//        }

//        public BsonDocument Get(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition, string[] columns = null)
//        {
//            if (string.IsNullOrEmpty(metaObjectCode))
//                throw new Exception("实体编码不能为空");
//            var metaObject = metaObjectService.GetByCode(metaObjectCode);
//            if (metaObject == null)
//                throw new Exception("没有找到该实体编码对应的实体信息");

//            return Get(tenantId, metaObject, condition, columns);
//        }
//        public BsonDocument Get(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition, string[] columns = null)
//        {
//            var metaObject = metaObjectService.GetById(metaObjectId);
//            if (metaObject == null)
//                throw new Exception("没有找到该实体编码对应的实体信息");

//            return Get(tenantId, metaObject, condition, columns);
//        }
//        public BsonDocument Get(int tenantId, MetaObject metaObject, FilterDefinition<BsonDocument> condition, string[] columns = null)
//        {
//            var projection = Builders<BsonDocument>.Projection.Include("_id");
//            //include fields
//            if (columns != null && columns.Any())
//                foreach (var item in columns)
//                    projection = projection.Include(item);

//            return db.GetCollectionBson(metaObject.Code).Find<BsonDocument>(CombineTenantId(tenantId, condition)).Project(projection).Limit(1)?.FirstOrDefault();
//        }
//        public SingleObjectComponent GetSingleObjectComponent(QueryPiplineContext queryPiplineContext, FilterDefinition<BsonDocument> condition)
//        {
//            var fieldMetas = fieldListAggregationService.GetColumnDataByFieldListId(queryPiplineContext);
//            var document = Get(queryPiplineContext.TenantId, queryPiplineContext.MetaObjectId, condition, fieldMetas?.Select(t => t.CmpData.Name)?.ToArray());
//            SingleObjectComponent singleObjectComponent = new SingleObjectComponent
//            {
//                BizData = fieldBizDataService.ToBizDataDictionary(queryPiplineContext, document),
//                ColunmDatas = fieldMetas?.OrderBy(t => t.CmpData.ShowIndex)?.ToList()
//            };
//            return singleObjectComponent;
//        }

//        public BsonDocument GetById(int tenantId, string metaObjectCode, string _id)
//        {
//            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", _id);
//            return Get(tenantId, metaObjectCode, filter);
//        }
//        public List<BsonDocument> GetByIds(int tenantId, string metaObjectCode, string[] _ids, SortDefinition<BsonDocument> sort, string[] columns = null)
//        {
//            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.In("_id", _ids);
//            return GetList(tenantId, metaObjectCode, filter, sort, columns);
//        }
//        public List<BsonDocument> GetList(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition, SortDefinition<BsonDocument> sort, string[] columns = null)
//        {
//            return GetList(tenantId, metaObjectId, condition, 0, 0, sort, columns);
//        }
//        public List<BsonDocument> GetList(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition, SortDefinition<BsonDocument> sort, string[] columns = null)
//        {
//            return GetList(tenantId, metaObjectCode, condition, 0, 0, sort, columns);
//        }

//        public List<BsonDocument> GetList(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, string[] columns = null)
//        {
//            var metaObject = metaObjectService.GetById(metaObjectId);
//            if (metaObject == null)
//                throw new Exception("没有找到该实体编码对应的实体信息");

//            return GetList(tenantId, metaObject, condition, pageIndex, pageSize, sort, columns);
//        }
//        public List<BsonDocument> GetList(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, string[] columns = null)
//        {
//            if (string.IsNullOrEmpty(metaObjectCode))
//                throw new Exception("实体编码不能为空");
//            var metaObject = metaObjectService.GetByCode(metaObjectCode);
//            if (metaObject == null)
//                throw new Exception("没有找到该实体编码对应的实体信息");

//            return GetList(tenantId, metaObject, condition, pageIndex, pageSize, sort, columns);
//        }
//        public List<BsonDocument> GetList(int tenantId, MetaObject metaObject, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, string[] columns = null)
//        {
//            List<BsonDocument> bson = new List<BsonDocument>();
//            if (pageSize == 0)
//            {
//                //无条件默认取10000条
//                pageSize = 1000;
//            }

//            var projection = Builders<BsonDocument>.Projection.Include("_id");
//            //include fields
//            if (columns != null && columns.Any())
//                foreach (var item in columns)
//                    projection = projection.Include(item);

//            int skipSize = (pageIndex - 1) > 0 ? ((pageIndex - 1) * pageSize) : 0;

//            if (sort == null)
//                sort = new SortDefinitionBuilder<BsonDocument>().Ascending("_id");

//            bson = db.GetCollectionBson(metaObject.Code).Find(CombineTenantId(tenantId, condition)).Skip(skipSize).Limit(pageSize).Sort(sort).Project(projection).ToList();
//            return bson;
//        }

//        public List<BsonDocument> GetList(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count, string[] columns = null)
//        {
//            var metaObject = metaObjectService.GetById(metaObjectId);
//            if (metaObject == null)
//                throw new Exception("没有找到该实体编码对应的实体信息");
//            return GetList(tenantId, metaObject, condition, pageIndex, pageSize, sort, out count, columns);
//        }
//        public List<BsonDocument> GetList(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count, string[] columns = null)
//        {
//            if (string.IsNullOrEmpty(metaObjectCode))
//                throw new Exception("实体编码不能为空");
//            var metaObject = metaObjectService.GetByCode(metaObjectCode);
//            if (metaObject == null)
//                throw new Exception("没有找到该实体编码对应的实体信息");
//            return GetList(tenantId, metaObject, condition, pageIndex, pageSize, sort, out count, columns);
//        }
//        public List<BsonDocument> GetList(int tenantId, MetaObject metaObject, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count, string[] columns = null)
//        {
//            List<BsonDocument> bson = GetList(tenantId, metaObject, condition, pageIndex, pageSize, sort, columns);

//            if (pageSize == 0)
//                count = bson?.Count ?? 0;
//            else
//                count = Convert.ToInt32(db.GetCollectionBson(metaObject.Code).CountDocuments(CombineTenantId(tenantId, condition)));

//            return bson;
//        }
//        public TableListComponent GetTableListComponent(QueryPiplineContext queryPiplineContext, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count)
//        {
//            var fieldMetas = fieldListAggregationService.GetColumnDataByFieldListId(queryPiplineContext);
//            var documents = GetList(queryPiplineContext.TenantId, queryPiplineContext.MetaObjectId, condition, pageIndex, pageSize, sort, out count, fieldMetas?.Select(t => t.CmpData.Name)?.ToArray());
//            TableListComponent tableListComponent = new TableListComponent
//            {
//                BizData = fieldBizDataService.ToBizDataDictionaryList(queryPiplineContext, documents),
//                BizDataTotalCount = count,
//                Columns = fieldMetas?.OrderBy(t => t.CmpData.ShowIndex)?.ToList()
//            };

//            if (pageSize != 0)
//                tableListComponent.PageCount = count / pageSize;

//            return tableListComponent;
//        }

//        public int GetCount(int tenantId, int metaObjectId, FilterDefinition<BsonDocument> condition)
//        {
//            var metaObject = metaObjectService.GetById(metaObjectId);
//            if (metaObject == null)
//                throw new Exception("没有找到该实体编码对应的实体信息");

//            return GetCount(tenantId, metaObject, condition);
//        }
//        public int GetCount(int tenantId, string metaObjectCode, FilterDefinition<BsonDocument> condition)
//        {
//            if (string.IsNullOrEmpty(metaObjectCode))
//                throw new Exception("实体编码不能为空");
//            var metaObject = metaObjectService.GetByCode(metaObjectCode);
//            if (metaObject == null)
//                throw new Exception("没有找到该实体编码对应的实体信息");

//            return GetCount(tenantId, metaObject, condition);
//        }
//        public int GetCount(int tenantId, MetaObject metaObject, FilterDefinition<BsonDocument> condition)
//        {
//            return Convert.ToInt32(db.GetCollectionBson(metaObject.Code).CountDocuments(CombineTenantId(tenantId, condition)));
//        }

//        /// <summary>
//        /// 拼接租户id到条件上
//        /// </summary>
//        /// <param name="tenantId"></param>
//        /// <param name="filter"></param>
//        /// <returns></returns>
//        private FilterDefinition<BsonDocument> CombineTenantId(int tenantId, FilterDefinition<BsonDocument> filter)
//        {
//            //把租户id拼接上
//            var bf = Builders<BsonDocument>.Filter;
//            return bf.And(filter, bf.Eq(MetaDataConst.TenantId, tenantId));
//        }
//    }
//}
