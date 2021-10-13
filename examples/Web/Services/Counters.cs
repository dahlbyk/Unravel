using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace UnravelExamples.Web.Services
{
    public class Counters : EnvironmentBase
    {
        private readonly SingletonCounter singleton;
        private readonly ScopedCounter scoped;
        private readonly TransientCounter transient;

        public Counters(IServiceProvider services, SingletonCounter singleton, TransientCounter transient) : base(services)
        {
            this.singleton = singleton;
            scoped = services.GetScopedServiceOrDefault<ScopedCounter>();
            this.transient = transient;

            VerifyLifetime(services);
        }

        public override string EnvironmentName => "Counters";

        public override JToken ToJson()
        {
            return new JObject
            {
                { "Singleton", singleton?.Count },
                { "Scoped", scoped?.Count },
                { "Transient", transient?.Count },
            };
        }

        private void VerifyLifetime(IServiceProvider services)
        {
            if (singleton.Count != 1)
                throw new InvalidOperationException("Singleton Count should always be 1.");

            if (scoped?.Count != services.GetScopedServiceOrDefault<ScopedCounter>()?.Count)
                throw new InvalidOperationException("Scoped Count did not match within same request.");

            var newTransientCount = services.GetService<TransientCounter>()?.Count;
            if (transient.Count == newTransientCount)
                throw new InvalidOperationException(string.Format("Transient Count should never match (old = {0}, new = {1}).", transient.Count, newTransientCount));
        }

        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<SingletonCounter>();
            services.AddScoped<ScopedCounter>();
            services.AddTransient<TransientCounter>();
            services.AddTransient<Counters>();
        }

        public class SingletonCounter
        {
            static int count = 0;
            public int Count { get; } = Interlocked.Increment(ref count);
        }

        public class ScopedCounter
        {
            static int count = 0;
            public int Count { get; } = Interlocked.Increment(ref count);
        }

        public class TransientCounter
        {
            static int count = 0;
            public int Count { get; } = Interlocked.Increment(ref count);
        }
    }
}
