using System;
using System.Web;
using Unravel.SystemWeb;

[assembly: PreApplicationStartMethod(typeof(ServiceScopeModule), nameof(ServiceScopeModule.InitModule))]
namespace Unravel.SystemWeb
{
    public sealed class ServiceScopeModule : IHttpModule
    {
        public static void InitModule() =>
            HttpApplication.RegisterModule(typeof(ServiceScopeModule));

        void IHttpModule.Init(HttpApplication context)
        {
            context.BeginRequest += OnContextBeginRequest;
        }

        void IHttpModule.Dispose() { }

        private static void OnContextBeginRequest(object sender, EventArgs e)
        {
            var httpContext = ((HttpApplication)sender).Context;
            var scope = httpContext.CreateServiceScope();
            httpContext.DisposeOnPipelineCompleted(scope);
        }
    }
}
