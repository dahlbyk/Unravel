using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using Owin;

namespace UnravelExamples.Web.Services
{
    public class EnvironmentOwin : EnvironmentBase
    {
        public EnvironmentOwin(IServiceProvider services) : base(services)
        {
            AppBuilder = services.GetService<IAppBuilder>();
            OwinContext = services.GetScopedServiceOrDefault<IOwinContext>();
            AuthenticationManager = services.GetScopedServiceOrDefault<IAuthenticationManager>();
        }

        public IAppBuilder AppBuilder { get; }
        public IOwinContext OwinContext { get; }
        public IAuthenticationManager AuthenticationManager { get; }

        public override string EnvironmentName => "OWIN";

        public override JToken ToJson()
        {
            return new JObject
            {
                { nameof(IAppBuilder), ToJson(AppBuilder?.Properties, "owin.Version", "host.AppName", "host.AppMode") },
                { nameof(IOwinContext), ToJson(OwinContext?.Environment, "owin.Version", "owin.RequestPath", "owin.RequestPathBase", typeof(HttpContextBase).FullName) },
                { nameof(IAuthenticationManager), AuthenticationManager?.User?.GetType().FullName }
            };
        }

        private JObject ToJson(IDictionary<string, object> env, params string[] keys)
        {
            if (env == null)
                return null;

            var result = new JObject();
            foreach (var key in keys)
            {
                if (!env.TryGetValue(key, out var value))
                    result.Add(key, "(missing)");
                else if (value is null)
                    result.Add(key, null);
                else if (value is string || value.GetType().GetTypeInfo().IsPrimitive)
                    result.Add(key, value.ToString());
                else
                    result.Add(key, "(object)");
            }
            return result;
        }
    }
}
