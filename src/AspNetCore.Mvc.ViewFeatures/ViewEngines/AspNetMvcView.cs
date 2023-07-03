using System.Threading.Tasks;
using System.Web.Mvc;

namespace Unravel.AspNetCore.Mvc.ViewEngines
{
    public class AspNetMvcView : Microsoft.AspNetCore.Mvc.ViewEngines.IView
    {
        private readonly ControllerContext controllerContext;
        private readonly IView view;

        public AspNetMvcView(ControllerContext controllerContext, IView view)
        {
            this.controllerContext = controllerContext;
            this.view = view;
        }

        public string? Path => (view as BuildManagerCompiledView)?.ViewPath;

        public Task RenderAsync(Microsoft.AspNetCore.Mvc.Rendering.ViewContext context)
        {
            var viewContext = AdaptViewContext(context);
            view.Render(viewContext, context.Writer);
            return Task.CompletedTask;
        }

        protected virtual ViewContext AdaptViewContext(Microsoft.AspNetCore.Mvc.Rendering.ViewContext context)
        {
            var viewData = AdaptViewData(context);
            var tempData = AdaptTempData(context);

            var viewContext = new ViewContext(controllerContext, view, viewData, tempData, context.Writer);
            return viewContext;
        }

        protected virtual TempDataDictionary AdaptTempData(Microsoft.AspNetCore.Mvc.Rendering.ViewContext context)
        {
            // TODO: Share ITempDataProvider?
            var source = context.TempData;
            var tempData = new TempDataDictionary();

            foreach (var kvp in source)
                tempData.Add(kvp.Key, kvp.Value);

            return tempData;
        }

        protected virtual ViewDataDictionary AdaptViewData(Microsoft.AspNetCore.Mvc.Rendering.ViewContext context)
        {
            var source = context.ViewData;
            var viewData = new ViewDataDictionary
            {
                Model = source.Model
            };

            foreach (var kvp in source)
                viewData.Add(kvp.Key, kvp.Value);

            var modelState = viewData.ModelState;
            foreach (var kvp in source.ModelState)
                modelState.Add(
                    kvp.Key,
                    new ModelState
                    {
                        // TODO: Culture?
                        Value = new ValueProviderResult(kvp.Value.RawValue, kvp.Value.AttemptedValue, null)
                    });

            // TODO: ModelMetadata?

            return viewData;
        }
    }
}
