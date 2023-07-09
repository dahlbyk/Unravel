# Unravel.AspNetCore.Mvc

Unravel will _try_ to enable as much of ASP.NET Core as possible. (Experimental!)

Extending [`IMvcBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.imvcbuilder?view=aspnetcore-2.1) and [`IMvcCoreBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.imvccorebuilder?view=aspnetcore-2.1):

- `IgnoreControllersOfType<TController>()` prevents ASP.NET Core from trying to invoke legacy ASP.NET controllers.

```csharp
public override void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
        .IgnoreControllersOfType<System.Web.Mvc.IController>()
        .IgnoreControllersOfType<System.Web.Http.Controllers.IHttpController>();
}
```
