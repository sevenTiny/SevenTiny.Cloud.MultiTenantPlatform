using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.ScriptEngine;
using SevenTiny.Cloud.ScriptEngine.CSharp;
using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Infrastructure.DependencyInjection;
using System.Reflection;

namespace SevenTiny.Cloud.MultiTenant.Application
{
    public static class ServiceInjector
    {
        public static void InjectApplication(this IServiceCollection services)
        {
            services.AddScoped(Assembly.GetExecutingAssembly());
        }
    }
}
