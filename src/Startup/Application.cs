using System;
using System.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Owin;
using Unravel.Hosting;
using Unravel.Startup.Owin;
using Unravel.SystemWeb;

namespace Unravel
{
    /// <summary>
    ///   A base <see cref="HttpApplication"/> which initializes <see cref="WebHost"/>,
    ///   and can serve as ASP.NET Core <c>Startup</c>.
    /// </summary>
    public abstract class Application : HttpApplication
    {
        /// <summary>
        ///   Default constructor is required by ASP.NET.
        ///   If your startup requires constructor dependency injection, see <see cref="StartupType"/>.
        /// </summary>
        protected Application()
        { }

        /// <summary>
        ///   The current <see cref="WebHost"/>'s <see cref="IServiceProvider"/>.
        /// </summary>
        public static IServiceProvider Services => WebHost.Services;

        /// <inheritdoc cref="Services" />
        [Obsolete("Use " + nameof(Services))]
        public static IServiceProvider ServiceProvider => Services;

        /// <summary>
        ///   The current <see cref="IWebHost"/>.
        ///   Cannot be used until OWIN has been initialized.
        /// </summary>
        public static IWebHost WebHost { get; private set; } = new InitialWebHost();

        /// <summary>
        ///   Builds and starts <see cref="WebHost"/>, then calls <see cref="ConfigureOwin(IAppBuilder)"/>.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            var host = WebHost = BuildWebHost(app);
            host.Start();

            app.SetServiceProvider(host.Services);

            ConfigureOwin(app);
        }

        /// <summary>
        ///   Builds <see cref="WebHost"/> and registers a few services:
        ///   <list type="bullet">
        ///     <item><see cref="IAppBuilder"/> (singleton)</item>
        ///     <item><see cref="IHttpContextAccessor"/> (singleton)</item>
        ///     <item><see cref="IOwinContext"/> (scoped)</item>
        ///     <item><see cref="IAuthenticationManager"/> (scoped)</item>
        ///     <item>
        ///       <see cref="Microsoft.AspNetCore.Http.IHttpContextAccessor"/> (singleton);
        ///       see <see cref="AspNetCore.Http.SystemWebHttpContextAccessor"/> for details
        ///     </item>
        ///   </list>
        /// </summary>
        /// <param name="app">The host app.</param>
        /// <returns>The <see cref="IWebHost"/>.</returns>
        protected IWebHost BuildWebHost(IAppBuilder app)
        {
            return CreateWebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton(app);
                    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

                    services.AddScoped(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext?.GetOwinContext());
                    services.AddScoped(sp => sp.GetService<IOwinContext>()?.Authentication);

                    services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, AspNetCore.Http.SystemWebHttpContextAccessor>();
                })
                .Build();
        }

        /// <summary>
        ///   Configures OWIN pipeline after <see cref="WebHost"/> has been started, with <see cref="Services"/> available.
        /// </summary>
        /// <remarks>
        ///   The default implementation calls <see cref="OwinLoader.Configure(object, IAppBuilder)"/>,
        ///   but only if <see cref="StartupType"/> isn't the current instance type.
        ///   Most apps will just <c>override</c> this if they need an OWIN pipeline.
        /// </remarks>
        /// <param name="app">The host app.</param>
        public virtual void ConfigureOwin(IAppBuilder app)
        {
            if (StartupType != GetType())
            {
                var startup = Services.GetService<IStartup>();
                OwinLoader.Configure(startup, app);
            }
        }

        /// <summary>
        ///   Configures ASP.NET Core pipeline.
        /// </summary>
        /// <param name="app">The host app.</param>
        public virtual void Configure(IApplicationBuilder app)
        { }

        /// <summary>
        ///   Configures services for the current <see cref="WebHost"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        public virtual void ConfigureServices(IServiceCollection services)
        { }

        /// <summary>
        ///   Creates <see cref="IWebHostBuilder"/> used to initialize <see cref="WebHost"/>.
        /// </summary>
        /// <remarks>
        ///   The default implementation uses <see cref="OwinHost.CreateDefaultBuilder()"/>, then
        ///     calls <see cref="WebHostBuilderSystemWebExtensions.UseHostingEnvironment(IWebHostBuilder)"/>, and
        ///     calls <see cref="WebHostBuilderExtensions.UseStartup(IWebHostBuilder, Type)"/> with <see cref="StartupType"/>.
        /// </remarks>
        /// <returns>The initialized <see cref="IWebHostBuilder"/>.</returns>
        protected virtual IWebHostBuilder CreateWebHostBuilder()
        {
            return OwinHost.CreateDefaultBuilder()
                .UseHostingEnvironment()
                .UseStartup(StartupType);
        }

        /// <summary>
        ///   The type passed to <see cref="WebHostBuilderExtensions.UseStartup(IWebHostBuilder, Type)"/> in <see cref="CreateWebHostBuilder()"/>.
        /// </summary>
        /// <remarks>
        ///   The default implementation returns the <see cref="Type"/> of the current instance.
        ///   <para>
        ///     Override to use a custom startup type, e.g. to support dependency injection.
        ///     You will need to add <c>[asssembly: OwinStartup(YourHttpApplication)]</c>, so OWIN initializes through <c>Unravel.Application</c>.
        ///   </para>
        /// </remarks>
        protected virtual Type StartupType => GetType();
    }
}
