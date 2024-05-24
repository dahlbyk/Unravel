# Unravel.Startup

Unravel provides a fully configurable ASP.NET Core `IWebHost` on top of `System.Web` via OWIN, with forward-compatible dependency injection, configuration and logging.

- `Unravel.Application`
  - Inherits from `System.Web.HttpApplication`
  - Provides OWIN Startup
  - Provides ASP.NET Core Startup, including `ConfigureServices(IServiceCollection)`
  - Builds an ASP.NET Core `IWebHost`, including an `IServiceProvider`
    - Static `Unravel.Application.Services` service locator
    - Scoped per `HttpContext`
    - With `GetRequestServices()` extension methods
- `OwinHost.CreateDefaultBuilder()`, equivalent to [`WebHost.CreateDefaultBuilder()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.webhost.createdefaultbuilder?view=aspnetcore-2.1).
  - `Microsoft.Extensions.Configuration`
    - `ConfigurationManagerConfigurationProvider` inspired by [@benfoster](https://benfoster.io/blog/net-core-configuration-legacy-projects/)
    - `appsettings.json` and `appsettings.{env}.json`
    - User Secrets (Development only)
    - Environment Variables
  - `Microsoft.Extensions.DependencyInjection`
  - `Microsoft.Extensions.Logging`
    - Configured from `IConfiguration`
    - Debug Logger

## Setup

1. Install `Unravel.Startup` in your Web Application project
1. Search the project for an existing [OWIN Startup Class](https://learn.microsoft.com/en-us/aspnet/aspnet/overview/owin-and-katana/owin-startup-class-detection):
    - There may be an `OwinStartup` attribute, e.g.

      ```csharp
      [assembly: OwinStartup(typeof(UnravelExamples.Web.OwinStartup))]
      ```

    - `Web.config` may contain `owin:appStartup`, e.g.

      ```xml
      <appSettings>
        <add key="owin:appStartup" value="UnravelExamples.Web.OwinStartup" />
      </appSettings>
      ```

    In the instructions that follow, use that class name instead of `Startup` (or rename it to `Startup`).

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
    1. Replace `void Configuration(IAppBuilder app)` with `override void ConfigureOwin(IAppBuilder app)`

   If you don't, create one.

   Then you can `override ConfigureServices()`, too.

    ```csharp
    // Startup.cs
    public partial class Startup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
        }

        public override void ConfigureOwin(IAppBuilder app)
        {
        }
    }
    ```
