using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.ScriptEngine;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using SevenTiny.Cloud.Infrastructure.DependencyInjection;
using SevenTiny.Cloud.MultiTenantPlatform.UI.DataAccess;
using System.Reflection;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core
{
    public static class ServiceInjector
    {
        public static void InjectCore(this IServiceCollection services)
        {
            services.AddScoped(Assembly.GetExecutingAssembly());

            //脚本引擎
            services.AddSingleton<IScriptEngineProvider, ScriptEngineProvider>();
            services.AddScoped<MultiTenantPlatformDbContext>();
            services.AddScoped<MultiTenantDataDbContext>();
        }
    }
}
