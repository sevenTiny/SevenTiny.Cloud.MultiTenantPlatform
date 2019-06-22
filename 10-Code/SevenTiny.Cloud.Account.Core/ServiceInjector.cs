using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.Account.DataAccess;
using SevenTiny.Cloud.Infrastructure.DependencyInjection;
using System.Reflection;

namespace SevenTiny.Cloud.Account.Core
{
    public static class ServiceInjector
    {
        public static void InjectCore(this IServiceCollection services)
        {
            services.AddScoped(Assembly.GetExecutingAssembly());

            //脚本引擎
            services.AddScoped<AccountDbContext>();
        }
    }
}
