using SevenTiny.Bantina;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.DataAccess;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Enum;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Infrastructure.ValueObject;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    public class MetaObjectService : CommonServiceBase<MetaObject>, IMetaObjectService
    {
        public MetaObjectService(IMetaObjectRepository metaObjectRepository) : base(metaObjectRepository)
        {
            _metaObjectRepository = metaObjectRepository;
        }

        IMetaObjectRepository _metaObjectRepository;

        public Result<MetaObject> Add(Guid applicationId, string applicationCode, MetaObject metaObject)
        {
            return Result<MetaObject>.Success()
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
                .Continue(_ => _metaObjectRepository.Add(metaObject));
        }
    }
}
