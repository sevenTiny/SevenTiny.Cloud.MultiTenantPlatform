using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.MultiTenant.Application;
using SevenTiny.Cloud.MultiTenant.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Bootstrapper
{
    public class BootStrap
    {
        public static void Start()
        {

        }

        public static IServiceProvider ServiceProviderInstance { get; set; }

        public static IInterface GetService<IInterface>()
        {
            if (ServiceProviderInstance == null)
                throw new ArgumentNullException($"ServiceProviderInstance is null,please set value in bootstrap");

            return ServiceProviderInstance.GetServices<IInterface>().First();
        }

        public static void InjectDependency(IServiceCollection services)
        {
            services.InjectApplication();
            services.InjectDomain();
        }
    }
}
