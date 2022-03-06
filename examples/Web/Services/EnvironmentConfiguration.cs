using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;

namespace UnravelExamples.Web.Services
{
    public class EnvironmentConfiguration : EnvironmentBase
    {
        public EnvironmentConfiguration(IServiceProvider services) : base(services)
        {
            Configuration = services.GetService<IConfiguration>();
            HostingEnvironment = services.GetService<IHostingEnvironment>();
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public override string EnvironmentName => typeof(IConfiguration).ToString();

        public override JToken ToJson()
        {
            switch (Configuration)
            {
                case null:
                    return null;

                case IConfigurationRoot configRoot:
                    return new JArray(configRoot.Providers.Select(cp => Dump(cp)));

                default:
                    return Configuration.GetType().ToString();
            }
        }

        private JToken Dump(IConfigurationProvider configProvider)
        {
            switch (configProvider)
            {
                case FileConfigurationProvider fileConfigProvider:
                    var root = (fileConfigProvider.Source.FileProvider as PhysicalFileProvider)?.Root;

                    if (HostingEnvironment != null)
                        root = root.Replace(HostingEnvironment.ContentRootPath ?? "", ".");

                    return JObject.FromObject(new
                    {
                        Type = configProvider.GetType().ToString(),
                        FilePath = Path.Combine(root, fileConfigProvider.Source.Path),
                        fileConfigProvider.Source.Optional,
                    });

                default:
                    return configProvider.GetType().ToString();
            }
        }
    }
}
