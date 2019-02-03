using SevenTiny.Bantina.Caching;
using System;

namespace SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Caching
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
        }
    }
}
