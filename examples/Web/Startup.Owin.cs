using System;
using System.Threading.Tasks;
using Owin;
using UnravelExamples.Web.Services;

namespace UnravelExamples.Web
{
    public partial class Startup
    {
        private static void OwinTestRoutes(IAppBuilder app)
        {
            app.Run(context =>
            {
                context.Response.Headers["Content-Type"] = "application/json; charset=utf-8";

                switch (context.Request.Path.Value)
                {
                    case "/Startup_Services":
                        return WriteEnvironment("Startup.Services", Services);
                    case "/IAppBuilder_GetApplicationServices":
                        return WriteEnvironment("IAppBuilder.GetApplicationServices()", app.GetApplicationServices());
                    case "/IOwinContext_GetRequestServices":
                        return WriteEnvironment("IOwinContext.GetRequestServices()", context.GetRequestServices());
                    case "/Throw":
                        throw new ApplicationException("From OWIN");
                    default:
                        context.Response.StatusCode = 404;
                        return context.Response.WriteAsync("Not Found");
                }

                Task WriteEnvironment(string title, IServiceProvider services)
                {
                    var env = new EnvironmentCheck(title, services);
                    return context.Response.WriteAsync(env.ToString());
                }
            });
        }
    }
}
