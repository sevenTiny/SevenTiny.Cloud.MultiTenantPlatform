using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.Repository;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using System;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Domain.Service
{
    internal class FormViewService : MetaObjectCommonServiceBase<FormView>, IFormViewService
    {
        public FormViewService(IFormViewRepository formRepository) : base(formRepository)
        {
            _formRepository = formRepository;
        }

        IFormViewRepository _formRepository;

        public new Result Add(FormView entity)
        {
            return Result.Success()
               .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
               .ContinueEnsureArgumentNotNullOrEmpty(entity.MetaObjectId, nameof(entity.MetaObjectId))
               //拼接编码
               .Continue(_ =>
               {
                   entity.Code = string.Concat(_formRepository.GetMetaObjectCodeById(entity.MetaObjectId), ".Form.", entity.Code);
                   return _;
               })
               .Continue(_ => base.Add(entity));
        }
    }
}
