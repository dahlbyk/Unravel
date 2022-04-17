# Unravel

Unravel your dependence on `System.Web`.

## Goals

Facilitate in-place upgrade from ASP.NET MVC and Web API on .NET Framework to ASP.NET Core 2.1 (the last version [supported on .NET Framework](https://dotnet.microsoft.com/platform/support/policy/aspnet#dotnet-core)). Long-lived upgrade branches are bad.

The ultimate goal is that an upgrade to ASP.NET Core could be reduced to roughly:
1. Remove `Global.asax`, `Web.config`, and other lingering ASP.NET artifacts
1. Remove `<ProjectTypeGuids>` from `.csproj`
1. Migrate `.csproj` to [SDK format](https://docs.microsoft.com/en-us/dotnet/core/project-sdk/overview) with [Project2015To2017.Migrate2019.Tool](https://github.com/hvanbakel/CsprojToVs2017)
1. Convert remaining references to OWIN `IAppBuilder` to `UseOwin()`
1. Update `Startup.cs` to inherit from `Microsoft.AspNetCore.Hosting.StartupBase`, or to a convention-based `Startup`
1. Convert project to .NET Core Console Application with standard ASP.NET Core `Program.cs`

We'll see how close we can get…

## Unravel.Startup

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

## Unravel.AspNet.Mvc

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

### `MvcOptions`

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

### `IFormFile` Model Binder

`AddAspNetMvc()` registers a model binder for `IFormFile`, to replace references to
[`System.Web.HttpPostedFileBase`](https://docs.microsoft.com/en-us/dotnet/api/system.web.httppostedfilebase?view=netframework-4.8).

Some `IFormFile` properties are not supported, due to `HttpPostedFileBase` limitations.

## Unravel.AspNet.WebApi

Similar to `AddAspNetMvc()` described above, there's also an `AddAspNetWebApi()` extension method on `IServiceCollection` that registers a `System.Web.Http.Dependencies.IDependencyResolver`.

The resulting `IAspNetWebApiBuilder` is similar to [`IMvcBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.imvcbuilder?view=aspnetcore-2.1), providing an extension point for additional configuration:

- [`AddControllersAsServices()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.mvccoremvcbuilderextensions.addcontrollersasservices?view=aspnetcore-2.1)

```csharp
public override void ConfigureServices(IServiceCollection services)
{
    services.AddAspNetWebApi()
        .AddControllersAsServices();
}
```

## Unravel.AspNet.Identity

Unravel provides `AddIdentity()` extension methods on `IServiceCollection`. The resulting `IdentityBuilder` provides methods and an extension point to configure **Microsoft.AspNet.Identity** services:

- `SetPerOwinContext<T>()` replaces [`CreatePerOwinContext()`](https://docs.microsoft.com/en-us/previous-versions/aspnet/dn497608(v=vs.108))
- `AddUserManager<T>()`, `AddRoleManager<T>()`, `AddSignInManager<T>()`
  - Calls `SetPerOwinContext<T>()`
  - Extension method overloads align with `CreatePerOwinContext()` signatures
- `AddEntityFrameworkStores<TContext>()`, provided in **Unravel.AspNet.Identity.EntityFramework**, registers a context inheriting from `IdentityDbContext<>`, plus dependent store implementations
  - Calls `SetPerOwinContext<T>()`
- `Microsoft.Owin.Security.DataProtection.IDataProtectionProvider` is registered automatically from `IAppBuilder.GetDataProtectionProvider()`

This is designed to directly migrate from the scaffolded `Startup.Auth.cs` pattern:

```csharp
public void ConfigureAuth(IAppBuilder app)
{
    // Configure the db context, user manager and signin manager to use a single instance per request
    app.CreatePerOwinContext(ApplicationDbContext.Create);
    app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
    app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
    // ...
}
```

To:

```csharp
public override void ConfigureServices(IServiceCollection services)
{
    services.AddIdentity<ApplicationUser>()
        .AddEntityFrameworkStores(ApplicationDbContext.Create)
        .AddUserManager<ApplicationUserManager>(ApplicationUserManager.Create)
        .AddSignInManager<ApplicationSignInManager>(ApplicationSignInManager.Create)
}
```

You can then refactor away from the `Create` methods to constructor injection, e.g.

```diff
--- a/App_Start/IdentityConfig.cs
+++ b/App_Start/IdentityConfig.cs
@@ -12,2 +12,3 @@
 using Microsoft.Owin.Security;
+using Microsoft.Owin.Security.DataProtection;
 using UnravelExamples.Identity.Models;
@@ -37,10 +38,10 @@
     {
-        public ApplicationUserManager(IUserStore<ApplicationUser> store)
+        public ApplicationUserManager(IUserStore<ApplicationUser> store, IDataProtectionProvider dataProtectionProvider)
             : base(store)
         {
+            Init(this, dataProtectionProvider);
         }

-        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
+        private static ApplicationUserManager Init(ApplicationUserManager manager, IDataProtectionProvider dataProtectionProvider)
         {
-            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
             // Configure validation logic for usernames
@@ -80,3 +81,2 @@
             manager.SmsService = new SmsService();
-            var dataProtectionProvider = options.DataProtectionProvider;
             if (dataProtectionProvider != null)
--- a/App_Start/Startup.Auth.cs
+++ b/App_Start/Startup.Auth.cs
@@ -20,5 +20,5 @@
             services.AddIdentity<ApplicationUser>()
-                .AddEntityFrameworkStores(ApplicationDbContext.Create)
-                .AddUserManager<ApplicationUserManager>(ApplicationUserManager.Create)
-                .AddSignInManager<ApplicationSignInManager>(ApplicationSignInManager.Create)
+                .AddEntityFrameworkStores<ApplicationDbContext>()
+                .AddUserManager<ApplicationUserManager>()
+                .AddSignInManager<ApplicationSignInManager>()
                 ;
```

## Credits

- `Startup` concept from [Arex388.AspNet.Mvc.Startup](https://github.com/arex388/Arex388.AspNet.Mvc.Startup)
- [David Fowler's Gist](https://gist.github.com/davidfowl/563a602936426a18f67cd77088574e61)
- [Scott Dorman's Blog](https://scottdorman.blog/2016/03/17/integrating-asp-net-core-dependency-injection-in-mvc-4/)
- The [StackOverflow question](https://stackoverflow.com/questions/43311099/how-to-create-dependency-injection-for-asp-net-mvc-5) that lead @arex388 to David and Scott.
