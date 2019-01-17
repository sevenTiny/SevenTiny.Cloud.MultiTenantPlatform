using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.MultiTenantPlatform.Domain;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.DependencyInjection;
using System.Reflection;

namespace SevenTiny.Cloud.MultiTenantPlatform.Application
{
    public static class ServiceInjector
    {
        public static void InjectApplication(this IServiceCollection services)
        {
            services.AddScoped(Assembly.GetExecutingAssembly());

            services.InjectDomain();
        }
    }
}
