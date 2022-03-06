using Microsoft.Owin.Builder;
using Owin;

namespace Unravel.Owin
{
    /// <summary>
    ///   Provides a middleware node to be added to the OWIN function pipeline by
    ///   <see cref="OwinAspNetCoreExtensions.UseAspNetCore(IAppBuilder, object[])"/>.
    ///   Middleware can receive additional <c>args</c> from <c>UseAspNetCore()</c>.
    /// </summary>
    public interface IOwinMiddlewareFeature
    {
        /// <summary>
        ///   The middleware passed to <see cref="IAppBuilder.Use(object, object[])"/>.
        ///   See <see cref="AppBuilder.Use(object, object[])"/> for documentation.
        /// </summary>
        object OwinMiddleware { get; }
    }
}
