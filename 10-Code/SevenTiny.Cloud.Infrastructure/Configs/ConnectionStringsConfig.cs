using SevenTiny.Bantina.Configuration;

namespace SevenTiny.Cloud.Infrastructure.Configs
{
    [ConfigName("ConnectionStrings")]
    public class ConnectionStringsConfig : MySqlColumnConfigBase<ConnectionStringsConfig>
    {
        [ConfigProperty]
        public string mongodb39911 { get; set; }
        [ConfigProperty]
        public string Account { get; set; }
        [ConfigProperty]
        public string MultiTenantPlatformWeb { get; set; }
        [ConfigProperty]
        public string mysql39901 { get; set; }
    }
}
