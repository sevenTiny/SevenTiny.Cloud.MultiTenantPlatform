using SevenTiny.Bantina.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Configs
{
    [ConfigName("ConnectionStrings")]
    public class ConnectionStringsConfig : MySqlConfigBase<ConnectionStringsConfig>
    {
        private static ConnectionStringsConfig Instance = new ConnectionStringsConfig();

        [ConfigProperty]
        public string Key { get; set; }
        [ConfigProperty]
        public string Value { get; set; }

        protected override string _ConnectionString => GetConnectionStringFromAppSettings("SeventinyConfig");

        private static Dictionary<string, string> _configs;

        public static string Get(string key)
        {
            if (_configs == null)
            {
                _configs = Instance.GetConfigList().ToDictionary(t => t.Key, v => v.Value);
            }
            if (_configs.ContainsKey(key))
            {
                return _configs[key];
            }
            return string.Empty;
        }
    }
}
