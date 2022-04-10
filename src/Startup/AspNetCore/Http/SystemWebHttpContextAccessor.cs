using System;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace Unravel.AspNetCore.Http
{
    /// <summary>
    /// Stores current <see cref="Microsoft.AspNetCore.Http.HttpContext"/> in
    /// <see cref="System.Web.HttpContext.Current"/>.Items
    /// instead of an <see cref="AsyncLocal{T}"/> field.
    /// </summary>
    public class SystemWebHttpContextAccessor : IHttpContextAccessor
    {
        /// <summary>
        /// Key in <see cref="HttpContext.Items"/> for current <see cref="HttpContext"/>.
        /// </summary>
        public static readonly Type HttpContextKey = typeof(HttpContext);

        public HttpContext HttpContext
        {
            get
            {
                return System.Web.HttpContext.Current?.Items[HttpContextKey] as HttpContext;
            }
            set
            {
                var httpContext = System.Web.HttpContext.Current ?? throw new InvalidOperationException("HttpContext not found.");
                httpContext.Items[HttpContextKey] = value;
            }
        }
    }
}
