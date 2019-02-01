using SevenTiny.Bantina.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Caching
{
    /// <summary>
    /// 触发器脚本专用缓存
    /// </summary>
    public class TriggerScriptCache
    {
        public static TValue GetSet<TKey, TValue>(TKey key, Func<TValue> func)
        {
            var triggerScriptInCache = MemoryCacheHelper.Get<TKey, TValue>(key);
            if (triggerScriptInCache == null)
            {
                triggerScriptInCache = MemoryCacheHelper.Put<TKey, TValue>(key, func());
            }
            return triggerScriptInCache;
        }
    }
}
