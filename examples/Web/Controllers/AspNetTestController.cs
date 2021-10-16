using System;
using System.Web;
using System.Web.Mvc;
using Unravel;
using UnravelExamples.Web.Services;

namespace UnravelExamples.Web.Controllers
{
    public class AspNetTestController : Controller
    {
        private readonly IServiceProvider services;

        public AspNetTestController(IServiceProvider services)
        {
            this.services = services;
        }

        public ActionResult Index()
        {
            var env = new EnvironmentCheck("ASP.NET Controller Injection", services);
            return Content(env.ToString(), "application/json; charset=utf-8");
        }
    }
}
