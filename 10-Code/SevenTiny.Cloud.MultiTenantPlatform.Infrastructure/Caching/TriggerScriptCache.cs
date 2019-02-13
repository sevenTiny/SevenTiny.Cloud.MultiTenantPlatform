using Newtonsoft.Json;
using SevenTiny.Bantina.Caching;
using SevenTiny.Bantina.Redis;
using StackExchange.Redis;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Caching
{
    /// <summary>
    /// 触发器脚本专用缓存
    /// </summary>
    public class TriggerScriptCache
    {
        private static IRedisCache redisCache = new RedisCacheManager("101");
        public static TValue GetSet<TValue>(string script, Func<TValue> func)
        {
            var hashKey = script.GetHashCode().ToString();
            var expired = TimeSpan.FromMinutes(15);

            //redis cache
            var triggerScriptFunc = redisCache.Get(hashKey); ;
            if (string.IsNullOrEmpty(triggerScriptFunc))
            {
                redisCache.Update(hashKey, JsonConvert.SerializeObject(func()));
                return func();
            }

            //local cache
            //var triggerScriptInCache = MemoryCacheHelper.Get<int, TValue>(hashKey);
            //if (triggerScriptInCache == null)
            //{
            //    triggerScriptInCache = MemoryCacheHelper.Put<int, TValue>(hashKey, func(), expired);
            //}

            return JsonConvert.DeserializeObject<TValue>(triggerScriptFunc);
        }

        public static void ClearCache(string script)
        {
            var hashKey = script.GetHashCode().ToString();

            //local cache
            //MemoryCacheHelper.Delete(hashKey);

            //redis cache
            redisCache.Delete(hashKey);
        }
    }
}
