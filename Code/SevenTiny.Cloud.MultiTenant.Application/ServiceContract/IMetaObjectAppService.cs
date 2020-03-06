using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;

namespace SevenTiny.Cloud.MultiTenant.Application.ServiceContract
{
    public interface IMetaObjectAppService
    {
        Result<MetaObject> Delete(Guid id);
    }
}
