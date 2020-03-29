using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.MultiTenant.Application;
using SevenTiny.Cloud.MultiTenant.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Bootstrapper
{
    public class BootStrap
    {
        public static void Start()
        {

        }

        public static void InjectDependency(IServiceCollection services)
        {
            services.InjectApplication();
            services.InjectDomain();
        }
    }
}
