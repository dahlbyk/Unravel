using System;
using Spring.Context;
using Spring.Web.Mvc;
using System.Web.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Spring.Context.Support;
using MvcDependencyResolver = Unravel.AspNet.Mvc.DependencyInjection.Internal.RequestServicesDependencyResolver;
using WebApiDependencyResolver = Unravel.AspNet.WebApi.DependencyInjection.Internal.RequestServicesDependencyResolver;

namespace Unravel.Spring.Web.Mvc
{
    public class SpringMvcApplication : Application
    {
        /// <summary>
        /// Calls <see cref="Application.CreateWebHostBuilder()"/>,
        /// then calls <see cref="IWebHostBuilder.ConfigureServices(Action{IServiceCollection})"/>
        /// with <see cref="ConfigureSpringService()"/>.
        /// </summary>
        /// <returns></returns>
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return base.CreateWebHostBuilder()
                .ConfigureServices(ConfigureSpringServices);
        }

        /// <summary>
        /// Calls <see cref="ConfigureApplicationContext()"/>,
        /// then adds Spring services to the specified <see cref="IServiceCollection" />, including:
        /// <list type="bullet">
        ///   <item>The root <see cref="IApplicationContext"/> from <see cref="ContextRegistry.GetContext()"/></item>
        ///   <item>An MVC <c>IDependencyResolver</c> with the result of <see cref="BuildDependencyResolver()"/> as fallback.</item>
        ///   <item>A WebApi <c>IDependencyResolver</c> with the result of <see cref="BuildDependencyResolver()"/> as fallback.</item>
        /// </list>
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to configure.</param>
        protected virtual void ConfigureSpringServices(IServiceCollection services)
        {
            ConfigureApplicationContext();
            services.AddSingleton(ContextRegistry.GetContext());

            services.AddSingleton<IDependencyResolver>(
                new MvcDependencyResolver { Fallback = BuildDependencyResolver() });

            services.AddSingleton<System.Web.Http.Dependencies.IDependencyResolver>(
                new WebApiDependencyResolver { Fallback = BuildWebApiDependencyResolver() });
        }

        /// <summary>
        /// Builds the dependency resolver.
        /// </summary>
        /// <returns>The <see cref="IDependencyResolver"/> instance.</returns>
        /// <remarks>
        /// You must override this method in a derived class to control the manner in which the
        /// <see cref="IDependencyResolver"/> is created.
        /// </remarks>
        protected virtual IDependencyResolver BuildDependencyResolver()
        {
            return new SpringMvcDependencyResolver(ContextRegistry.GetContext());
        }

        /// <summary>
        /// Builds the dependency resolver.
        /// </summary>
        /// <returns>The <see cref="System.Web.Http.Dependencies.IDependencyResolver"/> instance.</returns>
        /// <remarks>
        /// You must override this method in a derived class to control the manner in which the
        /// <see cref="System.Web.Http.Dependencies.IDependencyResolver"/> is created.
        /// </remarks>
        protected virtual System.Web.Http.Dependencies.IDependencyResolver BuildWebApiDependencyResolver()
        {
            return new SpringWebApiDependencyResolver(ContextRegistry.GetContext());
        }

        /// <summary>
        /// Configures the <see cref="IApplicationContext"/> instance.
        /// </summary>
        /// <remarks>
        /// You must override this method in a derived class to control the manner in which the
        /// <see cref="IApplicationContext"/> is configured.
        /// </remarks>
        protected virtual void ConfigureApplicationContext()
        {
        }

        /// <summary>
        /// Unused by Unravel.
        /// Formerly registered the DependencyResolver implementation with the MVC runtime.
        /// </summary>
        /// <remarks>
        /// Override <c>ConfigureServices()</c> to control the manner in which the
        /// <see cref="IDependencyResolver"/> is registered.
        /// The MVC runtime is automatically configured from <c>Services</c>.
        /// </remarks>
        [Obsolete("Override ConfigureServices() to customize the dependency resolver.")]
        public virtual void RegisterDependencyResolver(IDependencyResolver resolver)
        {
            ThreadSafeDependencyResolverRegistrar.Register(resolver);
        }

        /// <summary>
        /// Unused by Unravel.
        /// Formerly registered the DependencyResolver implementation with the WebApi runtime.
        /// </summary>
        /// <remarks>
        /// Override <c>ConfigureServices()</c> to control the manner in which the
        /// <see cref="System.Web.Http.Dependencies.IDependencyResolver"/> is registered.
        /// The WebApi runtime is automatically configured from <c>Services</c>.
        /// </remarks>
        [Obsolete("Override ConfigureServices() to customize the dependency resolver.")]
        public virtual void RegisterDependencyResolver(System.Web.Http.Dependencies.IDependencyResolver resolver)
        {
            ThreadSafeDependencyResolverRegistrar.Register(resolver);
        }

        [Obsolete("Dependency resolvers are automatically registered from Services.")]
        protected static class ThreadSafeDependencyResolverRegistrar
        {
            /// <summary>
            /// Registers the specified <see cref="System.Web.Mvc.IDependencyResolver"/>.
            /// </summary>
            /// <param name="resolver">The resolver.</param>
            /// <exception cref="NotSupportedException">MVC IDependencyResolver is automatically registered from Services.</exception>
            [Obsolete("Dependency resolver is automatically registered from Services.")]
            public static void Register(IDependencyResolver resolver)
            {
                throw new NotSupportedException("MVC IDependencyResolver is automatically registered from Services.");
            }

            /// <summary>
            /// Registers the specified <see cref="System.Web.Http.Dependencies.IDependencyResolver"/>.
            /// </summary>
            /// <param name="resolver">The resolver.</param>
            /// <exception cref="NotSupportedException">WebApi IDependencyResolver is automatically registered from Services.</exception>
            [Obsolete("Dependency resolver is automatically registered from Services.")]
            public static void Register(System.Web.Http.Dependencies.IDependencyResolver resolver)
            {
                throw new NotSupportedException("WebApi IDependencyResolver is automatically registered from Services.");
            }
        }
    }
}
