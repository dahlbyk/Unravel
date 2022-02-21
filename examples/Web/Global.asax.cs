using System.Web.Http;

namespace UnravelExamples.Web
{
    public partial class Startup : Unravel.Application
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
