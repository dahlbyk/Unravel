using System.Web.Http.Dependencies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Unravel.AspNet.WebApi.DependencyInjection.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///   Extension methods for setting up ASP.NET Web API services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class UnravelAspNetWebApiServiceCollectionExtensions
    {
        /// <summary>
        ///   Adds ASP.NET Web API services to the specified <see cref="IServiceCollection" />, including:
        ///   <list type="bullet">
        ///     <item><see cref="GlobalConfigurationStartupFilter"/> as <see cref="IStartupFilter"/></item>
        ///     <item><see cref="RequestServicesDependencyResolver"/> as <see cref="IDependencyResolver"/></item>
        ///   </list>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="IAspNetWebApiBuilder"/> that can be used to further configure the Web API services.</returns>
        public static IAspNetWebApiBuilder AddAspNetWebApi(this IServiceCollection services)
        {
            services.AddSingleton<IStartupFilter, GlobalConfigurationStartupFilter>();
            services.TryAddSingleton<IDependencyResolver, RequestServicesDependencyResolver>();

            return new AspNetWebApiBuilder(services);
        }
    }
}
