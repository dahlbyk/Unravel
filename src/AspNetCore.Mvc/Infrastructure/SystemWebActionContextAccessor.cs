using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Unravel.AspNetCore.Mvc.Infrastructure
{
    /// <summary>
    /// Stores current <see cref="Microsoft.AspNetCore.Mvc.ActionContext"/> in
    /// <see cref="System.Web.HttpContext.Current"/>.Items
    /// instead of an <see cref="AsyncLocal{T}"/> field.
    /// </summary>
    public class SystemWebActionContextAccessor : IActionContextAccessor
    {
        /// <summary>
        /// Key in <see cref="HttpContext.Items"/> for current <see cref="ActionContext"/>.
        /// </summary>
        public static readonly Type ActionContextKey = typeof(ActionContext);

        public ActionContext? ActionContext
        {
            get
            {
                return System.Web.HttpContext.Current?.Items[ActionContextKey] as ActionContext;
            }
            set
            {
                var httpContext = System.Web.HttpContext.Current ?? throw new InvalidOperationException("HttpContext not found.");
                httpContext.Items[ActionContextKey] = value;
            }
        }
    }
}
