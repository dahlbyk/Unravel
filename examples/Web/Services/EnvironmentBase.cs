using System;
using System.Security.Principal;
using Newtonsoft.Json.Linq;

namespace UnravelExamples.Web.Services
{
    public abstract class EnvironmentBase
    {
        protected IServiceProvider services;

        protected EnvironmentBase(IServiceProvider services)
        {
            this.services = services;
        }

        public abstract string EnvironmentName { get; }

        public abstract JToken ToJson();

        protected JToken ToJson(IPrincipal user)
        {
            if (user == null)
                return null;

            return JObject.FromObject(new
            {
                Type = user.GetType().ToString(),
                user.Identity.IsAuthenticated,
                user.Identity.Name,
            });
        }

        public override string ToString()
        {
            return ToJson().ToString(Newtonsoft.Json.Formatting.Indented);
        }
    }
}
