using SevenTiny.Bantina.Bankinate;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Configs;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    public class MultiTenantPlatformDbContext : MySqlDbContext<MultiTenantPlatformDbContext>
    {
        public MultiTenantPlatformDbContext() : base(ConnectionStringsConfig.Get("MultiTenantPlatformWeb"))
        {
            IsFromCache = false;
        }
    }
}
