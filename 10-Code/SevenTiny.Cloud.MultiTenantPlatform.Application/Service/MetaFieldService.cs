//using SevenTiny.Cloud.MultiTenantPlatform.Application.ServiceContract;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
//using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryContract;
//using System.Collections.Generic;

//namespace SevenTiny.Cloud.MultiTenantPlatform.Application.Service
//{
//    public class MetaFieldService : IMetaFieldService
//    {
//        private readonly IMetaFieldRepository _metaFieldRepository;
//        public MetaFieldService(IMetaFieldRepository metaFieldRepository)
//        {
//            this._metaFieldRepository = metaFieldRepository;
//        }

//        //下面这个逻辑应该放在Domain层

//        /// <summary>
//        /// 为MetaObject添加默认字段
//        /// </summary>
//        /// <param name="metaObjectId"></param>
//        public void PresetFields(int metaObjectId)
//        {
//            _metaFieldRepository.Add(new List<MetaField> {
//                new MetaField{
//                    MetaObjectId=metaObjectId,
//                    Code ="IsDeleted",
//                    Name ="是否删除",
//                    Description="系统字段",
//                    IsSystem =(int)TrueFalse.True,
//                    IsMust=(int)TrueFalse.True,
//                    FieldType=(int)DataType.Int,
//                    SortNumber=-1
//                },
//                new MetaField{
//                    MetaObjectId=metaObjectId,
//                    Code ="CreateBy",
//                    Name ="创建人",
//                    Description="系统字段",
//                    IsSystem =(int) TrueFalse.True,
//                    IsMust= (int)TrueFalse.True,
//                    FieldType= (int)DataType.Int,
//                    SortNumber=-1
//                },
//                new MetaField{
//                    MetaObjectId=metaObjectId,
//                    Code ="CreateTime",
//                    Name ="创建时间",
//                    Description="系统字段",
//                    IsSystem =(int) TrueFalse.True,
//                    IsMust= (int)TrueFalse.True,
//                    FieldType= (int)DataType.DateTime,
//                    SortNumber=-1
//                },
//                new MetaField{
//                    MetaObjectId=metaObjectId,
//                    Code ="ModifyBy",
//                    Name ="修改人",
//                    Description="系统字段",
//                    IsSystem =(int) TrueFalse.True,
//                    IsMust= (int)TrueFalse.True,
//                    FieldType= (int)DataType.Int,
//                    SortNumber=-1
//                },
//                new MetaField{
//                    MetaObjectId=metaObjectId,
//                    Code ="ModifyTime",
//                    Name ="修改时间",
//                    Description="系统字段",
//                    IsSystem =(int) TrueFalse.True,
//                    IsMust= (int)TrueFalse.True,
//                    FieldType= (int)DataType.DateTime,
//                    SortNumber=-1
//                }
//            });
//        }
//    }
//}
