using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SevenTiny.Cloud.MultiTenantPlatform.Domain;
using SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine;

namespace SevenTiny.Cloud.MultiTenantPlatform.DataApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //start 7tiny ---
            //session support
            services.AddDistributedMemoryCache();
            services.AddSession();
            //DI
            services.InjectDomain();
            services.InjectTriggerScriptEngine();
            //end 7tiny ---

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
