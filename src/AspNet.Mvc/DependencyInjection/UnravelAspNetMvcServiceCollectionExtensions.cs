using System;
using System.Web.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Unravel.AspNet.Mvc;
using Unravel.AspNet.Mvc.DependencyInjection.Internal;
using Unravel.AspNet.Mvc.Internal;
using Unravel.AspNet.Mvc.ModelBinding;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///   Extension methods for setting up ASP.NET MVC services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class UnravelAspNetMvcServiceCollectionExtensions
    {
        /// <summary>
        ///   Adds ASP.NET MVC services to the specified <see cref="IServiceCollection" />, including:
        ///   <list type="bullet">
        ///     <item><see cref="DependencyResolverStartupFilter"/> as <see cref="IStartupFilter"/></item>
        ///     <item><see cref="RequestServicesDependencyResolver"/> as <see cref="IDependencyResolver"/></item>
        ///   </list>
        ///   Also adds useful MVC configuration:
        ///   <list type="bullet">
        ///     <item><see cref="IFormFile"/> model binding with <see cref="HttpPostedFormFileModelBinder"/></item>
        ///   </list>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="IAspNetMvcBuilder"/> that can be used to further configure the MVC services.</returns>
        public static IAspNetMvcBuilder AddAspNetMvc(this IServiceCollection services)
        {
            services.AddSingleton<IStartupFilter, DependencyResolverStartupFilter>();
            services.TryAddSingleton<IDependencyResolver, RequestServicesDependencyResolver>();

            services.AddSingleton<IStartupFilter, MvcOptionsStartupFilter>();
            services.Configure<MvcOptions>(ConfigureOptions);

            return new AspNetMvcBuilder(services);

            void ConfigureOptions(MvcOptions options)
            {
                options.ModelBinders.Add(typeof(IFormFile), new HttpPostedFormFileModelBinder());
            }
        }

        /// <inheritdoc cref="AddAspNetMvc(IServiceCollection)"/>
        /// <param name="setupAction">An <see cref="Action{MvcOptions}"/> to configure the provided <see cref="MvcOptions"/>.</param>
        public static IAspNetMvcBuilder AddAspNetMvc(this IServiceCollection services, Action<MvcOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            var builder = services.AddAspNetMvc();
            builder.Services.Configure(setupAction);

            return builder;
        }
    }
}
