using System;
using System.Reflection;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace UnravelExamples.Web.Services
{
    public class ViewEnvironment : EnvironmentBase
    {
        public ViewEnvironment(WebViewPage page) : base(null)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
        }

        public WebViewPage Page { get; }

        public override string EnvironmentName => typeof(WebViewPage).ToString();

        public override JToken ToJson()
        {
            var res = new JObject();
            var pageType = Page.GetType();
            foreach (var prop in pageType.GetProperties())
            {
                var propName = pageType == prop.DeclaringType ? prop.Name : $"{prop.DeclaringType.Name}.{prop.Name}";
                res.Add(propName, GetDisplayValue(prop));
            }
            return res;
        }

        private JToken GetDisplayValue(PropertyInfo prop)
        {
            var value = prop.GetValue(Page);
            if (value is string || prop.PropertyType.IsPrimitive)
                return JToken.FromObject(value);
            else
                return prop.PropertyType.ToString();
        }
    }
}
