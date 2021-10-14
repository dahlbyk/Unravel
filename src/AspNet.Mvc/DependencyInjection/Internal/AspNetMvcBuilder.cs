using Microsoft.Extensions.DependencyInjection;

namespace Unravel.AspNet.Mvc.DependencyInjection.Internal
{
    /// <summary>
    ///   Allows fine grained configuration of ASP.NET MVC services.
    /// </summary>
    public class AspNetMvcBuilder : IAspNetMvcBuilder
    {
        /// <summary>
        ///   Initializes a new <see cref="AspNetMvcBuilder"/> instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        public AspNetMvcBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <inheritdoc/>
        public IServiceCollection Services { get; }
    }
}
