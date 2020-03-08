using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.ScriptEngine;
using SevenTiny.Cloud.ScriptEngine.CSharp;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Infrastructure.DependencyInjection;
using System.Reflection;

namespace SevenTiny.Cloud.MultiTenant.Domain
{
    public static class ServiceInjector
    {
        public static void InjectDomain(this IServiceCollection services)
        {
            services.AddScoped(Assembly.GetExecutingAssembly());

            //脚本引擎
            services.AddSingleton<IDynamicScriptEngine, CSharpDynamicScriptEngine>();
            services.AddScoped<MultiTenantPlatformDbContext>();
        }
    }
}
