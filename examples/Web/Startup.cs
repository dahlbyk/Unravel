using Microsoft.Extensions.DependencyInjection;
using UnravelExamples.Web.Services;

namespace UnravelExamples.Web
{
    public partial class Startup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            Counters.Register(services);
        }
    }
}
