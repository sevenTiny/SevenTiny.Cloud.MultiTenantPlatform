using SevenTiny.Cloud.Account.Core.DataAccess;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Repository;
using SevenTiny.Cloud.Account.Core.ServiceContract;

namespace SevenTiny.Cloud.Account.Core.Service
{
    public class TenantInfoService : CommonInfoRepository<TenantInfo>, ITenantInfoService
    {
        public TenantInfoService(AccountDbContext accountDbContext) : base(accountDbContext)
        {
        }
    }
}
