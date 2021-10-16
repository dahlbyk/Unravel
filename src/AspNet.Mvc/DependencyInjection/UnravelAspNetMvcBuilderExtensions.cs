using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///   Extensions for configuring ASP.NET MVC using an <see cref="IAspNetMvcBuilder"/>.
    /// </summary>
    public static class UnravelAspNetMvcBuilderExtensions
    {
        /// <summary>
        ///   Registers controllers discovered in <see cref="IHostingEnvironment.ApplicationName"/>
        ///   as services in the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IAspNetMvcBuilder"/>.</param>
        /// <returns>The <see cref="IAspNetMvcBuilder"/>.</returns>
        public static IAspNetMvcBuilder AddControllersAsServices(this IAspNetMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // https://github.com/dotnet/aspnetcore/blob/c2cfc5f140cd2743ecc33eeeb49c5a2dd35b017f/src/Mvc/Mvc.Core/src/DependencyInjection/MvcCoreServiceCollectionExtensions.cs#L83-L90
            var environment = GetServiceFromCollection<IHostingEnvironment>(builder.Services);
            var entryAssemblyName = environment?.ApplicationName;
            if (string.IsNullOrEmpty(entryAssemblyName))
                return builder;

            var entryAssembly = Assembly.Load(new AssemblyName(entryAssemblyName));
            return builder.AddControllersAsServices(entryAssembly);
        }

        /// <summary>
        ///   Registers controllers discovered in <paramref name="assembly"/>
        ///   as services in the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IAspNetMvcBuilder"/>.</param>
        /// <param name="assembly">The <see cref="Assembly"/> to scan.</param>
        /// <returns>The <see cref="IAspNetMvcBuilder"/>.</returns>
        public static IAspNetMvcBuilder AddControllersAsServices(this IAspNetMvcBuilder builder, Assembly assembly)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var services = builder.Services;

            var controllerType = typeof(IController);

            foreach (var type in assembly.GetExportedTypes())
            {
                if (type.IsAbstract || type.IsGenericTypeDefinition)
                    continue;

                if (!controllerType.IsAssignableFrom(type))
                    continue;

                if (!type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                    continue;

                services.AddTransient(type);
            }

            return builder;
        }

        private static T GetServiceFromCollection<T>(IServiceCollection services)
        {
            return (T)services
                .LastOrDefault(d => d.ServiceType == typeof(T))
                ?.ImplementationInstance;
        }
    }
}
