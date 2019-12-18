using SevenTiny.Bantina.Configuration;
using SevenTiny.Bantina.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Infrastructure.Configs
{
    [ConfigName("Redis")]
    public class RedisConfig : MySqlRowConfigBase<RedisConfig>
    {
        [ConfigProperty]
        public string KeySpace { get; set; }
        [ConfigProperty]
        public string Key { get; set; }
        [ConfigProperty]
        public string Value { get; set; }
        [ConfigProperty]
        public string Description { get; set; }

        private static Dictionary<string, Dictionary<string, string>> dictionary;

        private static void Initial()
        {
            var group = Instance.GroupBy(t => t.KeySpace).Select(t => new { KeySpace = t.Key, RedisConfig = t }).ToList();
            dictionary = new Dictionary<string, Dictionary<string, string>>();
            foreach (var item in group)
            {
                var innerDic = new Dictionary<string, string>();
                foreach (var config in item.RedisConfig)
                {
                    innerDic.AddOrUpdate(config.Key, config.Value);
                }
                dictionary.AddOrUpdate(item.KeySpace, innerDic);
            }
        }

        /// <summary>
        /// get redis config value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string keySpace, string key)
        {
            try
            {
                if (dictionary != null && dictionary.ContainsKey(keySpace))
                {
                    if (!dictionary[keySpace].ContainsKey(key))
                    {
                        Initial();
                    }
                    return dictionary[keySpace][key] ?? throw new ArgumentNullException($"Redis Config of keyspace({keySpace}), key ({key}) not exist or error value!");
                }
                Initial();
                if (!dictionary.ContainsKey(keySpace))
                {
                    throw new ArgumentNullException($"Redis Config of keyspace({keySpace}) not exist or error value!");
                }
                return dictionary[keySpace][key];
            }
            catch (Exception)
            {
                throw new ArgumentNullException($"Redis Config of keyspace({keySpace}), key ({key}) not exist or error value!");
            }
        }
    }
}
