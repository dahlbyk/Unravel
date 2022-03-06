using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Unravel.AspNet.Mvc.DependencyInjection.Internal
{
    public sealed class ServiceProviderDependencyResolver : IDependencyResolver
    {
        /// <inheritdoc/>
        public object GetService(Type serviceType) =>
            HttpContext.Current.GetRequestServices()
                .GetService(serviceType);

        /// <inheritdoc/>
        public IEnumerable<object> GetServices(Type serviceType) =>
            HttpContext.Current.GetRequestServices()
                .GetServices(serviceType);
    }
}
