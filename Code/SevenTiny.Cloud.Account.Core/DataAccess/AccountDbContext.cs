using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.Infrastructure.Configs;

namespace SevenTiny.Cloud.Account.Core.DataAccess
{
    [DataBase("MultiTenantAccount")]
    public class AccountDbContext : MySqlDbContext<AccountDbContext>
    {
        public AccountDbContext() : base(ConnectionStringsConfig.Instance.MultiTenantAccount)
        {
            //开启一级缓存
            OpenQueryCache = false;
            OpenTableCache = false;
            //用redis做缓存
            CacheMediaType = CacheMediaType.Local;
        }
    }
}
