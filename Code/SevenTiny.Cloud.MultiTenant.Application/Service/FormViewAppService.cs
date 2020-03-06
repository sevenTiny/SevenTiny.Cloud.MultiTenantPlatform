using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Application.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;

namespace SevenTiny.Cloud.MultiTenant.Application.Service
{
    internal class FormViewAppService : IFormViewAppService
    {
        public FormViewAppService(
            IFormViewRepository formViewRepository,
            ICloudInterfaceRepository cloudInterfaceRepository
            )
        {
            _formViewRepository = formViewRepository;
            _cloudInterfaceRepository = cloudInterfaceRepository;
        }

        IFormViewRepository _formViewRepository;
        ICloudInterfaceRepository _cloudInterfaceRepository;

        Result<FormView> IFormViewAppService.Delete(Guid id)
            => Result<FormView>.Success()
                //校验是否存在引用关系，先删除引用该数据的数据
                .ContinueAssert(_ => !_cloudInterfaceRepository.CheckFormIdExist(id), "存在cloud接口的引用关系，先删除引用该数据的数据")
                .Continue(_ => _formViewRepository.LogicDelete(id));
    }
}