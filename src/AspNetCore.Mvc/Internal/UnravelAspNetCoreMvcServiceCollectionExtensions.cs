using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Unravel.AspNetCore.Mvc.Internal
{
    /// <summary>
    ///   Extension methods for setting up ASP.NET Core MVC services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class UnravelAspNetCoreMvcServiceCollectionExtensions
    {
        public static void AddIgnoreControllersOfTypeActionInvokerProvider<T>(this IServiceCollection services)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IActionInvokerProvider, IgnoreControllersOfTypeActionInvokerProvider<T>>());
        }
    }
}
