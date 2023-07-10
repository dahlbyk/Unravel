using System;
using System.Threading.Tasks;
using Unravel.Hosting;
using Xunit;

namespace Unravel.Startup.Hosting.Tests
{
    public class InitialWebHostTests
    {
        [Fact]
        public void ServerFeaturesThrows()
        {
            using var host = new InitialWebHost();

            Assert.Throws<InvalidOperationException>(() => host.ServerFeatures);
        }

        [Fact]
        public void ServicesThrows()
        {
            using var host = new InitialWebHost();

            Assert.Throws<InvalidOperationException>(() => host.Services);
        }

        [Fact]
        public async Task StartThrows()
        {
            using var host = new InitialWebHost();

            Assert.Throws<InvalidOperationException>(() => host.Start());

            // Shouldn't throw yet
            var startTask = host.StartAsync(default);
            await Assert.ThrowsAsync<InvalidOperationException>(() => startTask);
        }

        [Fact]
        public async Task StopDoesNotThrow()
        {
            using var host = new InitialWebHost();

            await host.StopAsync(default);
        }
    }
}
