using System;
using System.Web.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Unravel.AspNet.Mvc.DependencyInjection.Internal
{
    public class DependencyResolverStartupFilter : IStartupFilter
    {
        /// <summary>
        ///   Calls <see cref="DependencyResolver.SetResolver(IDependencyResolver)"/>
        ///   with an <see cref="IDependencyResolver"/> from
        ///   <see cref="IApplicationBuilder.ApplicationServices"/>, if available.
        /// </summary>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return SetDependencyResolver;

            void SetDependencyResolver(IApplicationBuilder builder)
            {
                var resolver = builder.ApplicationServices.GetService<IDependencyResolver>();
                if (resolver != null)
                    DependencyResolver.SetResolver(resolver);

                next(builder);
            }
        }
    }
}
