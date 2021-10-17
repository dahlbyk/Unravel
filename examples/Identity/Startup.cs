using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UnravelExamples.Identity.Startup))]
namespace UnravelExamples.Identity
{
    public partial class Startup : Unravel.Application
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            ConfigureAuthServices(services);
        }

        public override void ConfigureOwin(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
