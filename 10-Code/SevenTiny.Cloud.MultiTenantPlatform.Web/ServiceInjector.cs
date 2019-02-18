using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.MultiTenantPlatform.Domain;
using SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web
{
    /// <summary>
    /// 依赖注入器
    /// </summary>
    public static class ServiceInjector
    {
        //使用.netcore自带的DI
        public static void InjectWeb(this IServiceCollection services)
        {
            services.InjectDomain();
            services.InjectTriggerScriptEngine();
        }
    }
}
