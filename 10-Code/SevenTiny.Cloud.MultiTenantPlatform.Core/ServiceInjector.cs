using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.MultiTenantPlatform.Core.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.DependencyInjection;
using System.Reflection;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core
{
    public static class ServiceInjector
    {
        public static void InjectDomain(this IServiceCollection services)
        {
            services.AddScoped(Assembly.GetExecutingAssembly());

            services.AddScoped<MultiTenantPlatformDbContext>();
            services.AddScoped<MultiTenantDataDbContext>();
        }
    }
}
