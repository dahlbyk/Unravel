using System.Web.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Unravel.AspNet.Mvc.DependencyInjection.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///   Extension methods for setting up ASP.NET MVC services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class UnravelAspNetMvcServiceCollectionExtensions
    {
        /// <summary>
        ///   Adds ASP.NET MVC services to the specified <see cref="IServiceCollection" />, including:
        ///   <list type="bullet">
        ///     <item><see cref="DependencyResolverStartupFilter"/> as <see cref="IStartupFilter"/></item>
        ///     <item><see cref="ServiceProviderDependencyResolver"/> as <see cref="IDependencyResolver"/></item>
        ///   </list>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="IAspNetMvcBuilder"/> that can be used to further configure the MVC services.</returns>
        public static IAspNetMvcBuilder AddAspNetMvc(this IServiceCollection services)
        {
            services.AddSingleton<IStartupFilter, DependencyResolverStartupFilter>();
            services.TryAddSingleton<IDependencyResolver, ServiceProviderDependencyResolver>();

            return new AspNetMvcBuilder(services);
        }
    }
}
