using Unravel.AspNetCore.Mvc.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///   Extensions for configuring MVC using an <see cref="IMvcCoreBuilder"/>.
    /// </summary>
    public static class UnravelAspNetCoreMvcCoreBuilderExtensions
    {
        /// <summary>
        ///   Configures ASP.NET Core to not handle controllers assignable from <typeparamref name="TController"/>,
        ///   e.g. <c>System.Web.Mvc.Controller</c> or <c>System.Web.Http.ApiController</c>.
        /// </summary>
        /// <typeparam name="TController">The controller base type to ignore.</typeparam>
        /// <returns>The <see cref="IMvcCoreBuilder"/>.</returns>
        public static void IgnoreControllersOfType<TController>(this IMvcCoreBuilder builder)
        {
            builder.Services.AddIgnoreControllersOfTypeActionInvokerProvider<TController>();
        }
    }
}
