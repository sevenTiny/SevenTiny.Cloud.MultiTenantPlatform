using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Model.Enums;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Application
{
    public class MetaFieldService : IMetaFieldService
    {
        private readonly IRepository<MetaField> _metaFieldRepository;
        public MetaFieldService(IRepository<MetaField> metaFieldRepository)
        {
            this._metaFieldRepository = metaFieldRepository;
        }
        /// <summary>
        /// 为MetaObject添加默认字段
        /// </summary>
        /// <param name="metaObjectId"></param>
        public void PresetFields(int metaObjectId)
        {
            _metaFieldRepository.Add(new List<MetaField> {
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="IsDeleted",
                    Name ="是否删除",
                    Description="标识数据是否删除",
                    IsSystem =(int)TrueFalse.True,
                    IsMust=(int)TrueFalse.True,
                    FieldType=(int)DataType.Int,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="CreateBy",
                    Name ="创建人",
                    Description="数据创建人",
                    IsSystem =(int) TrueFalse.True,
                    IsMust= (int)TrueFalse.True,
                    FieldType= (int)DataType.Int,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="CreateTime",
                    Name ="创建时间",
                    Description="数据创建时间",
                    IsSystem =(int) TrueFalse.True,
                    IsMust= (int)TrueFalse.True,
                    FieldType= (int)DataType.DateTime,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="ModifyBy",
                    Name ="修改人",
                    Description="数据修改人",
                    IsSystem =(int) TrueFalse.True,
                    IsMust= (int)TrueFalse.True,
                    FieldType= (int)DataType.Int,
                    SortNumber=-1
                },
                new MetaField{
                    MetaObjectId=metaObjectId,
                    Code ="ModifyTime",
                    Name ="修改时间",
                    Description="数据修改时间",
                    IsSystem =(int) TrueFalse.True,
                    IsMust= (int)TrueFalse.True,
                    FieldType= (int)DataType.DateTime,
                    SortNumber=-1
                }
            });
        }
    }
}
