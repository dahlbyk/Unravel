using System;
using Microsoft.AspNetCore.Mvc;
using UnravelExamples.Web.Services;

namespace UnravelExamples.Web.Controllers
{
    [ApiController]
    public class AspNetCoreApiController : Controller
    {
        private readonly IServiceProvider services;

        public AspNetCoreApiController(IServiceProvider services)
        {
            this.services = services;
        }

        [HttpGet("[controller]")]
        public ActionResult Index()
        {
            var env = new EnvironmentCheck("ASP.NET Core API Controller Injection", services);
            return Content(env.ToString(), "application/json; charset=utf-8");
        }

        [HttpGet("[controller]/[action]")]
        public ActionResult Throw()
        {
            throw new ApplicationException("From ASP.NET Core API Controller");
        }
    }
}
