using Unravel.AspNetCore.Mvc.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///   Extensions for configuring MVC using an <see cref="IMvcBuilder"/>.
    /// </summary>
    public static class UnravelAspNetCoreMvcBuilderExtensions
    {
        /// <summary>
        ///   Configures ASP.NET Core to not handle controllers assignable from <typeparamref name="TController"/>,
        ///   e.g. <c>System.Web.Mvc.Controller</c> or <c>System.Web.Http.ApiController</c>.
        /// </summary>
        /// <typeparam name="TController">The controller base type to ignore.</typeparam>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder IgnoreControllersOfType<TController>(this IMvcBuilder builder)
        {
            builder.Services.AddIgnoreControllersOfTypeActionInvokerProvider<TController>();
            return builder;
        }
    }
}
