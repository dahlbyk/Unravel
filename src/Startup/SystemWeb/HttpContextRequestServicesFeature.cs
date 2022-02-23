using System;
using System.Web;
using Microsoft.AspNetCore.Http.Features;

namespace Unravel.SystemWeb
{
    /// <summary>
    /// Provides scoped <see cref="IServiceProvider"/> from HttpContext/OWIN to AspNetCore.
    /// </summary>
    public class HttpContextRequestServicesFeature : IServiceProvidersFeature
    {
        private IServiceProvider requestServices;

        public HttpContextRequestServicesFeature(HttpContextBase httpContext)
        {
            RequestServices = httpContext.GetRequestServices();
        }

        public IServiceProvider RequestServices { get; set; }
    }
}
