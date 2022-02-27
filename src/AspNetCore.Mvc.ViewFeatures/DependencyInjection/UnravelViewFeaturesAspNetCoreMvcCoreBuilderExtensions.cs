using Microsoft.AspNetCore.Mvc.ViewEngines;
using Unravel.AspNetCore.Mvc.ViewEngines;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///   Extensions for configuring MVC using an <see cref="IMvcCoreBuilder"/>.
    /// </summary>
    public static class UnravelViewFeaturesAspNetCoreMvcCoreBuilderExtensions
    {
        /// <summary>
        ///   Configures ASP.NET Core to use ASP.NET MVC view compilation.
        /// </summary>
        /// <returns>The <see cref="IMvcCoreBuilder"/>.</returns>
        public static IMvcCoreBuilder AddAspNetMvcViewEngines(this IMvcCoreBuilder builder)
        {
            builder.Services.AddSingleton<ICompositeViewEngine, AspNetMvcCompositeViewEngine>();

            return builder;
        }
    }
}
