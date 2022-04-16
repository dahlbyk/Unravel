using System.Configuration;
using Unravel.Startup.Configuration;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationManagerConfigurationExtensions
    {
        /// <summary>
        /// Adds the <see cref="ConfigurationManager"/> configuration provider to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddConfigurationManager(this IConfigurationBuilder builder)
        {
            return builder.Add(new ConfigurationManagerConfigurationProvider());
        }
    }
}
