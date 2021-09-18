using System;
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
    }
}
