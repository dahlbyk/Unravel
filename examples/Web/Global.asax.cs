using System.Web.Http;
using System.Web.Optimization;

namespace UnravelExamples.Web
{
    public partial class Startup : Unravel.Application
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
