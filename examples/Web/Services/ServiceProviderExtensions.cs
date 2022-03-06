using System;
using Microsoft.Extensions.DependencyInjection;

namespace UnravelExamples.Web.Services
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        ///   Catches exception thrown with <see cref="ServiceProviderOptions.ValidateScopes"/> enabled.
        /// </summary>
        /// <param name="services">The <see cref="IServiceProvider"/>.</param>
        /// <typeparam name="T">The service type.</typeparam>
        /// <returns>The service of type <typeparamref name="T"/>, or <c>default(T)</c> if scope not available.</returns>
        public static T GetScopedServiceOrDefault<T>(this IServiceProvider services)
        {
            try
            {
                return services.GetService<T>();
            }
            catch (InvalidOperationException)
            {
                return default;
            }
        }
    }
}
