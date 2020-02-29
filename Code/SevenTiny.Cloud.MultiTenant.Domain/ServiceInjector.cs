using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.ScriptEngine;
using SevenTiny.Cloud.MultiTenant.Domain.DataAccess;
using SevenTiny.Cloud.MultiTenant.Infrastructure.DependencyInjection;
using SevenTiny.Cloud.MultiTenant.UI.DataAccess;
using System.Reflection;

namespace SevenTiny.Cloud.MultiTenant.Domain
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
