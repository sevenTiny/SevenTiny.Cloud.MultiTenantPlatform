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

        public DbSet<CloudApplication> CloudApplication { get; set; }
        public DbSet<CloudInterface> CloudInterface { get; set; }
        public DbSet<ConfigField> ConfigField { get; set; }
        public DbSet<DataSource> DataSource { get; set; }
        public DbSet<FormView> FormView { get; set; }
        public DbSet<Identity> Identity { get; set; }
        public DbSet<IndexView> IndexView { get; set; }
        public DbSet<ListView> ListView { get; set; }
        public DbSet<Menue> Menue { get; set; }
        public DbSet<MetaField> MetaField { get; set; }
        public DbSet<MetaObject> MetaObject { get; set; }
        public DbSet<SearchCondition> SearchCondition { get; set; }
        public DbSet<SearchConditionNode> SearchConditionNode { get; set; }
        public DbSet<TriggerScript> TriggerScript { get; set; }
    }
}
