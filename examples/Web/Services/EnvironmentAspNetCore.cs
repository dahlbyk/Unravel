using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace UnravelExamples.Web.Services
{
    public class EnvironmentAspNetCore : EnvironmentBase
    {
        public EnvironmentAspNetCore(IServiceProvider services) : base(services)
        {
            Server = services.GetService<IServer>();
            HttpContext = services.GetService<IHttpContextAccessor>()?.HttpContext;
            HttpRequest = HttpContext?.Request;
        }

        public IServer Server { get; }
        public HttpContext HttpContext { get; }
        public HttpRequest HttpRequest { get; }

        public override string EnvironmentName => "Microsoft.AspNetCore";

        public override JToken ToJson()
        {
            return new JObject
            {
                { nameof(IServer), Server?.ToString() },
                {
                    nameof(HttpContext),
                    HttpContext == null ?
                        null :
                        JObject.FromObject(HttpContext.Features.ToDictionary(kvp => kvp.Key.Name, kvp => kvp.Value.GetType().FullName))
                },
                {
                    nameof(HttpRequest),
                    HttpRequest == null ?
                        null :
                        JObject.FromObject(new
                        {
                            HttpRequest?.PathBase,
                            HttpRequest?.Path,
                        })
                },
                { nameof(HttpContext.User), ToJson(HttpContext?.User) },
            };
        }
    }
}
