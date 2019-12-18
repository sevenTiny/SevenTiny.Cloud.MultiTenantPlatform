using SevenTiny.Bantina;
using SevenTiny.Cloud.Account.Core.Entity;
using SevenTiny.Cloud.Account.Core.Repository;
using System.Collections.Generic;

namespace SevenTiny.Cloud.Account.Core.ServiceContract
{
    public interface IUserAccountService : ICommonInfoRepository<UserAccount>
    {
        Result ValidateRegisterdByEmail(string email);
        Result SignUpByEmail(UserAccount userAccount);
        Result<UserAccount> LoginByEmail(UserAccount userAccount);
        Result<List<UserAccount>> GetUserAccountsByTenantId(int tenantId);
    }
}
