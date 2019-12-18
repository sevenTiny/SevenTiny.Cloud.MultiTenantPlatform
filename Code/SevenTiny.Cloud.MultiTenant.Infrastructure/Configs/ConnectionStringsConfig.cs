using SevenTiny.Bantina.Configuration;

namespace SevenTiny.Cloud.MultiTenant.Infrastructure.Configs
{
    [ConfigName("ConnectionStrings")]
    public class ConnectionStringsConfig : MySqlColumnConfigBase<ConnectionStringsConfig>
    {
        [ConfigProperty]
        public string mongodb39911 { get; set; }
        [ConfigProperty]
        public string MultiTenantAccount { get; set; }
        [ConfigProperty]
        public string MultiTenantPlatformWeb { get; set; }
        [ConfigProperty]
        public string mysql39901 { get; set; }
    }
}
