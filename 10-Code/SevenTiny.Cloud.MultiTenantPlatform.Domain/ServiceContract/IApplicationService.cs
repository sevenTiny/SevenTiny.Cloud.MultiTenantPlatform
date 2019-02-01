using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Repository;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract
{
    public interface IApplicationService : ICommonInfoRepository<Application>
    {
        bool ExistForSameName(string name);
        bool ExistForSameNameAndNotSameId(string name, int id);
    }
}
