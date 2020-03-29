using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class MetaObjectService : CommonServiceBase<MetaObject>, IMetaObjectService
    {
        public MetaObjectService(IMetaObjectRepository metaObjectRepository) : base(metaObjectRepository)
        {
            _metaObjectRepository = metaObjectRepository;
        }

        IMetaObjectRepository _metaObjectRepository;

        public Result Add(Guid applicationId, string applicationCode, MetaObject metaObject)
        {
            return Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(applicationCode, nameof(applicationCode))
                .ContinueEnsureArgumentNotNullOrEmpty(metaObject, nameof(metaObject))
                .ContinueEnsureArgumentNotNullOrEmpty(metaObject.Name, nameof(metaObject.Name))
                .ContinueEnsureArgumentNotNullOrEmpty(metaObject.Code, nameof(metaObject.Code))
                //设置默认字段值
                .Continue(_ =>
                {
                    metaObject.ApplicationId = applicationId;
                    metaObject.Code = string.Concat(applicationCode, ".", metaObject.Code);
                    return _;
                })
                .Continue(_ => base.Add(metaObject));
        }

        public List<MetaObject> GetMetaObjectListUnDeletedByApplicationId(Guid applicationId)
        {
            return _metaObjectRepository.GetMetaObjectListUnDeletedByApplicationId(applicationId);
        }

        public List<MetaObject> GetMetaObjectListDeletedByApplicationId(Guid applicationId)
        {
            return _metaObjectRepository.GetMetaObjectListDeletedByApplicationId(applicationId);
        }

        public MetaObject GetMetaObjectByCodeOrNameWithApplicationId(Guid applicationId, string code, string name)
        {
            return GetMetaObjectByCodeOrNameWithApplicationId(applicationId, code, name);
        }

        public MetaObject GetMetaObjectByCodeAndApplicationId(Guid applicationId, string code)
        {
            return GetMetaObjectByCodeAndApplicationId(applicationId, code);
        }
    }
}
