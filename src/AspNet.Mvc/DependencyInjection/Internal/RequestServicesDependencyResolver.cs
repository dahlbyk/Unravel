using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Unravel.AspNet.Mvc.DependencyInjection.Internal
{
    /// <summary>
    /// MVC <see cref="IDependencyResolver"/> that resolves services from
    /// <see cref="HttpContextServicesExtensions.GetRequestServices(HttpContext?)"/>,
    /// or from <see cref="RequestServicesDependencyResolver.Fallback"/> (if provided).
    /// </summary>
    public sealed class RequestServicesDependencyResolver : IDependencyResolver
    {
        /// <summary>
        /// An optional fallback to use for services not resolved from
        /// <see cref="HttpContextServicesExtensions.GetRequestServices(HttpContext?)"/>.
        /// </summary>
        public IDependencyResolver? Fallback { get; set; }

        /// <inheritdoc/>
        public object? GetService(Type serviceType) =>
            HttpContext.Current.GetRequestServices()
                .GetService(serviceType)
            ?? Fallback?.GetService(serviceType);

        /// <inheritdoc/>
        public IEnumerable<object>? GetServices(Type serviceType) =>
            HttpContext.Current.GetRequestServices()
                .GetServices(serviceType)
            ?? Fallback?.GetServices(serviceType);
    }
}
