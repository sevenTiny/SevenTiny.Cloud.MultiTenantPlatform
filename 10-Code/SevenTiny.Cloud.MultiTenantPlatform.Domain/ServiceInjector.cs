using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.DependencyInjection;
using System.Reflection;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain
{
    public static class ServiceInjector
    {
        public static void InjectDomain(this IServiceCollection services)
        {
            services.AddScoped(Assembly.GetExecutingAssembly());

            services.AddScoped<MultiTenantPlatformDbContext>();
        }
    }
}
