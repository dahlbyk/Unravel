# Unravel.AspNet.Identity.EntityFramework

Unravel provides `AddIdentity()` extension methods on `IServiceCollection`. The resulting `IdentityBuilder` provides methods and an extension point to configure **Microsoft.AspNet.Identity** services:

- `AddEntityFrameworkStores<TContext>()` registers a context inheriting from `IdentityDbContext<>`, plus dependent store implementations
  - Calls `SetPerOwinContext<T>()`, from **Unravel.AspNet.Identity**, replacing `CreatePerOwinContext<T>()`

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

