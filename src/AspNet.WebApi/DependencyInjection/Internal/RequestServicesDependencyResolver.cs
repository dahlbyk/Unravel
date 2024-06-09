using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace Unravel.AspNet.WebApi.DependencyInjection.Internal
{
    /// <summary>
    /// WebApi <see cref="IDependencyResolver"/> that resolves services from
    /// <see cref="HttpContextServicesExtensions.GetRequestServices(HttpContext?)"/>.
    /// </summary>
    public sealed class RequestServicesDependencyResolver : IDependencyResolver
    {
        /// <inheritdoc/>
        public object? GetService(Type serviceType) =>
            HttpContext.Current.GetRequestServices()
                .GetService(serviceType);

        /// <inheritdoc/>
        public IEnumerable<object>? GetServices(Type serviceType) =>
            HttpContext.Current.GetRequestServices()
                .GetServices(serviceType);

        /// <summary>
        ///   Scope is managed by <see cref="Unravel.SystemWeb.ServiceScopeModule"/>.
        /// </summary>
        /// <returns>The current instance.</returns>
        public IDependencyScope BeginScope() => this;

        /// <summary>
        ///   Scope is managed by <see cref="Unravel.SystemWeb.ServiceScopeModule"/>.
        /// </summary>
        public void Dispose() { }
    }
}
