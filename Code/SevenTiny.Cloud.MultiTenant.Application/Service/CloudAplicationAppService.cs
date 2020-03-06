using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;

namespace SevenTiny.Cloud.MultiTenant.Application.Service
{
    internal class CloudAplicationAppService : ICloudApplicationAppService
    {
        public CloudAplicationAppService(
            IMetaObjectAppService metaObjectAppService,

            IMetaObjectRepository metaObjectRepository
            )
        {
            _metaObjectAppService = metaObjectAppService;

            _metaObjectRepository = metaObjectRepository;
        }

        IMetaObjectAppService _metaObjectAppService;

        IMetaObjectRepository _metaObjectRepository;

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public Result<CloudApplication> Delete(Guid id)
        {
            var metaObjects = _metaObjectRepository.GetListAll();

            return Result<CloudApplication>.Success()
                .ContinueWithTryCatch(_ =>
                {
                    _metaObjectRepository.TransactionBegin();

                    metaObjects.ForEach(item =>
                    {
                        _metaObjectAppService.Delete(item.Id);
                    });

                    _metaObjectRepository.TransactionCommit();

                    return _;
                }, _ =>
                {
                    _metaObjectRepository.TransactionRollback();
                });
        }
    }
}