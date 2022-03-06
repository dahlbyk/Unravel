using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Unravel.Owin;

namespace Unravel.Hosting
{
    public class OwinServer : IServer
    {
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<OwinServer> logger;

        public OwinServer(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            logger = loggerFactory.CreateLogger<OwinServer>();

            // TODO: Set<IServerAddressesFeature>()?
        }

        public IFeatureCollection Features { get; } = new FeatureCollection();

        public virtual Task StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken)
        {
            logger.LogTrace(nameof(StartAsync));

            Features.Set<IOwinMiddlewareFeature>(new OwinMiddlewareFeature<TContext>(application, loggerFactory));

            return Task.CompletedTask;
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogTrace(nameof(StopAsync));

            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            logger.LogTrace(nameof(Dispose));

            GC.SuppressFinalize(this);
        }
    }
}
