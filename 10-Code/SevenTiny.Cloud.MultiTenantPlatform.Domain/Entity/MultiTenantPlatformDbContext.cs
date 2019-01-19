using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Configs;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity
{
    [DataBase("MultiTenantPlatformWeb")]
    public class MultiTenantPlatformDbContext : MySqlDbContext<MultiTenantPlatformDbContext>
    {
        public MultiTenantPlatformDbContext() : base(ConnectionStringsConfig.Get("MultiTenantPlatformWeb"))
        {
            //开启一级缓存
            OpenQueryCache = true;
        }
    }
}
