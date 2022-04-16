using System;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    ///   Service-related extensions for <see cref="HttpContext"/>,
    ///   aligned with <see cref="System.Web.HttpContextServicesExtensions"/> to ease migration.
    /// </summary>
    public static class UnravelHttpContextServicesExtensions
    {
        /// <summary>
        ///   Gets the <see cref="IServiceProvider"/> that provides access to <paramref name="context"/>'s service container.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <returns>The <see cref="IServiceProvider"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <c>null</c>.</exception>
        [Obsolete("Use " + nameof(HttpContext.RequestServices))]
        public static IServiceProvider GetRequestServices(this HttpContext context) =>
            (context ?? throw new ArgumentNullException(nameof(context)))
                .RequestServices;

        /// <inheritdoc cref="GetRequestServices(HttpContext)"/>
        [Obsolete("Use " + nameof(HttpContext.RequestServices))]
        public static IServiceProvider GetServiceProvider(this HttpContext context) =>
            context.GetRequestServices();
    }
}
