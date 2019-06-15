using SevenTiny.Bantina.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.Infrastructure.Configs
{
    [ConfigName("Common")]
    public class CommonConfig : MySqlColumnConfigBase<CommonConfig>
    {
        public static CommonConfig Instance = new CommonConfig();

        [ConfigProperty]
        public string InternationalizationLanguage { get; set; }
        [ConfigProperty]
        public string DataApiUrl { get; set; }

        protected override string _ConnectionString => GetConnectionStringFromAppSettings("SeventinyConfig");
    }
}
