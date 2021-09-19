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
        }

        public override void Configure(IAppBuilder app)
        {
            app.Map("/OWIN", OwinTestRoutes);
        }
    }
}
