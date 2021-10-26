using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Unravel.AspNet.Identity.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services with Entity Framework.
    /// </summary>
    public static class UnravelIdentityEntityFrameworkServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default identity system configuration for the specified User type,
        /// with <see cref="IdentityRole"/> as the Role type.
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
        public static IdentityBuilder AddIdentity<TUser>(this IServiceCollection services)
            where TUser : class, IUser<string>
        {
            return services.AddIdentity<TUser, IdentityRole, string>();
        }
    }
}
