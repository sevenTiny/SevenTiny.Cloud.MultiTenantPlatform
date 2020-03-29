using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Cloud.Infrastructure.Configs;
using System;

namespace SevenTiny.Cloud.Account.Core.DataAccess
{
    [DataBase("MultiTenantPlatformWeb")]
    public class MultiTenantPlatformDbContext : MySqlDbContext<MultiTenantPlatformDbContext>
    {
        public MultiTenantPlatformDbContext() : base(ConnectionStringsConfig.Instance.MultiTenantPlatformWeb)
        {
            //开启一级缓存
            OpenQueryCache = false;
            OpenTableCache = false;
            //用redis做缓存
            CacheMediaType = CacheMediaType.Local;
            CacheMediaServer = $"{RedisConfig.Get("101", "Server")}:{RedisConfig.Get("101", "Port")}";//redis服务器地址以及端口号
        }
    }
}
