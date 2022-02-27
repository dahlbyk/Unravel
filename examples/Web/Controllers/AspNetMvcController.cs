using System;
using System.Web.Mvc;
using UnravelExamples.Web.Services;

namespace UnravelExamples.Web.Controllers
{
    public class AspNetMvcController : Controller
    {
        private readonly IServiceProvider services;

        public AspNetMvcController(IServiceProvider services)
        {
            this.services = services;
        }

        public ActionResult Index()
        {
            var env = new EnvironmentCheck("ASP.NET MVC Controller Injection", services);
            return Content(env.ToString(), "application/json; charset=utf-8");
        }

        public new ActionResult PartialView()
        {
            var env = new EnvironmentCheck("ASP.NET MVC Partial View", services);
            return base.PartialView(nameof(EnvironmentCheck), env);
        }

        public new ActionResult View()
        {
            var env = new EnvironmentCheck("ASP.NET MVC View", services);
            return base.View(nameof(EnvironmentCheck), env);
        }

        public ActionResult ViewNotFound()
        {
            return base.View("ViewNotFound");
        }

        public ActionResult Upload(Microsoft.AspNetCore.Http.IFormFile testFile)
        {
            return Json(new
            {
                testFile.Name,
                testFile.FileName,
                testFile.ContentType,
                testFile.Length,
                Bytes = ReadContent(),
            });

            byte[] ReadContent()
            {
                byte[] buffer = new byte[testFile.Length];
                using (var stream = testFile.OpenReadStream())
                    stream.Read(buffer, 0, (int)testFile.Length);
                return buffer;
            }
        }
    }
}
