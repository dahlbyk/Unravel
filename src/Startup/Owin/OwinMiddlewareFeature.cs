using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Owin;

namespace Unravel.Owin
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    /// <summary>
    ///   Provides an OWIN middleware that processes requests with the given
    ///   <see cref="IHttpApplication{TContext}"/>.
    ///   Context is created with <see cref="OwinFeatureCollection"/>.
    /// </summary>
    /// <typeparam name="TContext">The context associated with the application.</typeparam>
    public class OwinMiddlewareFeature<TContext> : IOwinMiddlewareFeature
    {
        private readonly IHttpApplication<TContext> application;

        public OwinMiddlewareFeature(IHttpApplication<TContext> application)
        {
            this.application = application;
        }

        object IOwinMiddlewareFeature.OwinMiddleware => new Func<AppFunc, AppFunc>(Invoke);

        public AppFunc Invoke(AppFunc next)
        {
            // https://github.com/dotnet/AspNetCore.Docs/blob/55e9843af1055d38d33450497850bd4295d0f5fe/aspnetcore/fundamentals/owin/sample/src/NowinSample/NowinServer.cs#L32-L50
            return async env =>
            {
                // The reason for 2 level of wrapping is because the OwinFeatureCollection isn't mutable
                // so features can't be added
                var owinFeatures = new OwinFeatureCollection(env);
                var features = new FeatureCollection(owinFeatures);

                var response = features.Get<IHttpResponseFeature>();

                var initialStatus = response.StatusCode; // Incoming OWIN status (typically 200)

                var context = application.CreateContext(features);

                // From: https://github.com/dotnet/aspnetcore/blob/c2cfc5f140cd2743ecc33eeeb49c5a2dd35b017f/src/Hosting/TestHost/src/HttpContextBuilder.cs#L67-L77
                // TODO: https://github.com/dotnet/aspnetcore/blob/ccfb12cf73b0285c981c70a2061312a837510f7b/src/Servers/Kestrel/Core/src/Internal/Http/HttpProtocol.cs#L662-L756
                try
                {
                    await application.ProcessRequestAsync(context);

                    application.DisposeContext(context, null);
                }
                catch (Exception ex)
                {
                    application.DisposeContext(context, ex);
                    throw;
                }

                // TODO: Need a more robust check
                if (response.StatusCode == 404)
                {
                    // Reset to prior OWIN status
                    response.StatusCode = initialStatus;
                    await next(env);
                }
            };
        }
    }
}
