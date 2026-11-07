using System;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Security;
using Xunit;

namespace Unravel.Startup.Tests
{
    public class ApplicationTests
    {
        [Theory]
        [InlineData(typeof(IOwinContext))]
        [InlineData(typeof(IAuthenticationManager))]
        [InlineData(typeof(OwinContextConsumer))]
        public void OwinServicesThrowOutsideRequest(Type serviceType)
        {
            using var host = new TestApplication().Build();
            using var scope = host.Services.CreateScope();

            Assert.Throws<InvalidOperationException>(() => scope.ServiceProvider.GetService(serviceType));
        }

        [Fact]
        public void HttpContextAccessorReturnsNullOutsideRequest()
        {
            using var host = new TestApplication().Build();
            using var scope = host.Services.CreateScope();

            var accessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

            Assert.Null(accessor.HttpContext);
        }

        private class TestApplication : Application
        {
            public IWebHost Build() => BuildWebHost(new AppBuilder());

            protected override IWebHostBuilder CreateWebHostBuilder() =>
                new WebHostBuilder().UseStartup(StartupType);

            public override void ConfigureServices(IServiceCollection services) =>
                services.AddScoped<OwinContextConsumer>();
        }

        private class OwinContextConsumer
        {
            public OwinContextConsumer(IOwinContext context) => Context = context;

            public IOwinContext Context { get; }
        }
    }
}
