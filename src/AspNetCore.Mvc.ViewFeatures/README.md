# Unravel.AspNetCore.Mvc

Unravel will _try_ to enable as much of ASP.NET Core as possible. (Experimental!)

Extending [`IMvcBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.imvcbuilder?view=aspnetcore-2.1) and [`IMvcCoreBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.imvccorebuilder?view=aspnetcore-2.1):

- `AddAspNetMvcViewEngines()` registers the ASP.NET compiler as an ASP.NET Core view engine.
  - The ASP.NET Core view engine is incompatible with legacy projects, as are new features like Tag Helpers.

```csharp
public override void ConfigureServices(IServiceCollection services)
{
    services.AddMvc()
        .AddAspNetMvcViewEngines();
}
```
