using System.Collections;
using Microsoft.Extensions.DependencyInjection;
using Unravel;

namespace System.Web
{
    /// <summary>
    ///   Service-related extensions for <see cref="HttpContext"/> and <see cref="HttpContextBase"/>.
    /// </summary>
    public static class HttpContextServicesExtensions
    {
        /// <summary>
        ///   Key in <see cref="HttpContext.Items"/> for current <see cref="IServiceScope"/>.
        /// </summary>
        public static readonly Type ServiceScopeKey = typeof(IServiceScope);

        /// <summary>
        ///   Adds a new <see cref="IServiceScope"/> to <paramref name="context"/>'s
        ///   <see cref="HttpContext.Items"/> at key <see cref="ServiceScopeKey"/>.
        /// </summary>
        /// <remarks>
        ///   This is normally handled by <see cref="Unravel.SystemWeb.ServiceScopeModule"/>.
        /// </remarks>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <returns>The new <see cref="IServiceScope"/>, which you must <see cref="IDisposable.Dispose()"/>.</returns>
        public static IServiceScope CreateServiceScope(this HttpContext context)
        {
            var scope = Application.Services.CreateScope();

            context.Items[ServiceScopeKey] = scope;

            return scope;
        }

        /// <summary>
        ///   Gets the scoped <see cref="IServiceProvider"/> for <paramref name="context"/>.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <returns>The <see cref="IServiceProvider"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">
        ///   An <see cref="IServiceScope"/> was not found.
        ///   See <see cref="CreateServiceScope(HttpContext)"/>.
        /// </exception>
        public static IServiceProvider GetRequestServices(this HttpContext? context) =>
            context.GetServiceScope().ServiceProvider;

        /// <inheritdoc cref="GetRequestServices(HttpContext)"/>
        [Obsolete("Use " + nameof(GetRequestServices))]
        public static IServiceProvider GetServiceProvider(this HttpContext? context) =>
            context.GetRequestServices();

        /// <summary>
        ///   Gets the scoped <see cref="IServiceProvider"/> for <paramref name="context"/>.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <returns>The <see cref="IServiceProvider"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">
        ///   An <see cref="IServiceScope"/> was not found.
        ///   See <see cref="CreateServiceScope(HttpContext)"/>.
        /// </exception>
        public static IServiceProvider GetRequestServices(this HttpContextBase? context) =>
            context.GetServiceScope().ServiceProvider;

        /// <inheritdoc cref="GetRequestServices(HttpContextBase)"/>
        [Obsolete("Use " + nameof(GetRequestServices))]
        public static IServiceProvider GetServiceProvider(this HttpContextBase? context) =>
            context.GetRequestServices();

        private static IServiceScope GetServiceScope(this HttpContext? context) =>
            (context ?? throw new ArgumentNullException(nameof(context))).Items.GetServiceScope();

        private static IServiceScope GetServiceScope(this HttpContextBase? context) =>
            (context ?? throw new ArgumentNullException(nameof(context))).Items.GetServiceScope();

        private static IServiceScope GetServiceScope(this IDictionary items) =>
            (IServiceScope)items[ServiceScopeKey] ?? throw new InvalidOperationException("IServiceScope not found.");
    }
}
