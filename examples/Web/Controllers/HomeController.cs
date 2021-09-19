using System.Web;
using System.Web.Mvc;
using Unravel;
using UnravelExamples.Web.Services;

namespace UnravelExamples.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Unravel_Application_Services()
        {
            var services = Application.Services;
            var env = new EnvironmentCheck("Unravel.Application.Services", services);
            return Content(env.ToString(), "application/json; charset=utf-8");
        }

        public ActionResult HttpContextBase_GetRequestServices()
        {
            var services = HttpContext.GetRequestServices();
            var env = new EnvironmentCheck("HttpContextBase.GetRequestServices()", services);
            return Content(env.ToString(), "application/json; charset=utf-8");
        }

        public ActionResult HttpContext_Current_GetRequestServices()
        {
            var services = System.Web.HttpContext.Current.GetRequestServices();
            var env = new EnvironmentCheck("HttpContext.GetRequestServices()", services);
            return Content(env.ToString(), "application/json; charset=utf-8");
        }

        public ActionResult Throw()
        {
            throw new System.ApplicationException("From ASP.NET MVC");
        }
    }
}
