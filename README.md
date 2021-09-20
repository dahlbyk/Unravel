# Unravel

Unravel your dependence on `System.Web`.

## Goals

Facilitate in-place upgrade from ASP.NET MVC and Web API on .NET Framework to ASP.NET Core 2.1 (the last version [supported on .NET Framework](https://dotnet.microsoft.com/platform/support/policy/aspnet#dotnet-core)). Long-lived upgrade branches are bad.

The ultimate goal is that an upgrade to ASP.NET Core could be reduced to roughly:
1. Remove `Global.asax`, `Web.config`, and other lingering ASP.NET artifacts
1. Remove `<ProjectTypeGuids>` from `.csproj`
1. Migrate `.csproj` to [SDK format](https://docs.microsoft.com/en-us/dotnet/core/project-sdk/overview) with [Project2015To2017.Migrate2019.Tool](https://github.com/hvanbakel/CsprojToVs2017)
1. Convert remaining references to OWIN `IAppBuilder` to `UseOwin()`
1. Update `Startup.cs` to inherit from `Microsoft.AspNetCore.Hosting.StartupBase`
1. Convert project to .NET Core Console Application with standard ASP.NET Core `Program.cs`

We'll see how close we can get…

## Unravel.Startup

- `Unravel.Application`
  - Inherits from `System.Web.HttpApplication`
  - Provides OWIN Startup
  - Implements ASP.NET Core `IStartup`, including `ConfigureServices(IServiceCollection)`
  - Builds an ASP.NET Core `IWebHost`, including an `IServiceProvider`
    - Static `Unravel.Application.Services` service locator
    - Scoped per `HttpContext`
    - With `GetRequestServices()` extension methods
- `OwinHost.CreateDefaultBuilder()`, equivalent to [`WebHost.CreateDefaultBuilder()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.webhost.createdefaultbuilder?view=aspnetcore-2.1).
  - `Microsoft.Extensions.Configuration`
    - `appsettings.json` and `appsettings.{env}.json`
    - User Secrets (Development only)
    - TODO: Integrates with `ConfigurationManager`
  - `Microsoft.Extensions.DependencyInjection`
  - `Microsoft.Extensions.Logging`
    - Configured from `IConfiguration`
    - Debug Logger

### Startup Setup

1. Install `Unravel.Startup`
1. Open `Global.asax.cs`:
    - Rename the class to `Startup`
    - Make the class `partial`
    - Inherit from `Unravel.Application`

    ```diff
    -public class MvcApplication : System.Web.HttpApplication
    +public partial class Startup : Unravel.Application
    ```

    Make sure the `Inherits` in `Global.asax` updated, too

      ```diff
      -<%@ Application CodeBehind="Global.asax.cs" Inherits="UnravelExamples.Web.MvcApplication" Language="C#" %>
      +<%@ Application CodeBehind="Global.asax.cs" Inherits="UnravelExamples.Web.Startup" Language="C#" %>
      ```

1. If you already have a `Startup.cs`:
    1. Make it `partial`
    1. Replace `void Configuration(IAppBuilder app)` with `override void Configure(IAppBuilder app)`

   If you don't, create one.

   Then you can `override ConfigureServices()`, too.

    ```csharp
    // Startup.cs
    public partial class Startup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
        }

        public override void Configure(IAppBuilder app)
        {
        }
    }
    ```

## Credits

- `Startup` concept from [Arex388.AspNet.Mvc.Startup](https://github.com/arex388/Arex388.AspNet.Mvc.Startup)
- [David Fowler's Gist](https://gist.github.com/davidfowl/563a602936426a18f67cd77088574e61)
- [Scott Dorman's Blog](https://scottdorman.blog/2016/03/17/integrating-asp-net-core-dependency-injection-in-mvc-4/)
- The [StackOverflow question](https://stackoverflow.com/questions/43311099/how-to-create-dependency-injection-for-asp-net-mvc-5) that lead @arex388 to David and Scott.
