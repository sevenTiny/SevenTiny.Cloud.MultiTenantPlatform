using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Infrastructure.Configs;
using System;

namespace SevenTiny.Cloud.MultiTenant.Domain.DbContext
{
    [DataBase("MultiTenantPlatformWeb")]
    internal class MultiTenantPlatformDbContext : MySqlDbContext<MultiTenantPlatformDbContext>
    {
        public MultiTenantPlatformDbContext() : base(ConnectionStringsConfig.Instance.MultiTenantPlatformWeb)
        {
            ////开启一级缓存
            //OpenQueryCache = false;
            //OpenTableCache = false;
            ////用redis做缓存
            //CacheMediaType = CacheMediaType.Local;
            //CacheMediaServer = $"{RedisConfig.Get("101", "Server")}:{RedisConfig.Get("101", "Port")}";//redis服务器地址以及端口号
        }

        public DbSet<Application> Application { get; set; }
    }
}
