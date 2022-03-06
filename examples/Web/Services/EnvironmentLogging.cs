using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace UnravelExamples.Web.Services
{
    public class EnvironmentLogging : EnvironmentBase
    {
        public EnvironmentLogging(IServiceProvider services) : base(services)
        {
            LoggerProviders = services.GetServices<ILoggerProvider>();
        }

        public IEnumerable<ILoggerProvider> LoggerProviders { get; }

        public override string EnvironmentName => typeof(ILoggerProvider).FullName;

        public override JToken ToJson()
        {
            return new JArray(LoggerProviders.Select(lp => lp.GetType().FullName));
        }
    }
}
