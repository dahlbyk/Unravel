using Microsoft.AspNetCore.Mvc.ViewEngines;
using Unravel.AspNetCore.Mvc.ViewEngines;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///   Extensions for configuring MVC using an <see cref="IMvcBuilder"/>.
    /// </summary>
    public static class UnravelViewFeaturesAspNetCoreMvcBuilderExtensions
    {
        /// <summary>
        ///   Configures ASP.NET Core to use ASP.NET MVC view compilation.
        /// </summary>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddAspNetMvcViewEngines(this IMvcBuilder builder)
        {
            builder.Services.AddSingleton<ICompositeViewEngine, AspNetMvcCompositeViewEngine>();

            return builder;
        }
    }
}
