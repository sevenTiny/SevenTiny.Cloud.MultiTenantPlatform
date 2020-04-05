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
    }
}