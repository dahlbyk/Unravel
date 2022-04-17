using System.Collections;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace Unravel.Startup.Configuration
{
    // https://benfoster.io/blog/net-core-configuration-legacy-projects/
    /// <summary>
    /// <see cref="IConfigurationProvider"/> loaded from
    /// <see cref="ConfigurationManager.ConnectionStrings"/> and
    /// <see cref="ConfigurationManager.AppSettings"/>.
    /// </summary>
    public class ConfigurationManagerConfigurationProvider : ConfigurationProvider, IConfigurationSource
    {
        public override void Load()
        {
            foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
            {
                Data.Add($"ConnectionStrings:{connectionString.Name}", connectionString.ConnectionString);
            }

            var appSettings = ConfigurationManager.AppSettings;
            foreach (var key in appSettings.AllKeys)
            {
                Data.Add(key, appSettings[key]);
            }
        }

        IConfigurationProvider IConfigurationSource.Build(IConfigurationBuilder builder) => this;
    }
}
