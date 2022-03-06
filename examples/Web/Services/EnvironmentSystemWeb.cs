using System;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace UnravelExamples.Web.Services
{
    public class EnvironmentSystemWeb : EnvironmentBase
    {
        public EnvironmentSystemWeb(IServiceProvider services) : base(services)
        {
            HttpContext = services.GetService<IHttpContextAccessor>()?.HttpContext;
            HttpRequest = HttpContext?.Request;
        }

        public HttpContextBase HttpContext { get; set; }
        public HttpRequestBase HttpRequest { get; set; }

        public override string EnvironmentName => "System.Web";

        public override JToken ToJson()
        {
            return new JObject
            {
                { nameof(HttpApplication), ToJson(HttpContext?.ApplicationInstance) },
                { nameof(HttpContext), HttpContext?.Handler?.GetType().FullName },
                {
                    nameof(HttpRequest),
                    HttpRequest == null ?
                        null :
                        JObject.FromObject(new
                        {
                            HttpRequest?.Path,
                        })
                },
                { nameof(HttpContext.User), ToJson(HttpContext?.User) },
            };
        }

        private JToken ToJson(HttpApplication application)
        {
            var result = new JArray();

            var type = application.GetType();
            while (type.BaseType != null)
            {
                result.Add(type.FullName);
                type = type.BaseType;
            }

            return result;
        }
    }
}
