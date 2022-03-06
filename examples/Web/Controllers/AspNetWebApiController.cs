using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using UnravelExamples.Web.Services;

namespace UnravelExamples.Web.Controllers
{
    public class AspNetWebApiController : ApiController
    {
        private readonly IServiceProvider services;

        public AspNetWebApiController(IServiceProvider services)
        {
            this.services = services;
        }

        [HttpGet]
        [Route("AspNetWebApi")]
        public IHttpActionResult Index()
        {
            var env = new EnvironmentCheck("ASP.NET Core API Controller Injection", services);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(env.ToString(), Encoding.UTF8, "application/json");
            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("AspNetWebApi/Throw")]
        public IHttpActionResult Throw()
        {
            throw new ApplicationException("From ASP.NET Core API Controller");
        }
    }
}
