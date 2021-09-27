using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Owin;
using UnravelExamples.Web.Services;

namespace UnravelExamples.Web
{
    public partial class Startup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            Counters.Register(services);

            services.AddAspNetMvc()
                .AddControllersAsServices()
                ;

            services.AddHttpContextAccessor();
        }

        public override void ConfigureOwin(IAppBuilder app)
        {
            app.Map("/OWIN", OwinTestRoutes);

            app.UseAspNetCore();

            app.Map("/OWIN-after-UseAspNetCore", OwinTestRoutes);
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.Map("/AspNetCore", AspNetCoreTestRoutes);
        }
    }
}
