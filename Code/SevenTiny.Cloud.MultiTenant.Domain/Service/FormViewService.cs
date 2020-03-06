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
       
    }
}
