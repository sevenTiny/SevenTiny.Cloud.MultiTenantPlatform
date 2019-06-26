using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.Account.Core.DataAccess;
using SevenTiny.Cloud.Infrastructure.DependencyInjection;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Test.SevenTiny.Cloud.Account.Core")]
namespace SevenTiny.Cloud.Account.Core
{
    public static class ServiceInjector
    {
        public static void InjectCore(this IServiceCollection services)
        {
            services.AddScoped(Assembly.GetExecutingAssembly());

            services.AddScoped<AccountDbContext>();
            services.AddScoped<MultiTenantPlatformDbContext>();
        }
    }
}
