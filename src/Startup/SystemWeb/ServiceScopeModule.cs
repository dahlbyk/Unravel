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
            context.EndRequest += OnContextEndRequest;
        }

        void IHttpModule.Dispose() { }

        private static void OnContextBeginRequest(object sender, EventArgs e) =>
            Context(sender).CreateServiceScope();

        private static void OnContextEndRequest(object sender, EventArgs e) =>
            Context(sender).DisposeServiceScope();

        private static HttpContext Context(object sender) =>
            ((HttpApplication)sender).Context;
    }
}
