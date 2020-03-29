using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace SevenTiny.Cloud.MultiTenant.Infrastructure.DependencyInjection
{
    public static class InjectHelper
    {
        /// <summary>
        /// 扫描程序集添加注入
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        public static void AddScoped(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes();
            var interfaces = types.Where(t => t.IsInterface);
            var impTypes = types.Except(interfaces).ToList();
            foreach (var item in interfaces)
            {
                var impType = impTypes.FirstOrDefault(t => item.IsAssignableFrom(t));
                if (impType != null)
                {
                    services.AddScoped(item,impType);
                }
            }
        }
    }
}
