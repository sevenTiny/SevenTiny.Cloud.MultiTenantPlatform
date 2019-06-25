using SevenTiny.Bantina;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Repository;

namespace SevenTiny.Cloud.Account.Core.ServiceContract
{
    public interface ITenantInfoService : ICommonInfoRepository<TenantInfo>
    {
        bool Exist(int tenantId);
    }
}
