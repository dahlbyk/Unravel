using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;

namespace Unravel.Hosting
{
    internal class InitialWebHost : IWebHost, IServiceProvider
    {
        public IFeatureCollection ServerFeatures =>
            throw new InvalidOperationException("Host not available.");

        public IServiceProvider Services => this;

        public void Start() =>
            throw new InvalidOperationException("Host not available.");

        public Task StartAsync(CancellationToken cancellationToken) =>
            Task.FromException(new InvalidOperationException("Host not available."));

        public Task StopAsync(CancellationToken cancellationToken) =>
            Task.CompletedTask;

        public void Dispose() { }

        object? IServiceProvider.GetService(Type serviceType) => null;
    }
}
