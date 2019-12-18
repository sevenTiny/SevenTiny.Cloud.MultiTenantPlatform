using Newtonsoft.Json;
using SevenTiny.Bantina.Caching;
using SevenTiny.Bantina.Redis;
using StackExchange.Redis;
using System;

namespace SevenTiny.Cloud.Infrastructure.Caching
{
    /// <summary>
    /// 触发器脚本专用缓存
    /// </summary>
    public class TriggerScriptCache
    {
        public static TValue GetSet<TValue>(string script, Func<TValue> func)
        {
            var hashKey = script.GetHashCode();
            var expired = TimeSpan.FromMinutes(15);

            var triggerScriptInCache = MemoryCacheHelper.Get<int, TValue>(hashKey);
            if (triggerScriptInCache == null)
            {
                triggerScriptInCache = MemoryCacheHelper.Put<int, TValue>(hashKey, func(), expired);
            }

            return triggerScriptInCache;
        }

        public static void ClearCache(string script)
        {
            var hashKey = script.GetHashCode();
            MemoryCacheHelper.Delete(hashKey);
            //事实上，这是两个项目，内存中缓存是分应用程序域的，web的缓存和dataapi的缓存不是一套，并不能实现清楚缓存的功能
            //如果修改脚本，则是废弃旧脚本，重新存了个新的而已
            //因此，本清楚缓存的功能使用内存集合是不可用的！
        }
    }
}
