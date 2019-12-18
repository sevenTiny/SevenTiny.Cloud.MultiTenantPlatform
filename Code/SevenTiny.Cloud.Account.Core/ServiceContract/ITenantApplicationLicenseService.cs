using SevenTiny.Bantina;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.Account.Core.ServiceContract
{
    public interface ITenantApplicationLicenseService : ICommonInfoRepository<TenantApplicationLicense>
    {
        List<TenantApplicationLicense> GetUnDeletedEntitiesByTenantId(int tenantId);
        /// <summary>
        /// 获取在有效期间内的租户应用许可
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        List<TenantApplicationLicense> GetUnDeletedValidityEntitiesByTenantId(int tenantId);
        Result EnableApplication(int id);
        Result DisableApplication(int id);
        Dictionary<int, string> GetApplicationIdNameDic();
    }
}
