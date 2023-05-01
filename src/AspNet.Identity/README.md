# Unravel.AspNet.Identity

Unravel provides `AddIdentity()` extension methods on `IServiceCollection`. The resulting `IdentityBuilder` provides methods and an extension point to configure **Microsoft.AspNet.Identity** services:

- `SetPerOwinContext<T>()` replaces [`CreatePerOwinContext()`](https://docs.microsoft.com/en-us/previous-versions/aspnet/dn497608(v=vs.108))
- `AddUserManager<T>()`, `AddRoleManager<T>()`, `AddSignInManager<T>()`
  - Calls `SetPerOwinContext<T>()`
  - Extension method overloads align with `CreatePerOwinContext()` signatures
- `Microsoft.Owin.Security.DataProtection.IDataProtectionProvider` is registered automatically from `IAppBuilder.GetDataProtectionProvider()`
