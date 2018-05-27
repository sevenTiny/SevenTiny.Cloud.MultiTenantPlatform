﻿using Microsoft.Extensions.DependencyInjection;
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
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IMetaObjectRepository, MetaObjectRepository>();
            services.AddScoped<IMetaFieldRepository, MetaFieldRepository>();

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