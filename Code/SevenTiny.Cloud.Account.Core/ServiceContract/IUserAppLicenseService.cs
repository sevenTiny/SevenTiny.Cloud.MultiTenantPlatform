using SevenTiny.Cloud.Account.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.Account.Core.ServiceContract
{
    public interface IUserAppLicenseService
    {
        List<UserApplicationLicenseDto> GetTenantApplicationLicenseDtos(int tenantId);
    }
}
