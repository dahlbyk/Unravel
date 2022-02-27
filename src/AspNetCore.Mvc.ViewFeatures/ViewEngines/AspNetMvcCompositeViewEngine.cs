using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Unravel.AspNetCore.Mvc.ViewEngines
{
    public class AspNetMvcCompositeViewEngine : ICompositeViewEngine
    {
        public IReadOnlyList<IViewEngine> ViewEngines => new List<IViewEngine>(0);

        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
        {
            return ViewEngineResult.NotFound(viewName, Enumerable.Empty<string>());
        }

        public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
        {
            return ViewEngineResult.NotFound(viewPath, Enumerable.Empty<string>());
        }
    }
}
