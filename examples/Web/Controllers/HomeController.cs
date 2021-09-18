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
    }
}
