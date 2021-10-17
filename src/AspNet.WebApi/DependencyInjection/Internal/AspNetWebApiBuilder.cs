using Microsoft.Extensions.DependencyInjection;

namespace Unravel.AspNet.WebApi.DependencyInjection.Internal
{
    /// <summary>
    ///   Allows fine grained configuration of ASP.NET Web API services.
    /// </summary>
    public class AspNetWebApiBuilder : IAspNetWebApiBuilder
    {
        /// <summary>
        ///   Initializes a new <see cref="AspNetWebApiBuilder"/> instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        public AspNetWebApiBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <inheritdoc/>
        public IServiceCollection Services { get; }
    }
}
