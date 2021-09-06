using System.Web.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace Unravel.SystemWeb
{
    public static class WebHostBuilderSystemWebExtensions
    {
        /// <summary>
        ///   Sets <see cref="IHostingEnvironment.EnvironmentName"/> to 'Development'
        ///   if <see cref="HostingEnvironment.IsDevelopmentEnvironment"/> is <c>true</c>
        ///   and not set otherwise by an environment variable,
        /// </summary>
        /// <param name="builder">The <see cref="IWebHostBuilder"/> to configure.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        public static IWebHostBuilder UseHostingEnvironment(this IWebHostBuilder builder)
        {
            if (HostingEnvironment.IsDevelopmentEnvironment)
            {
                if (builder.GetSetting(WebHostDefaults.EnvironmentKey) == null)
                {
                    builder.UseEnvironment(EnvironmentName.Development);
                }
            }

            return builder;
        }
    }
}
