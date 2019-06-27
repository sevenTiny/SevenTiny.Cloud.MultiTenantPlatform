using SevenTiny.Bantina.Configuration;

namespace SevenTiny.Cloud.Infrastructure.Configs
{
    [ConfigName("Urls")]
    public class UrlsConfig : MySqlColumnConfigBase<CommonConfig>
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
