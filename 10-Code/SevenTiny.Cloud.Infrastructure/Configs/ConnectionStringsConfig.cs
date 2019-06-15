using SevenTiny.Bantina.Configuration;

namespace SevenTiny.Cloud.Infrastructure.Configs
{
    [ConfigName("ConnectionStrings")]
    public class ConnectionStringsConfig : MySqlColumnConfigBase<ConnectionStringsConfig>
    {
        public static ConnectionStringsConfig Instance = new ConnectionStringsConfig();

        [ConfigProperty]
        public string mongodb39911 { get; set; }
        [ConfigProperty]
        public string MultiTenantPlatformWeb { get; set; }
        [ConfigProperty]
        public string mysql39901 { get; set; }

        protected override string _ConnectionString => GetConnectionStringFromAppSettings("SeventinyConfig");
    }
}
