using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Owin;
using Microsoft.Extensions.Logging;
using Owin;

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
        private readonly ILogger logger;

        public OwinMiddlewareFeature(IHttpApplication<TContext> application, ILoggerFactory loggerFactory)
        {
            this.application = application;
            logger = loggerFactory.CreateLogger<OwinMiddlewareFeature<TContext>>();
        }

        object IOwinMiddlewareFeature.OwinMiddleware => new Func<AppFunc, AppFunc>(Invoke);

        public AppFunc Invoke(AppFunc next)
        {
            // https://github.com/dotnet/AspNetCore.Docs/blob/55e9843af1055d38d33450497850bd4295d0f5fe/aspnetcore/fundamentals/owin/sample/src/NowinSample/NowinServer.cs#L32-L50
            return UnravelAppFunc;

            async Task UnravelAppFunc(IDictionary<string, object> env)
            {
                // The reason for 2 level of wrapping is because the OwinFeatureCollection isn't mutable
                // so features can't be added
                var owinFeatures = new OwinFeatureCollection(env);
                var features = new FeatureCollection(owinFeatures);

                var response = new OwinResponseFeature(features.Get<IHttpResponseFeature>(), logger);
                features.Set<IHttpResponseFeature>(response);

                var initialStatus = response.StatusCode; // Incoming OWIN status (typically 200)

                var context = application.CreateContext(features);

                var requestException = default(Exception);

                var httpContext = env.GetHttpContext() ?? throw new InvalidOperationException("HttpContextBase not found in OWIN environment.");
                httpContext.AddOnRequestCompleted(OnRequestCompleted);

                void OnRequestCompleted(System.Web.HttpContextBase ctx)
                {
                    // Need to block on the callback since we can't change the HttpContextBase signature to be async
                    response.FireOnCompletedAsync().GetAwaiter().GetResult();

                    application.DisposeContext(context, requestException);
                }

                // From: https://github.com/dotnet/aspnetcore/blob/c2cfc5f140cd2743ecc33eeeb49c5a2dd35b017f/src/Hosting/TestHost/src/HttpContextBuilder.cs#L67-L77
                // TODO: https://github.com/dotnet/aspnetcore/blob/ccfb12cf73b0285c981c70a2061312a837510f7b/src/Servers/Kestrel/Core/src/Internal/Http/HttpProtocol.cs#L662-L756
                try
                {
                    await application.ProcessRequestAsync(context);
                }
                catch (Exception ex)
                {
                    requestException = ex;
                    throw;
                }

                // The base RequestDelegate only sets StatusCode = 404:
                // https://github.com/dotnet/aspnetcore/blob/c2cfc5f140cd2743ecc33eeeb49c5a2dd35b017f/src/Http/Http/src/Internal/ApplicationBuilder.cs#L82-L86
                // If we still have 404 and Body hasn't been accessed, let the OWIN pipeline continue
                if (response.StatusCode == 404 && !response.AssumeBodyModified)
                {
                    // Reset to prior OWIN status
                    response.StatusCode = initialStatus;
                    await next(env);
                }
            }
        }
    }
}
