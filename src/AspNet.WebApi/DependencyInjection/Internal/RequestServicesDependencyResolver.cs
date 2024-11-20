using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace Unravel.AspNet.WebApi.DependencyInjection.Internal
{
    /// <summary>
    /// WebApi <see cref="IDependencyResolver"/> that resolves services from
    /// <see cref="HttpContextServicesExtensions.GetRequestServices(HttpContext?)"/>,
    /// or from <see cref="RequestServicesDependencyResolver.Fallback"/> (if provided).
    /// </summary>
    public sealed class RequestServicesDependencyResolver : RequestServicesDependencyScope, IDependencyResolver
    {
        /// <summary>
        /// Scope is managed by <see cref="Unravel.SystemWeb.ServiceScopeModule"/>,
        /// but the fallback <see cref="IDependencyResolver"/> might have its own scope.
        /// In that case we return a new <see cref="RequestServicesDependencyScope"/>.
        /// </summary>
        /// <returns>The current instance.</returns>
        public IDependencyScope BeginScope() =>
            Fallback is IDependencyResolver resolver
            && resolver.BeginScope() is { } scope
            && !ReferenceEquals(scope, this)
                ? new RequestServicesDependencyScope { Fallback = scope }
                : this;
    }

    /// <summary>
    /// WebApi <see cref="IDependencyScope"/> that resolves services from
    /// <see cref="HttpContextServicesExtensions.GetRequestServices(HttpContext?)"/>,
    /// or from <see cref="RequestServicesDependencyResolver.Fallback"/> (if provided).
    /// </summary>
    public class RequestServicesDependencyScope : IDependencyScope
    {
        /// <summary>
        /// An optional fallback to use for services not resolved from
        /// <see cref="HttpContextServicesExtensions.GetRequestServices(HttpContext?)"/>.
        /// </summary>
        public IDependencyScope? Fallback { get; set; }

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

        /// <summary>
        ///   Scope is managed by <see cref="Unravel.SystemWeb.ServiceScopeModule"/>.
        /// </summary>
        public void Dispose() =>
            Fallback?.Dispose();
    }
}
