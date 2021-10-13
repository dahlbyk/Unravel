using System;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Owin;

namespace Unravel.Startup.Owin
{
    public class OwinLoader
    {
        /// <summary>
        ///   Calls <c>Configuration(IAppBuilder)</c> with <paramref name="app"/>, if defined on <paramref name="startup"/>.
        /// </summary>
        /// <remarks>
        ///   <see href="https://docs.microsoft.com/en-us/aspnet/aspnet/overview/owin-and-katana/owin-startup-class-detection"/>.
        /// </remarks>
        /// <param name="startup">The startup instance.</param>
        /// <param name="app">The <see cref="IAppBuilder"/>.</param>
        public static void Configure(object startup, IAppBuilder app)
        {
            if (startup is ConventionBasedStartup cbs)
            {
                var _methods = typeof(ConventionBasedStartup).GetTypeInfo().GetField("_methods", BindingFlags.NonPublic | BindingFlags.Instance);
                var startupMethods = _methods?.GetValue(cbs) as StartupMethods
                    ?? throw new InvalidOperationException("ConventionBasedStartup changed. Please open an issue and include ASP.NET Core version.");

                startup = startupMethods?.StartupInstance;
            }

            if (startup is null) return;

            var configuration = startup.GetType().GetTypeInfo().GetMethod("Configuration", new[] { typeof(IAppBuilder) });
            configuration?.Invoke(startup, new object[] { app });
        }
    }
}
