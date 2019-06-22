using SevenTiny.Cloud.Account.Core.DataAccess;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Repository;
using SevenTiny.Cloud.Account.Core.ServiceContract;

namespace SevenTiny.Cloud.Account.Core.Service
{
    public class TenantApplicationLicenseService : CommonInfoRepository<TenantApplicationLicense>, ITenantApplicationLicenseService
    {
        public TenantApplicationLicenseService(AccountDbContext accountDbContext) : base(accountDbContext)
        {
        }
    }
}
