using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UnravelExamples.Identity.Startup))]
namespace UnravelExamples.Identity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
