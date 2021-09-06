using System;
using System.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Owin;
using Unravel.Hosting;
using Unravel.SystemWeb;

namespace Unravel
{
    /// <summary>
    ///   A base <see cref="HttpApplication"/> which initializes <see cref="WebHost"/>,
    ///   and implements ASP.NET Core <see cref="IStartup"/>.
    /// </summary>
    public abstract class Application : HttpApplication, IStartup
    {
        /// <summary>
        ///   The current <see cref="WebHost"/>'s <see cref="IServiceProvider"/>.
        /// </summary>
        public static IServiceProvider Services => WebHost.Services;

        /// <summary>
        ///   The current <see cref="IWebHost"/>.
        ///   Cannot be used until OWIN has been initialized.
        /// </summary>
        public static IWebHost WebHost { get; private set; } = new InvalidHost();

        /// <summary>
        ///   Builds and starts <see cref="WebHost"/>, then calls <see cref="Configure(IAppBuilder)"/>.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            var host = WebHost = BuildWebHost(app);
            host.Start();

            Configure(app);
        }

        /// <summary>
        ///   Builds <see cref="WebHost"/> and registers current <see cref="IAppBuilder"/> as a singleton service.
        /// </summary>
        /// <param name="app">The host app.</param>
        /// <returns>The <see cref="IWebHost"/>.</returns>
        protected IWebHost BuildWebHost(IAppBuilder app)
        {
            return CreateWebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(app);
                })
                .Build();
        }

        /// <summary>
        ///   Configures OWIN pipeline after <see cref="WebHost"/> has been started, with <see cref="Services"/> available.
        /// </summary>
        /// <param name="app">The host app.</param>
        public virtual void Configure(IAppBuilder app) { }

        /// <summary>
        ///   Configures ASP.NET Core pipeline.
        /// </summary>
        /// <param name="app">The host app.</param>
        public virtual void Configure(IApplicationBuilder app) { }

        IServiceProvider IStartup.ConfigureServices(IServiceCollection services)
        {
            ConfigureServices(services);
            return CreateServiceProvider(services);
        }

        /// <summary>
        ///   Configures services for the current <see cref="WebHost"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        public virtual void ConfigureServices(IServiceCollection services) { }

        /// <summary>
        ///   Builds a <see cref="IServiceProvider"/> from the configured <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        public virtual IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            return services.BuildServiceProvider();
        }

        /// <summary>
        ///   Creates <see cref="IWebHostBuilder"/> used to initialize <see cref="WebHost"/>.
        /// </summary>
        /// <remarks>
        ///   The default implementation uses <see cref="OwinHost.CreateDefaultBuilder()"/>, then
        ///     calls <see cref="WebHostBuilderSystemWebExtensions.UseHostingEnvironment(IWebHostBuilder)"/>, and
        ///     calls <see cref="WebHostBuilderApplicationExtensions.UseStartup{TStartup}(IWebHostBuilder, TStartup)"/> with <c>this</c>.
        /// </remarks>
        /// <returns>The initialized <see cref="IWebHostBuilder"/>.</returns>
        protected virtual IWebHostBuilder CreateWebHostBuilder()
        {
            return OwinHost.CreateDefaultBuilder()
                .UseHostingEnvironment()
                .UseStartup(this);
        }
    }
}
