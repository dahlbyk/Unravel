namespace Unravel.AspNet.Mvc
{
    /// <summary>
    /// Provides consolidated programmatic configuration for the ASP.NET MVC framework.
    /// </summary>
    public class MvcOptions
    {
        /// <inheritdoc cref="System.Web.Mvc.MvcHandler.DisableMvcResponseHeader"/>
        public bool DisableMvcResponseHeader
        {
            get => System.Web.Mvc.MvcHandler.DisableMvcResponseHeader;
            set => System.Web.Mvc.MvcHandler.DisableMvcResponseHeader = value;
        }

        /// <inheritdoc cref="GlobalFilters.Filters"/>
        public System.Web.Mvc.GlobalFilterCollection Filters =>
            System.Web.Mvc.GlobalFilters.Filters;

        /// <inheritdoc cref="System.Web.ModelBinding.ModelBinders.Binders"/>
        public System.Web.Mvc.ModelBinderDictionary ModelBinders =>
            System.Web.Mvc.ModelBinders.Binders;

        /// <inheritdoc cref="System.Web.ModelBinding.ModelBinderProviders.Providers"/>
        public System.Web.Mvc.ModelBinderProviderCollection ModelBinderProviders =>
            System.Web.Mvc.ModelBinderProviders.BinderProviders;

        /// <inheritdoc cref="System.Web.Routing.RouteTable.Routes"/>
        public System.Web.Routing.RouteCollection Routes =>
            System.Web.Routing.RouteTable.Routes;

        /// <inheritdoc cref="System.Web.Mvc.ViewEngines.Engines"/>
        public System.Web.Mvc.ViewEngineCollection ViewEngines =>
            System.Web.Mvc.ViewEngines.Engines;

        /// <inheritdoc cref="System.Web.Mvc.AreaRegistration.RegisterAllAreas()"/>
        public void RegisterAllAreas() => System.Web.Mvc.AreaRegistration.RegisterAllAreas();

        /// <inheritdoc cref="System.Web.Mvc.AreaRegistration.RegisterAllAreas(object)"/>
        public void RegisterAllAreas(object state) => System.Web.Mvc.AreaRegistration.RegisterAllAreas(state);
    }
}
