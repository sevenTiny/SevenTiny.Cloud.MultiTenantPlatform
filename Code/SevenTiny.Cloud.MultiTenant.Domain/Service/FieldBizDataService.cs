using MongoDB.Bson;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject.UIMetaData;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class FieldBizDataService : IFieldBizDataService
    {
        readonly IConfigFieldService fieldAggregationService;
        public FieldBizDataService(IConfigFieldService _fieldAggregationService)
        {
            fieldAggregationService = _fieldAggregationService;
        }

        public Dictionary<string, FieldBizData> ToBizDataDictionary(QueryPiplineContext queryPiplineContext, BsonDocument bsonElement)
        {
            if (bsonElement != null && bsonElement.Any())
            {
                //接口配置的字段字典
                var interfaceMetaFieldsDic = queryPiplineContext.MetaFieldsCodeDicByFieldListId;
                Dictionary<string, FieldBizData> keyValuePairs = new Dictionary<string, FieldBizData>();
                foreach (var field in interfaceMetaFieldsDic)
                {
                    //如果当前结果集包含字段
                    if (bsonElement.Contains(field.Key))
                    {
                        keyValuePairs.Add(field.Key, new FieldBizData
                        {
                            Name = field.Key,
                            Text = bsonElement[field.Key]?.ToString(),
                            Value = bsonElement[field.Key]
                        });
                    }
                }
                return keyValuePairs;
            }
            return null;
        }

        public List<Dictionary<string, FieldBizData>> ToBizDataDictionaryList(QueryPiplineContext queryPiplineContext, List<BsonDocument> bsonElements)
        {
            //接口配置的字段字典
            var interfaceMetaFieldsDic = queryPiplineContext.MetaFieldsCodeDicByFieldListId;
            List<Dictionary<string, FieldBizData>> resultList = new List<Dictionary<string, FieldBizData>>();
            if (bsonElements != null && bsonElements.Any())
            {
                foreach (var elements in bsonElements)
                {
                    Dictionary<string, FieldBizData> keyValuePairs = new Dictionary<string, FieldBizData>();
                    foreach (var field in interfaceMetaFieldsDic)
                    {
                        //如果当前结果集包含字段
                        if (elements.Contains(field.Key))
                        {
                            keyValuePairs.Add(field.Key, new FieldBizData
                            {
                                Name = field.Key,
                                Text = elements[field.Key]?.ToString(),
                                Value = elements[field.Key]
                            });
                        }
                    }
                    resultList.Add(keyValuePairs);
                }
            }
            return resultList;
        }
    }
}
