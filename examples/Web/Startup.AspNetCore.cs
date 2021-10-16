using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Owin;
using UnravelExamples.Web.Services;

namespace UnravelExamples.Web
{
    public partial class Startup
    {
        private static void AspNetCoreTestRoutes(IApplicationBuilder app)
        {
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger<Startup>();
            app.Run((HttpContext context) =>
            {
                context.Response.Headers["Content-Type"] = "application/json; charset=utf-8";

                var starting = false;
                var completed = false;
                context.Response.OnStarting(() =>
                {
                    if (completed)
                        throw new InvalidOperationException("OnCompleted before OnStarting!");

                    starting = true;
                    logger.LogInformation("{0} OnStarting", context.Request.Path);
                    return Task.CompletedTask;
                });

                context.Response.OnCompleted(() =>
                {
                    if (!starting)
                        throw new InvalidOperationException("OnCompleted before OnStarting!");

                    completed = true;
                    logger.LogInformation("{0} OnCompleting", context.Request.Path);
                    return Task.CompletedTask;
                });


                switch (context.Request.Path)
                {
                    case "/Startup_Services":
                        return WriteEnvironment("Startup.Services", Services);
                    case "/IApplicationBuilder_ApplicationServices":
                        return WriteEnvironment("IApplicationBuilder.ApplicationServices", app.ApplicationServices);
                    case "/HttpContext_RequestServices":
                        return WriteEnvironment("HttpContext.RequestServices", context.RequestServices);
                    case "/Throw":
                        throw new ApplicationException("From ASP.NET Core");
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
