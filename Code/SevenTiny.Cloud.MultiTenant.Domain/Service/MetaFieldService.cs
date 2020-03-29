using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.ValueObject;
using SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class MetaFieldService : MetaObjectCommonServiceBase<MetaField>, IMetaFieldService
    {
        public MetaFieldService(IMetaFieldRepository metaFieldRepository) : base(metaFieldRepository)
        {
            _metaFieldRepository = metaFieldRepository;
        }

        IMetaFieldRepository _metaFieldRepository;

        public Dictionary<string, MetaField> GetMetaFieldDicUnDeleted(Guid metaObjectId)
        {
            var metaFields = _metaFieldRepository.GetSystemAndCustomListUnDeleted(metaObjectId);
            return metaFields.ToDictionary(t => t.Code, t => t);
        }

        public Dictionary<string, MetaField> GetMetaFieldUpperKeyDicUnDeleted(Guid metaObjectId)
        {
            var metaFields = _metaFieldRepository.GetSystemAndCustomListUnDeleted(metaObjectId);
            return metaFields.ToDictionary(t => t.Code.ToUpperInvariant(), t => t);
        }

        public Dictionary<Guid, MetaField> GetMetaFieldIdDicUnDeleted(Guid metaObjectId)
        {
            var metaFields = _metaFieldRepository.GetSystemAndCustomListUnDeleted(metaObjectId);
            return metaFields.ToDictionary(t => t.Id, t => t);
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="metaField"></param>
        public new Result Update(MetaField metaField)
        {
            return base.UpdateWithOutCode(metaField, target =>
            {
                target.DataSourceId = metaField.DataSourceId;
            });
        }

        /// <summary>
        /// 预置字段
        /// </summary>
        /// <param name="metaObjectId"></param>
        public void PresetFields(Guid metaObjectId)
        {
            _metaFieldRepository.BatchAdd(new List<MetaField> {
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="_id",
                    Name ="数据ID",
                    Description="系统字段",
                    IsSystem =(int)TrueFalse.True,
                    FieldType=(int)DataType.Text,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="IsDeleted",
                    Name ="是否删除",
                    Description="系统字段",
                    IsSystem =(int)TrueFalse.True,
                    FieldType=(int)DataType.Boolean,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="CreateBy",
                    Name ="创建人",
                    Description="系统字段",
                    IsSystem =(int) TrueFalse.True,
                    FieldType= (int)DataType.Int,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="CreateTime",
                    Name ="创建时间",
                    Description="系统字段",
                    IsSystem =(int) TrueFalse.True,
                    FieldType= (int)DataType.DateTime,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="ModifyBy",
                    Name ="修改人",
                    Description="系统字段",
                    IsSystem =(int) TrueFalse.True,
                    FieldType= (int)DataType.Int,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="ModifyTime",
                    Name ="修改时间",
                    Description="系统字段",
                    IsSystem =(int) TrueFalse.True,
                    FieldType= (int)DataType.DateTime,
                    SortNumber=-1
                }
            });
        }

        /// <summary>
        /// 预置字段和数据的字典形式
        /// </summary>
        /// <returns></returns>
        public BsonDocument GetPresetFieldBsonElements()
        {
            return new BsonDocument
            {
                { "_id",Guid.NewGuid().ToString()},
                { "IsDeleted",false },
                { "CreateBy", -1 },
                { "CreateTime", DateTime.Now },
                { "ModifyBy", -1},
                { "ModifyTime", DateTime.Now }
            };
        }

        /// <summary>
        /// 同方法内多次调用该方法尽量不要直接用这个查询数据库，性能较差，应该通过对象查出所有对象用下面的重载方法
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Result<dynamic> CheckAndGetFieldValueByFieldType(Guid fieldId, object value)
        {
            MetaField metaField = _metaFieldRepository.GetById(fieldId);
            return CheckAndGetFieldValueByFieldType(metaField, value);
        }

        public Result<dynamic> CheckAndGetFieldValueByFieldType(MetaField metaField, object value)
        {
            dynamic resultData = null;
            bool isSuccess = false;
            switch ((DataType)metaField.FieldType)
            {
                case DataType.Boolean:
                    isSuccess = bool.TryParse(Convert.ToString(value), out bool boolVal);
                    resultData = boolVal;
                    break;
                case DataType.Number:
                    isSuccess = int.TryParse(Convert.ToString(value), out int number);
                    if (number < 0)
                        isSuccess = false;
                    resultData = number;
                    break;
                case DataType.Unknown:
                case DataType.Text:
                default:
                    isSuccess = true;
                    resultData = Convert.ToString(value);
                    break;
                case DataType.StandradDateTime:
                case DataType.DateTime:
                case DataType.StandradDate:
                case DataType.Date:
                    isSuccess = DateTime.TryParse(Convert.ToString(value), out DateTime dateTimeVal);
                    resultData = dateTimeVal;
                    break;
                case DataType.Int:
                    isSuccess = int.TryParse(Convert.ToString(value), out int intVal);
                    resultData = intVal;
                    break;
                case DataType.Long:
                    isSuccess = long.TryParse(Convert.ToString(value), out long longVal);
                    resultData = longVal;
                    break;
                case DataType.Double:
                    isSuccess = double.TryParse(Convert.ToString(value), out double doubleVal);
                    resultData = doubleVal;
                    break;
                case DataType.DataSource:
                    isSuccess = false;
                    break;
            }

            return isSuccess ? Result<dynamic>.Success(data: resultData) : Result<dynamic>.Error(data: resultData);
        }

        public List<MetaField> GetByIds(Guid metaObjectId, Guid[] ids)
        {
            List<MetaField> metaFields = new List<MetaField>();

            if (ids == null || !ids.Any())
                return metaFields;

            //Sql直接查询是没有缓存的
            //metaFields = dbContext.Queryable($"SELECT * FROM {dbContext.GetTableName<MetaField>()} WHERE Id IN ({string.Join(",", ids)})").ToList<MetaField>();

            //直接通过id查询的方案，配合二级缓存性能高
            //这里需要orm支持in操作性能会提高
            metaFields = _metaFieldRepository.GetSystemAndCustomListUnDeleted(metaObjectId).Where(t => ids.Contains(t.Id)).ToList();

            return metaFields;
        }

        public SortDefinition<BsonDocument> GetSortDefinitionBySortFields(QueryPiplineContext queryPiplineContext, SortField[] sortFields)
        {
            var builder = new SortDefinitionBuilder<BsonDocument>();
            if (sortFields == null || !sortFields.Any())
                return builder.Ascending("_id");

            //获取全部字段
            SortDefinition<BsonDocument> sort = null;
            var metaFieldDic = queryPiplineContext.MetaFieldsUnDeletedCodeDic;
            foreach (var item in sortFields)
            {
                if (!metaFieldDic.ContainsKey(item.Column))
                {
                    throw new ArgumentNullException(item.Column, $"field of {item.Column} is not exist in current MetaObject");
                }
                if (item.IsDesc)
                {
                    if (sort == null)
                        sort = builder.Descending(item.Column);
                    else
                        sort = sort.Descending(item.Column);
                }
                else
                {
                    if (sort == null)
                        sort = builder.Ascending(item.Column);
                    else
                        sort = sort.Ascending(item.Column);
                }
            }
            return sort;
        }
    }
}
