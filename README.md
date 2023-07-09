# Unravel

Unravel your dependence on `System.Web`.

## Goals

Unravel's goal is to facilitate in-place upgrade from ASP.NET MVC and Web API on .NET Framework to ASP.NET Core 2.1 (the last version [supported on .NET Framework](https://dotnet.microsoft.com/platform/support/policy/aspnet#dotnet-core)). Long-lived upgrade branches are bad.

The ultimate goal is that an upgrade to ASP.NET Core could be reduced to roughly:
1. Remove `Global.asax`, `Web.config`, and other lingering ASP.NET artifacts
1. Remove `<ProjectTypeGuids>` from `.csproj`
1. Migrate `.csproj` to [SDK format](https://docs.microsoft.com/en-us/dotnet/core/project-sdk/overview) with [Project2015To2017.Migrate2019.Tool](https://github.com/hvanbakel/CsprojToVs2017)
1. Convert remaining references to OWIN `IAppBuilder` to `UseOwin()`
1. Update `Startup.cs` to inherit from `Microsoft.AspNetCore.Hosting.StartupBase`, or to a convention-based `Startup`
1. Convert project to .NET Core Console Application with standard ASP.NET Core `Program.cs`

We'll see how close we can get…

## Getting Started

1. [Set up Unravel.Startup](src/Startup/README.md#setup)
1. Make sure everything still works!
1. Configure additional [packages](#packages) based on which parts of ASP.NET are in use.
1. Implement `ConfigureServices()` with additional services and configuration.
1. Begin migration to dependency injection, modern configuration, modern logging, etc.
1. **Open issues with your pain points.**
1. Begin migration from ASP.NET to [ASP.NET Core](#aspnet-core).
1. **Open more issues with your pain points.**

## Packages

Unravel provides granular packages to avoid extra dependencies.

### ASP.NET

- [Unravel.Startup](src/Startup/README.md) provides a fully configurable ASP.NET Core `IWebHost` on top of `System.Web` via OWIN, with forward-compatible dependency injection, configuration and logging.
- [Unravel.AspNet.Mvc](src/AspNet.Mvc/README.md) provides an `AddAspNetMvc()` extension method on `IServiceCollection` that registers a `System.Web.Mvc.IDependencyResolver` and allows additional configuration.
- [Unravel.AspNet.WebApi](src/AspNet.WebApi/README.md) provides an `AddAspNetWebApi()` extension method on `IServiceCollection` that registers a `System.Web.Http.Dependencies.IDependencyResolver` and allows additional configuration.

### ASP.NET Identity

**Microsoft.AspNet.Identity** has its own approach to dependency injection which needs to be adapted to work with `IServiceCollection`.

- [Unravel.AspNet.Identity](src/AspNet.Identity/README.md) provides `AddIdentity()` extension methods on `IServiceCollection` that allow additional configuration.
- [Unravel.AspNet.Identity.EntityFramework](src/AspNet.Identity.EntityFramework/README.md) allows configuring **Microsoft.AspNet.Identity.EntityFramework**.

### ASP.NET Core

Unravel will _try_ to enable as much of ASP.NET Core as possible. (Experimental!)

- [Unravel.AspNetCore.Mvc](src/AspNetCore.Mvc/README.md) provides `IgnoreControllersOfType<T>()` extension methods to prevent ASP.NET Core from trying to invoke legacy ASP.NET controllers.
- [Unravel.AspNetCore.Mvc.ViewFeatures](src/AspNetCore.Mvc.ViewFeatures/README.md) provides `AddAspNetMvcViewEngines()` to compile ASP.NET Core views with the ASP.NET compiler.

## Credits

- `Startup` concept from [Arex388.AspNet.Mvc.Startup](https://github.com/arex388/Arex388.AspNet.Mvc.Startup)
- [David Fowler's Gist](https://gist.github.com/davidfowl/563a602936426a18f67cd77088574e61)
- [Scott Dorman's Blog](https://scottdorman.blog/2016/03/17/integrating-asp-net-core-dependency-injection-in-mvc-4/)
- The [StackOverflow question](https://stackoverflow.com/questions/43311099/how-to-create-dependency-injection-for-asp-net-mvc-5) that lead @arex388 to David and Scott.
