using SevenTiny.Bantina.Bankinate.Attributes;
using SevenTiny.Bantina.Bankinate.Helpers;
using SevenTiny.Bantina.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Configs
{
    [ConfigName(Name = "Redis")]
    public class RedisConfig : ConfigBase<RedisConfig>
    {
        [Column]
        public string KeySpace { get; set; }
        [Column]
        public string Key { get; set; }
        [Column]
        public string Value { get; set; }
        [Column]
        public string Description { get; set; }

        private static Dictionary<string, Dictionary<string, string>> dictionary;

        private static void Initial()
        {
            var group = Configs.GroupBy(t => t.KeySpace).Select(t => new { KeySpace = t.Key, RedisConfig = t }).ToList();
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
