using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ValueObject;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 检查是否有相同名称的编码或名称
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultModel CheckSameCodeOrName(int metaObjectId, MetaField metaField)
        {
            var obj = dbContext.QueryOne<MetaField>(t => t.MetaObjectId == metaObjectId && t.Id != metaField.Id && (t.Code.Equals(metaField.Code) || t.Name.Equals(metaField.Name)));
            if (obj != null)
            {
                if (obj.Code.Equals(metaField.Code))
                    return ResultModel.Error($"编码[{obj.Code}]已存在", metaField);
                else if (obj.Name.Equals(metaField.Name))
                    return ResultModel.Error($"名称[{obj.Name}]已存", metaField);
            }
            return ResultModel.Success();
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="metaField"></param>
        public new void Update(MetaField metaField)
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
    }
}
