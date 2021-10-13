using System.Web;

namespace Unravel.SystemWeb
{
    public class HttpContextAccessor : IHttpContextAccessor
    {
        public HttpContextBase HttpContext
        {
            get
            {
                var context = System.Web.HttpContext.Current;
                return context == null ? null : new HttpContextWrapper(context);
            }
        }
    }
}
