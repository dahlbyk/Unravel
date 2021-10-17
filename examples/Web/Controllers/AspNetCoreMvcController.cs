using System;
using Microsoft.AspNetCore.Mvc;
using UnravelExamples.Web.Services;

namespace UnravelExamples.Web.Controllers
{
    public class AspNetCoreMvcController : Controller
    {
        private readonly IServiceProvider services;

        public AspNetCoreMvcController(IServiceProvider services)
        {
            this.services = services;
        }

        public ActionResult Index()
        {
            var env = new EnvironmentCheck("ASP.NET Core MVC Controller Injection", services);
            return Content(env.ToString(), "application/json; charset=utf-8");
        }

        public ActionResult Throw()
        {
            throw new ApplicationException("From ASP.NET Core MVC Controller");
        }
    }
}
