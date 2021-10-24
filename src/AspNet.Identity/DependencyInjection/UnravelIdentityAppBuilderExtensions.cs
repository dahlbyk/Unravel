using System;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Unravel.AspNet.Identity.DependencyInjection;

namespace Owin
{
    public static class UnravelIdentityAppBuilderExtensions
    {
        /// <summary>
        /// "The call is ambiguous between the following methods" conflict with
        /// <see cref="AppBuilderExtensions.CreatePerOwinContext{T}(IAppBuilder, Func{T})"/>.
        /// Configure Identity services with <see cref="IdentityBuilder"/> instead.
        /// </summary>
        /// <exception cref="NotSupportedException">Do not use this method.</exception>
        [Obsolete("Use IdentityBuilder.SetPerOwinContext<T>().")]
        public static IAppBuilder CreatePerOwinContext<T>(this IAppBuilder app, Func<T> createCallback)
            where T : class, IDisposable =>
            throw new NotSupportedException("Configure Identity services with IdentityBuilder instead.");

        /// <summary>
        /// "The call is ambiguous between the following methods" conflict with
        /// <see cref="AppBuilderExtensions.CreatePerOwinContext{T}(IAppBuilder, Func{IdentityFactoryOptions{T}, IOwinContext, T})"/>.
        /// Configure Identity services with <see cref="IdentityBuilder"/> instead.
        /// </summary>
        /// <exception cref="NotSupportedException">Do not use this method.</exception>
        [Obsolete("Use IdentityBuilder.SetPerOwinContext<T>().")]
        public static IAppBuilder CreatePerOwinContext<T>(this IAppBuilder app, Func<IdentityFactoryOptions<T>, IOwinContext, T> createCallback)
            where T : class, IDisposable =>
            throw new NotSupportedException("Configure Identity services with IdentityBuilder instead.");

        /// <summary>
        /// "The call is ambiguous between the following methods" conflict with
        /// <see cref="AppBuilderExtensions.CreatePerOwinContext{T}(IAppBuilder, Func{IdentityFactoryOptions{T}, IOwinContext, T}, Action{IdentityFactoryOptions{T}, T})"/>.
        /// Configure Identity services with <see cref="IdentityBuilder"/> instead.
        /// </summary>
        /// <exception cref="NotSupportedException">Do not use this method.</exception>
        [Obsolete("Use IdentityBuilder.SetPerOwinContext<T>().")]
        public static IAppBuilder CreatePerOwinContext<T>(this IAppBuilder app, Func<IdentityFactoryOptions<T>, IOwinContext, T> createCallback, Action<IdentityFactoryOptions<T>, T> disposeCallback)
            where T : class, IDisposable =>
            throw new NotSupportedException("Configure Identity services with IdentityBuilder instead.");
    }
}
