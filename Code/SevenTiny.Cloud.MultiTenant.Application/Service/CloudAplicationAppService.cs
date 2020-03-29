using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;

namespace SevenTiny.Cloud.MultiTenant.Application.Service
{
    internal class CloudAplicationAppService : ICloudApplicationAppService
    {
        public CloudAplicationAppService(IMetaObjectAppService metaObjectAppService, IMetaObjectService metaObjectService)
        {
            _metaObjectAppService = metaObjectAppService;

            _metaObjectService = metaObjectService;
        }

        IMetaObjectAppService _metaObjectAppService;

        IMetaObjectService _metaObjectService;

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public Result<CloudApplication> Delete(Guid id)
        {
            var metaObjects = _metaObjectService.GetListAll();

            return Result<CloudApplication>.Success()
                .ContinueWithTryCatch(_ =>
                {
                    _metaObjectService.TransactionBegin();

                    metaObjects.ForEach(item =>
                    {
                        _metaObjectAppService.Delete(item.Id);
                    });

                    _metaObjectService.TransactionCommit();

                    return _;
                }, _ =>
                {
                    _metaObjectService.TransactionRollback();
                });
        }
    }
}