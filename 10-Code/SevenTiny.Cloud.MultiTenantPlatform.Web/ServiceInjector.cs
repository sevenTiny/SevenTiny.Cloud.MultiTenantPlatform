using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web
{
    public static class ServiceInjector
    {
        public static IServiceCollection Inject(IServiceCollection services)
        {
            services.AddScoped<MultiTenantPlatformDbContext>();
            services.AddScoped<IRepository<DomainModel.Entities.Application>, ApplicationRepository>();
            services.AddScoped<IRepository<MetaObject>, MetaObjectRepository>();

            return services;
        }
    }
}
