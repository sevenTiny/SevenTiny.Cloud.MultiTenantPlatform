using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;

namespace SevenTiny.Cloud.MultiTenant.Application.Service
{
    internal class MetaFieldAppService : IMetaFieldAppService
    {
        public MetaFieldAppService(IMetaObjectService metaObjectService, IMetaFieldService metaFieldService)
        {
            _metaObjectService = metaObjectService;
            _metaFieldService = metaFieldService;
        }

        private IMetaObjectService _metaObjectService;
        private IMetaFieldService _metaFieldService;

        public Result Add(MetaField entity, Guid metaObjectId)
        {
            return Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                //拼接编码
                .Continue(_ =>
                {
                    var metaObject = _metaObjectService.GetById(metaObjectId);
                    if (metaObject == null)
                        return Result.Error($"[{metaObjectId}] 对应的对象信息未找到");

                    entity.Code = string.Concat(metaObject.Code, ".", entity.Code);
                    return _;
                })
                .Continue(_ => _metaFieldService.Add(entity));
        }
    }
}