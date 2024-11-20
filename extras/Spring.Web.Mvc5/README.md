# Unravel.Spring.Web.Mvc5

Unravel provides a `SpringMvcApplication` to support dependency injection with [Spring](https://springframework.net/).

- `Unravel.Spring.Web.Mvc.SpringMvcApplication`
  - Inherits from `Unravel.Application`
  - Provides `virtual` methods from `Spring.Web.SpringMvcApplication` for compatibility:
    - `ConfigureApplicationContext()`
    - `BuildDependencyResolver()`
    - `BuildApiDependencyResolver()`

Need `SpringControllerFactory` and `SpringActionInvoker`? **Open an issue.**

## Setup

1. Install `Unravel.Spring.Web.Mvc5` in your Web Application project

1. Continue with setup for [Unravel.Startup](https://www.nuget.org/packages/Unravel.Startup),
   except inherit from `Unravel.Spring.Web.Mvc.SpringMvcApplication`

    ```diff
    -public class MvcApplication : SpringMvcApplication
    +public partial class Startup : Unravel.Spring.Web.Mvc.SpringMvcApplication
    ```
