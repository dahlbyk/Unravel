# Unravel

Unravel your dependence on `System.Web`.

## Goals

Unravel's goal is to facilitate in-place upgrade from ASP.NET MVC and Web API on .NET Framework to ASP.NET Core 2.3 (the version [supported on .NET Framework](https://dotnet.microsoft.com/platform/support/policy/aspnet#dotnet-core)). Long-lived upgrade branches are bad.

> [!WARNING]
> ASP.NET Core 2.3 [reaches end of support on April 13, 2027](https://devblogs.microsoft.com/dotnet/aspnet-core-2-3-end-of-support/), after which it won't be supported on any version of .NET Framework. Microsoft no longer recommends it as a migration strategy.

### Why not a proxy?

Microsoft's [recommended incremental migration](https://learn.microsoft.com/aspnet/core/migration/fx-to-core/) puts a new ASP.NET Core app in front of the old one. The new app forwards unmigrated routes to the old one through [YARP](https://microsoft.github.io/reverse-proxy/), and the two apps share session and authentication through System.Web adapters. It runs on supported versions of .NET, and it suits a long, gradual migration.

Unravel takes the other trade-off. It runs ASP.NET Core inside your existing app:

- **One app.** There is one process, one deployment, and one set of URLs and cookies. You don't need a proxy hop, a second site to host, or remote session and auth plumbing.
- **Shared everything.** Migrated and legacy code use the same dependency injection container, configuration, logging, and data access, in process.
- **Migrate code, not routes.** Services, filters, and controllers move one at a time. ASP.NET Core controllers can render existing MVC 5 views with [`AddAspNetMvcViewEngines()`](src/AspNetCore.Mvc.ViewFeatures/README.md).
- **No rearranging.** Files can stay where they are. An SDK-style project can compile them for both `net48` and modern .NET, so API differences show up as compile errors while you migrate instead of at cutover.

The cost is ASP.NET Core 2.3. Most of what [changed after 2.x](https://learn.microsoft.com/aspnet/core/migration/22-to-30) is in hosting and startup (Generic Host, endpoint routing), which you rewrite once at cutover. Controllers and views are largely compatible, but some behavior compiles unchanged and then differs at runtime:

- **Link generation:** endpoint routing stops reusing ambient route values when linking to a different action. This affects `Url.Action` and `asp-action` links, especially with areas.
- **JSON:** the default serializer changed to System.Text.Json. `AddNewtonsoftJson()` restores the old behavior.
- **Synchronous I/O** (such as `HttpRequest.Body.Read` or `Stream.Flush`) is [disallowed by default](https://learn.microsoft.com/aspnet/core/migration/22-to-30#allowsynchronousio-disabled). To allow it, set `AllowSynchronousIO` on the server's options (for example, `KestrelServerOptions` or `IISServerOptions`), or set it for a single request with `IHttpBodyControlFeature`.
- **`Async` suffix:** it's trimmed from action names, which changes routes for actions like `IndexAsync`.
- **Nullable reference types** imply `[Required]` (6.0+), but only once you enable nullable.

ASP.NET Core 2.3 is also unsupported after April 13, 2027, so Unravel suits a migration you can sprint through, where running past end of support is a short, accepted risk rather than a long-term plan.

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
