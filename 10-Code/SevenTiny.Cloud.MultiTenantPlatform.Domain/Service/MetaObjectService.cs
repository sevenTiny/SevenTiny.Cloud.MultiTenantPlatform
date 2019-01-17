using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class MetaObjectService : CommonInfoRepository<MetaObject>, IMetaObjectService
    {
        public MetaObjectService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        MultiTenantPlatformDbContext dbContext;

        public List<MetaObject> GetMetaObjectsUnDeletedByApplicationId(int applicationId)
            => dbContext.QueryList<MetaObject>(t => t.ApplicationId == applicationId && t.IsDeleted == (int)IsDeleted.UnDeleted);

        public List<MetaObject> GetMetaObjectsDeletedByApplicationId(int applicationId)
            => dbContext.QueryList<MetaObject>(t => t.ApplicationId == applicationId && t.IsDeleted == (int)IsDeleted.Deleted);

        public MetaObject GetMetaObjectByCodeOrNameWithApplicationId(int applicationId, string code, string name)
            => dbContext.QueryOne<MetaObject>(t => (t.ApplicationId == applicationId && t.Name.Equals(name)) || (t.ApplicationId == applicationId && t.Code.Equals(code)));

        public MetaObject GetMetaObjectByCodeAndApplicationId(int applicationId, string code)
            => dbContext.QueryOne<MetaObject>(t => t.ApplicationId == applicationId && t.Code.Equals(code));

        public void PresetFields(int metaObjectId)
        {
            dbContext.Add(new List<MetaField> {
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

        public new void Update(MetaObject metaObject)
        {
            MetaObject app = GetById(metaObject.Id);
            if (app != null)
            {
                app.Name = metaObject.Name;
                app.Group = metaObject.Group;
                app.SortNumber = metaObject.SortNumber;
                app.Description = metaObject.Description;
                app.ModifyBy = -1;
                app.ModifyTime = DateTime.Now;
            }
            dbContext.Update(t => t.Id == metaObject.Id, app);
        }

        public bool ExistSameNameWithOtherIdByApplicationId(int applicationId, int id, string name)
            => dbContext.QueryExist<MetaObject>(t => t.ApplicationId != applicationId && t.Name.Equals(name) && t.Id != id);
    }
}
