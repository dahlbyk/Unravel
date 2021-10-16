using System;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Unravel.Owin;

namespace Owin
{
    public static class OwinAspNetCoreExtensions
    {
        /// <summary>
        ///   Inserts ASP.NET Core processing into the OWIN pipeline using
        ///   <see cref="IOwinMiddlewareFeature.OwinMiddleware"/>.
        ///   The default middleware ignores <paramref name="args"/>.
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/>.</param>
        /// <param name="args">
        ///   See <see cref="AppBuilder.Use(object, object[])"/> for documentation.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///   <see cref="IOwinMiddlewareFeature"/> was not found in <see cref="IServer.Features"/>.
        /// </exception>
        public static void UseAspNetCore(this IAppBuilder app, params object[] args)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var server = app.GetApplicationServices().GetRequiredService<IServer>();

            var feature = server.Features.Get<IOwinMiddlewareFeature>()
                ?? throw new InvalidOperationException($"Server does not support {nameof(IOwinMiddlewareFeature)}.");

            app.Use(feature.OwinMiddleware, args);
        }
    }
}
