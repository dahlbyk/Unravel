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
        }

        public override void ConfigureOwin(IAppBuilder app)
        {
            app.Map("/OWIN", OwinTestRoutes);
        }
    }
}
