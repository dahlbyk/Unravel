using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Options;
using ControllerContext = System.Web.Mvc.ControllerContext;
using IHttpContextAccessor = System.Web.IHttpContextAccessor;
using MvcOptions = Unravel.AspNet.Mvc.MvcOptions;
using RouteData = System.Web.Routing.RouteData;

namespace Unravel.AspNetCore.Mvc.ViewEngines
{
    public class AspNetMvcCompositeViewEngine : ICompositeViewEngine
    {
        private readonly MvcOptions mvcOptions;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AspNetMvcCompositeViewEngine(IOptions<MvcOptions> mvcOptions, IHttpContextAccessor httpContextAccessor)
        {
            this.mvcOptions = mvcOptions.Value;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IReadOnlyList<IViewEngine> ViewEngines => new List<IViewEngine>(0);

        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
        {
            var controllerContext = CreateControllerContext(context);

            var result = isMainPage ?
                mvcOptions.ViewEngines.FindView(controllerContext, viewName, null) :
                mvcOptions.ViewEngines.FindPartialView(controllerContext, viewName);

            if (result.View != null)
            {
                return ViewEngineResult.Found(viewName, AdaptView(controllerContext, result.View));
            }

            return ViewEngineResult.NotFound(viewName, result.SearchedLocations ?? Enumerable.Empty<string>());
        }

        public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
        {
            return ViewEngineResult.NotFound(viewPath, Enumerable.Empty<string>());
        }

        protected ControllerContext CreateControllerContext(ActionContext context)
        {
            var routeData = AdaptRouteData(context);

            return new ControllerContext
            {
                HttpContext = httpContextAccessor.HttpContext,
                RouteData = routeData,
            };
        }

        protected virtual RouteData AdaptRouteData(ActionContext context)
        {
            var routeData = new RouteData();

            var routeDataValues = routeData.Values;
            foreach (var kvp in context.RouteData.Values)
                routeDataValues.Add(kvp.Key, kvp.Value);

            return routeData;
        }

        protected virtual IView AdaptView(ControllerContext controllerContext, System.Web.Mvc.IView view)
        {
            return new AspNetMvcView(controllerContext, view);
        }
    }
}
