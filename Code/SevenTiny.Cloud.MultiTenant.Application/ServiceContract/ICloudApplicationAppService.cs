using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using System;

namespace SevenTiny.Cloud.MultiTenant.Application.ServiceContract
{
    public interface ICloudApplicationAppService
    {
        Result<CloudApplication> Delete(Guid id);
    }
}
