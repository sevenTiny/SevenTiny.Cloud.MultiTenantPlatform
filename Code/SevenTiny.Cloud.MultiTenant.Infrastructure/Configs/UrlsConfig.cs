using SevenTiny.Bantina.Configuration;

namespace SevenTiny.Cloud.MultiTenant.Infrastructure.Configs
{
    [ConfigName("Urls")]
    public class UrlsConfig : MySqlColumnConfigBase<UrlsConfig>
    {
        [ConfigProperty]
        public string DataApiUrl { get; set; }
        [ConfigProperty]
        public string DevelopmentWebUrl { get; set; }
        [ConfigProperty]
        public string SettingWeb { get; set; }
        [ConfigProperty]
        public string OfficialWeb { get; set; }
        [ConfigProperty]
        public string Account { get; set; }
    }
}
