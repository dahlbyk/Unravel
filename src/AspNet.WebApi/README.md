# Unravel.AspNet.WebApi

Similar to ASP.NET Core 2.1's [`AddMvc()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.mvcservicecollectionextensions.addmvc?view=aspnetcore-2.1), Unravel provides an `AddAspNetWebApi()` extension method on `IServiceCollection` that registers a `System.Web.Http.Dependencies.IDependencyResolver`.

The resulting `IAspNetWebApiBuilder` is similar to [`IMvcBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.imvcbuilder?view=aspnetcore-2.1), providing an extension point for additional configuration:

- [`AddControllersAsServices()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.mvccoremvcbuilderextensions.addcontrollersasservices?view=aspnetcore-2.1)

```csharp
public override void ConfigureServices(IServiceCollection services)
{
    services.AddAspNetWebApi()
        .AddControllersAsServices();
}
```
