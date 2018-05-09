using SevenTiny.Bantina.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Configs
{
    [ConfigName("ConnectionStrings")]
    public class ConnectionStringsConfig : ConfigBase<ConnectionStringsConfig>
    {
        public string Key { get; set; }
        public string Value { get; set; }

        private static Dictionary<string, string> _configs;

        public static string Get(string key)
        {
            if (_configs == null)
            {
                _configs = Configs.ToDictionary(t => t.Key, v => v.Value);
            }
            if (_configs.ContainsKey(key))
            {
                return _configs[key];
            }
            return string.Empty;
        }
    }
}
