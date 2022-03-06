using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UnravelExamples.Web.Services
{
    public class EnvironmentCheck
    {
        public EnvironmentCheck(string title, IServiceProvider services)
        {
            Title = title;
            Services = services;
        }

        public string Title { get; }
        public IServiceProvider Services { get; }

        public override string ToString()
        {
            var json = new JObject
            {
                { nameof(Title), Title },
                { nameof(IServiceProvider), Services == null ? null : "(object)" },
            };

            foreach (var environment in GetEnvironments())
                json.Add(environment.EnvironmentName, environment.ToJson());

            return json.ToString(Formatting.Indented);
        }

        public IEnumerable<EnvironmentBase> GetEnvironments()
        {
            if (Services == null)
                yield break;

            yield return new EnvironmentHosting(Services);
            yield return Services.GetService<Counters>();

            yield return new EnvironmentSystemWeb(Services);
            yield return new EnvironmentOwin(Services);
            yield return new EnvironmentAspNetCore(Services);
            yield return new EnvironmentConfiguration(Services);
            yield return new EnvironmentLogging(Services);
        }
    }
}
