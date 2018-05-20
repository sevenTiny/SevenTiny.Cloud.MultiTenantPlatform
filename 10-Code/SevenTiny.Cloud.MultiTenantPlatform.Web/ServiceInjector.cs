using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.MultiTenantPlatform.Application;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository;

namespace SevenTiny.Cloud.MultiTenantPlatform.Web
{
    /// <summary>
    /// 依赖注入器
    /// </summary>
    public static class ServiceInjector
    {
        //使用.netcore自带的DI
        public static IServiceCollection NetCoreInject(IServiceCollection services)
        {
            //repository
            services.AddScoped<MultiTenantPlatformDbContext>();
            services.AddScoped<IRepository<DomainModel.Entities.Application>, ApplicationRepository>();
            services.AddScoped<IRepository<MetaObject>, MetaObjectRepository>();
            services.AddScoped<IRepository<MetaField>, MetaFieldRepository>();

            //service
            services.AddScoped<IMetaFieldService, MetaFieldService>();


            return services;
        }

        //使用Autofac
        public static IServiceCollection AutofacInject(IServiceCollection services)
        {
            return services;
        }
    }
}
