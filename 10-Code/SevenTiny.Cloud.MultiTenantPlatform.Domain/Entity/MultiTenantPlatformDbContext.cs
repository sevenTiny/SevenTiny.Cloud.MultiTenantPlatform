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
            //用redis做缓存
            CacheMediaType = Bantina.Bankinate.Cache.CacheMediaType.Redis;
            CacheMediaServer = $"{RedisConfig.Get("101", "Server")}:{RedisConfig.Get("101", "Port")}";//redis服务器地址以及端口号
        }
    }
}
