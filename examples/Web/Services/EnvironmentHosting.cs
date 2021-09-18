using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace UnravelExamples.Web.Services
{
    public class EnvironmentHosting : EnvironmentBase
    {
        public EnvironmentHosting(IServiceProvider services) : base(services)
        {
            HostingEnvironment = services.GetService<IHostingEnvironment>();
        }

        public IHostingEnvironment HostingEnvironment { get; }

        public override string EnvironmentName => typeof(IHostingEnvironment).FullName;

        public override JToken ToJson()
        {
            return JObject.FromObject(new
            {
                HostingEnvironment.ApplicationName,
                HostingEnvironment.EnvironmentName,
                HostingEnvironment.ContentRootPath,
            });
        }
    }
}
