using SevenTiny.Bantina;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.Account.Core.ServiceContract
{
    public interface ITenantApplicationLicenseService : ICommonInfoRepository<TenantApplicationLicense>
    {
        List<TenantApplicationLicense> GetUnDeletedEntitiesByTenantId(int tenantId);
        Result EnableApplication(int id);
        Result DisableApplication(int id);
        Dictionary<int, string> GetApplicationIdNameDic();
    }
}
