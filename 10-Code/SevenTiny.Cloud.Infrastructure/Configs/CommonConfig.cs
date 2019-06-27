using SevenTiny.Bantina.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.Infrastructure.Configs
{
    [ConfigName("Common")]
    public class CommonConfig : MySqlColumnConfigBase<CommonConfig>
    {
        [ConfigProperty]
        public string InternationalizationLanguage { get; set; }
    }
}
