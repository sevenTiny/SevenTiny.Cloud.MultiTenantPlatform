using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.DependencyInjection;
using System.Reflection;

namespace SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine
{
    public static class ServiceInjector
    {
        public static void InjectTriggerScriptEngine(this IServiceCollection services)
        {
            services.AddScoped(Assembly.GetExecutingAssembly());
        }
    }
}
