using MongoDB.Bson;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class MetaFieldService : MetaObjectManageRepository<MetaField>, IMetaFieldService
    {
        public MetaFieldService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

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

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="metaField"></param>
        public new ResultModel Update(MetaField metaField)
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
            return ResultModel.Success();
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
                    Code ="IsDeleted",
                    Name ="是否删除",
                    Description="系统字段",
                    IsSystem =(int)TrueFalse.True,
                    IsMust=(int)TrueFalse.True,
                    FieldType=(int)DataType.Int,
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
                { "IsDeleted",(int)IsDeleted.UnDeleted },
                { "CreateBy", -1 },
                { "CreateTime", DateTime.Now },
                { "ModifyBy", -1},
                { "ModifyTime", DateTime.Now }
            };
        }

        public ResultModel CheckAndGetFieldValueByFieldType(int fieldId, object value)
        {
            MetaField metaField = GetById(fieldId);
            ResultModel result = new ResultModel();
            switch (EnumsTranslaterUseInProgram.ToDataType(metaField.FieldType))
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
                    result.IsSuccess = DateTimeOffset.TryParse(Convert.ToString(value), out DateTimeOffset dateTimeOffsetVal);
                    result.Data = dateTimeOffsetVal;
                    break;
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
    }
}
