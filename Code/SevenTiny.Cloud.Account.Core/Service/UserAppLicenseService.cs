using SevenTiny.Cloud.Account.Core.ServiceContract;
using SevenTiny.Cloud.Account.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenTiny.Cloud.Account.Core.Service
{
    public class UserAppLicenseService : IUserAppLicenseService
    {
        ITenantApplicationLicenseService _tenantApplicationLicenseService;
        public UserAppLicenseService(ITenantApplicationLicenseService tenantApplicationLicenseService)
        {
            _tenantApplicationLicenseService = tenantApplicationLicenseService;
        }

        public List<UserApplicationLicenseDto> GetTenantApplicationLicenseDtos(int tenantId)
        {
            var result = new List<UserApplicationLicenseDto>();
            //获取到有效期内的应用许可
            var license = _tenantApplicationLicenseService.GetUnDeletedValidityEntitiesByTenantId(tenantId);
            if (license != null && license.Any())
            {
                result = license.Select(t => new UserApplicationLicenseDto
                {
                    ApplicationId = t.ApplicationId,
                    TenantId = t.TenantId,
                    ExpirationTime = t.ExpirationTime,
                    AppIdentity = 0//用户身份稍后完善业务补充
                }).ToList();
            }
            return result;
        }
    }
}
