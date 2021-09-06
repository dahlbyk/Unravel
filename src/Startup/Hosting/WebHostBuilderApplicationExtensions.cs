using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Unravel.Hosting
{
    public static class WebHostBuilderApplicationExtensions
    {
        /// <summary>
        ///   Specify the startup instance to be used by the web host.
        /// </summary>
        /// <remarks>
        ///   This differs from <see cref="WebHostBuilderExtensions.UseStartup(IWebHostBuilder, Type)"/>
        ///   in setting <see cref="WebHostDefaults.ApplicationKey"/> (and thus <see cref="IHostingEnvironment.ApplicationName"/>)
        ///   to the concrete type's assembly name, instead of the abstract parent type's.
        /// </remarks>
        /// <param name="builder">The <see cref="IWebHostBuilder"/> to configure.</param>
        /// <param name="startup">The instance to be registered as a singleton <see cref="IStartup"/>.</param>
        /// <typeparam name ="TStartup">The type containing the startup methods for the application.</typeparam>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        public static IWebHostBuilder UseStartup<TStartup>(this IWebHostBuilder builder, TStartup startup)
            where TStartup : IStartup
        {
            if (startup == null) throw new ArgumentNullException(nameof(startup));

            var startupAssemblyName = startup.GetType().GetTypeInfo().Assembly.GetName().Name;

            return builder
                .UseSetting(WebHostDefaults.ApplicationKey, startupAssemblyName)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IStartup>(startup);
                });
        }
    }
}
