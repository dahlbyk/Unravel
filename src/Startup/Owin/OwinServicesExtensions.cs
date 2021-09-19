using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Owin;

namespace Owin
{
    public static class OwinServicesExtensions
    {
        /// <summary>
        ///   Key in <see cref="IAppBuilder.Properties"/> for current <see cref="IServiceProvider"/>.
        /// </summary>
        public static readonly string ServiceProviderKey = typeof(IServiceProvider).FullName;

        /// <summary>
        ///   Gets the scoped <see cref="IServiceProvider"/> for <paramref name="context"/>.
        /// </summary>
        /// <param name="context">The <see cref="IOwinContext"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="context"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">
        ///   An <see cref="IServiceProvider"/> was not found.
        ///   See <see cref="HttpContextServicesExtensions.CreateServiceScope(HttpContext)"/>.
        /// </exception>
        public static IServiceProvider GetRequestServices(this IOwinContext context) =>
            context.GetHttpContext()
                .GetRequestServices();

        /// <inheritdoc cref="GetRequestServices(IOwinContext)"/>
        [Obsolete("Use " + nameof(GetRequestServices))]
        public static IServiceProvider GetServiceProvider(this IOwinContext context) =>
            context.GetRequestServices();

        internal static HttpContextBase GetHttpContext(this IOwinContext context) =>
            (context ?? throw new ArgumentNullException(nameof(context)))
                .Environment.GetHttpContext();

        internal static HttpContextBase GetHttpContext(this IDictionary<string, object> env) =>
            (env ?? throw new ArgumentNullException(nameof(env)))
                .TryGetValue(typeof(HttpContextBase).FullName, out var value) ? value as HttpContextBase : null;

        /// <summary>
        ///   Gets the non-scoped <see cref="IServiceProvider"/> for <paramref name="app"/>.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/>.</param>
        /// <returns>The <see cref="IServiceProvider"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="app"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">
        ///   An <see cref="IServiceProvider"/> was not found.
        ///   See <see cref="SetServiceProvider(IAppBuilder, IServiceProvider)"/>.
        /// </exception>
        public static IServiceProvider GetApplicationServices(this IAppBuilder app) =>
            (app ?? throw new ArgumentNullException(nameof(app)))
                .Properties.GetServiceProvider();

        /// <inheritdoc cref="GetApplicationServices(IAppBuilder)"/>
        [Obsolete("Use " + nameof(GetApplicationServices))]
        public static IServiceProvider GetServiceProvider(this IAppBuilder app) =>
            app.GetApplicationServices();

        private static IServiceProvider GetServiceProvider(this IDictionary<string, object> env) =>
            (IServiceProvider)env[ServiceProviderKey] ?? throw new InvalidOperationException("IServiceProvider not found.");

        /// <summary>
        ///   Adds <paramref name="services"/> to <paramref name="app"/>'s
        ///   <see cref="IAppBuilder.Properties"/> at key <see cref="ServiceProviderKey"/>.
        /// </summary>
        /// <remarks>
        ///   This is normally handled by <see cref="Unravel.Application.Configuration(IAppBuilder)"/>.
        /// </remarks>
        /// <param name="app">The <see cref="IAppBuilder"/>.</param>
        /// <param name="services">The <see cref="IServiceProvider"/>.</param>
        public static void SetServiceProvider(this IAppBuilder app, IServiceProvider services) =>
            (app ?? throw new ArgumentNullException(nameof(app)))
                .Properties[ServiceProviderKey] = services;
    }
}
