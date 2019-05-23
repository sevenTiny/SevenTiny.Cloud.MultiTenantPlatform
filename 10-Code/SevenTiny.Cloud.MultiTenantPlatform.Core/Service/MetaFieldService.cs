using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
{
    public class MetaFieldService : MetaObjectManageRepository<MetaField>, IMetaFieldService
    {
        public MetaFieldService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public new List<MetaField> GetEntitiesDeletedByMetaObjectId(int metaObjectId)
            => dbContext.Queryable<MetaField>().Where(t => t.IsDeleted == (int)IsDeleted.Deleted && t.MetaObjectId == metaObjectId).ToList();

        public new List<MetaField> GetEntitiesUnDeletedByMetaObjectId(int metaObjectId)
        {
            //取字段要包含上系统字段
            return dbContext.Queryable<MetaField>().Where(t => t.IsDeleted == (int)IsDeleted.UnDeleted && (t.MetaObjectId == metaObjectId || t.MetaObjectId == -1)).ToList();
        }

        public Dictionary<string, MetaField> GetMetaFieldDicUnDeleted(int metaObjectId)
        {
            var metaFields = GetEntitiesUnDeletedByMetaObjectId(metaObjectId);
            return metaFields.ToDictionary(t => t.Code, t => t);
        }

        public Dictionary<string, MetaField> GetMetaFieldUpperKeyDicUnDeleted(int metaObjectId)
        {
            var metaFields = GetEntitiesUnDeletedByMetaObjectId(metaObjectId);
            return metaFields.ToDictionary(t => t.Code.ToUpperInvariant(), t => t);
        }

        public Dictionary<int, MetaField> GetMetaFieldDicIdObjUnDeleted(int metaObjectId)
        {
            var metaFields = GetEntitiesUnDeletedByMetaObjectId(metaObjectId);
            return metaFields.ToDictionary(t => t.Id, t => t);
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="metaField"></param>
        public new Result<MetaField> Update(MetaField metaField)
        {
            MetaField myfield = GetById(metaField.Id);
            if (myfield != null)
            {
                //编码不允许修改
                myfield.Name = metaField.Name;
                myfield.Group = metaField.Group;
                myfield.SortNumber = metaField.SortNumber;
                myfield.Description = metaField.Description;
                //myfield.FieldType = metaField.FieldType;//字段类型不允许修改
                myfield.IsMust = metaField.IsMust;
                myfield.DataSourceId = metaField.DataSourceId;
                myfield.ModifyBy = -1;
                myfield.ModifyTime = DateTime.Now;
            }
            base.Update(myfield);
            return Result<MetaField>.Success();
        }

        /// <summary>
        /// 预置字段
        /// </summary>
        /// <param name="metaObjectId"></param>
        public void PresetFields(int metaObjectId)
        {
            dbContext.Add<MetaField>(new List<MetaField> {
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="_id",
                    Name ="数据ID",
                    Description="系统字段",
                    IsSystem =(int)TrueFalse.True,
                    IsMust=(int)TrueFalse.True,
                    FieldType=(int)DataType.Text,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="IsDeleted",
                    Name ="是否删除",
                    Description="系统字段",
                    IsSystem =(int)TrueFalse.True,
                    IsMust=(int)TrueFalse.True,
                    FieldType=(int)DataType.Boolean,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="CreateBy",
                    Name ="创建人",
                    Description="系统字段",
                    IsSystem =(int) TrueFalse.True,
                    IsMust= (int)TrueFalse.True,
                    FieldType= (int)DataType.Int,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="CreateTime",
                    Name ="创建时间",
                    Description="系统字段",
                    IsSystem =(int) TrueFalse.True,
                    IsMust= (int)TrueFalse.True,
                    FieldType= (int)DataType.DateTime,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="ModifyBy",
                    Name ="修改人",
                    Description="系统字段",
                    IsSystem =(int) TrueFalse.True,
                    IsMust= (int)TrueFalse.True,
                    FieldType= (int)DataType.Int,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="ModifyTime",
                    Name ="修改时间",
                    Description="系统字段",
                    IsSystem =(int) TrueFalse.True,
                    IsMust= (int)TrueFalse.True,
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
        public Result<dynamic> CheckAndGetFieldValueByFieldType(int fieldId, object value)
        {
            MetaField metaField = GetById(fieldId);
            return CheckAndGetFieldValueByFieldType(metaField, value);
        }

        public Result<dynamic> CheckAndGetFieldValueByFieldType(MetaField metaField, object value)
        {
            Result<dynamic> result = new Result<dynamic>();
            switch ((DataType)metaField.FieldType)
            {
                case DataType.Boolean:
                    result.IsSuccess = bool.TryParse(Convert.ToString(value), out bool boolVal);
                    result.Data = boolVal;
                    break;
                case DataType.Number:
                    result.IsSuccess = int.TryParse(Convert.ToString(value), out int number);
                    if (number < 0)
                        result.IsSuccess = false;
                    result.Data = number;
                    break;
                case DataType.Unknown:
                case DataType.Text:
                default:
                    result.IsSuccess = true;
                    result.Data = Convert.ToString(value);
                    break;
                case DataType.StandradDateTime:
                case DataType.DateTime:
                case DataType.StandradDate:
                case DataType.Date:
                    result.IsSuccess = DateTime.TryParse(Convert.ToString(value), out DateTime dateTimeVal);
                    result.Data = dateTimeVal;
                    break;
                case DataType.Int:
                    result.IsSuccess = int.TryParse(Convert.ToString(value), out int intVal);
                    result.Data = intVal;
                    break;
                case DataType.Long:
                    result.IsSuccess = long.TryParse(Convert.ToString(value), out long longVal);
                    result.Data = longVal;
                    break;
                case DataType.Double:
                    result.IsSuccess = double.TryParse(Convert.ToString(value), out double doubleVal);
                    result.Data = doubleVal;
                    break;
                case DataType.DataSource:
                    result.IsSuccess = false;
                    break;
            }
            return result;
        }

        public List<MetaField> GetByIds(int[] ids)
        {
            //Sql直接查询是没有缓存的
            //StringBuilder builder = new StringBuilder();
            //builder.Append("SELECT * FROM ");
            //builder.Append(dbContext.GetTableName<MetaField>());
            //builder.Append(" WHERE ");
            //for (int i = 0; i < ids.Length; i++)
            //{
            //    if (i == 0)
            //        builder.Append(" Id=" + ids[i]);
            //    else
            //        builder.Append(" OR Id=" + ids[i]);
            //}
            //return dbContext.ExecuteQueryListSql<MetaField>(builder.ToString());

            //直接通过id查询的方案，配合二级缓存性能高
            //这里需要orm支持in操作性能会提高
            List<MetaField> metaFields = new List<MetaField>();
            foreach (var item in ids)
            {
                var metaField = GetById(item);
                if (metaField != null)
                {
                    metaFields.Add(metaField);
                }
            }
            return metaFields;
        }

        /// <summary>
        /// 删除,需要先解除引用关系
        /// </summary>
        /// <param name="id"></param>
        public new Result Delete(int id)
        {
            //先检查引用关系
            if (dbContext.Queryable<FieldListMetaField>().Where(t => t.MetaFieldId == id).Any())
            {
                return Result.Error("字段列表存在相关字段的引用关系，请先解除引用关系");
            }

            if (dbContext.Queryable<SearchConditionNode>().Where(t => t.FieldId == id).Any())
            {
                return Result.Error("搜索条件存在相关字段的引用关系，请先解除引用关系");
            }

            // ... 如果有其他引用字段的地方需要补充引用关系检查

            base.Delete(id);

            return Result.Success("删除成功");
        }

        public SortDefinition<BsonDocument> GetSortDefinitionBySortFields(int metaObjectId, SortField[] sortFields)
        {
            var builder = new SortDefinitionBuilder<BsonDocument>();
            if (sortFields == null || !sortFields.Any())
            {
                //默认给更新时间倒序排列
                sortFields = new[] { new SortField { Column = "ModifyTime", IsDesc = true } };
            }
            //获取全部字段
            var metaFieldDic = GetMetaFieldDicUnDeleted(metaObjectId);
            SortDefinition<BsonDocument> sort = null;
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
