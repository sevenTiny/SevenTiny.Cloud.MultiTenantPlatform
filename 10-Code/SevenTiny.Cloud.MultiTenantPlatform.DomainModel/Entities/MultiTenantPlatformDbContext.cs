using SevenTiny.Bantina.Bankinate;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities
{
    public class MultiTenantPlatformDbContext : MySqlDbContext<MultiTenantPlatformDbContext>
    {
        public MultiTenantPlatformDbContext() : base("server=101.201.66.247;Port=39901;database=MultiTenantPlatform;uid=root;pwd=CYj(9yyz*8;Allow User Variables=true;")
        {
            IsFromCache = false;
        }
    }
}
