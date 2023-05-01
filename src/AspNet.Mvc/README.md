# Unravel.AspNet.Mvc

Similar to ASP.NET Core 2.1's [`AddMvc()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.mvcservicecollectionextensions.addmvc?view=aspnetcore-2.1), Unravel provides an `AddAspNetMvc()` extension method on `IServiceCollection` that registers a `System.Web.Mvc.IDependencyResolver`.

The resulting `IAspNetMvcBuilder` is similar to [`IMvcBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.imvcbuilder?view=aspnetcore-2.1), providing an extension point for additional configuration:

- [`AddControllersAsServices()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.mvccoremvcbuilderextensions.addcontrollersasservices?view=aspnetcore-2.1)

```csharp
public override void ConfigureServices(IServiceCollection services)
{
    services.AddAspNetMvc()
        .AddControllersAsServices();
}
```

## `MvcOptions`

Also similar to ASP.NET Core 2.1, `AddAspNetMvc()` accepts an optional `Action<MvcOptions>` to consolidate configuration, including:

```csharp
        .AddAspNetMvc(options =>
        {
            // Equivalent to AreaRegistration.RegisterAllAreas()
            options.RegisterAllAreas();

            // Equivalent to MvcHandler.DisableMvcResponseHeader
            options.DisableMvcResponseHeader = true;

            // Equivalent to GlobalFilters.Filters.Add(...)
            options.Filters.Add(...);

            // Equivalent to ModelBinders.Binders.Add(...)
            options.ModelBinders.Add(...);

            // Equivalent to RouteTable.Routes.MapRoute(...);
            options.Routes.MapRoute(...);
        })
```

## `IFormFile` Model Binder

`AddAspNetMvc()` registers a model binder for `IFormFile`, to replace references to
[`System.Web.HttpPostedFileBase`](https://docs.microsoft.com/en-us/dotnet/api/system.web.httppostedfilebase?view=netframework-4.8).

Some `IFormFile` properties are not supported, due to `HttpPostedFileBase` limitations.
