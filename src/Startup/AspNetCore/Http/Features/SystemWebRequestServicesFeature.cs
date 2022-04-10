using System;
using System.Web;
using Microsoft.AspNetCore.Http.Features;

namespace Unravel.AspNetCore.Http.Features
{
    /// <summary>
    /// Provides scoped <see cref="IServiceProvider"/> from HttpContext/OWIN to AspNetCore.
    /// </summary>
    public class SystemWebRequestServicesFeature : IServiceProvidersFeature
    {
        public SystemWebRequestServicesFeature(HttpContextBase httpContext)
        {
            RequestServices = httpContext.GetRequestServices();
        }

        public IServiceProvider RequestServices { get; set; }
    }
}
