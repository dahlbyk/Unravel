using System;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Unravel.AspNet.WebApi.DependencyInjection.Internal
{
    public class GlobalConfigurationStartupFilter : IStartupFilter
    {
        /// <summary>
        ///   Updates <see cref="GlobalConfiguration.Configuration"/> from
        ///   <see cref="IApplicationBuilder.ApplicationServices"/>, if available:
        ///   <list type="bullet">
        ///     <item><see cref="HttpConfiguration.DependencyResolver"/></item>
        ///   </list>
        /// </summary>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return UpdateGlobalConfiguration;

            void UpdateGlobalConfiguration(IApplicationBuilder builder)
            {
                var config = GlobalConfiguration.Configuration;

                var resolver = builder.ApplicationServices.GetService<IDependencyResolver>();
                if (resolver != null)
                    config.DependencyResolver = resolver;

                next(builder);
            }
        }
    }
}
