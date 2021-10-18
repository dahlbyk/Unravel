using System;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Unravel.AspNet.Identity.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
    /// </summary>
    public static class UnravelIdentityServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default identity system configuration for the specified User and Role types.
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <typeparam name="TRole">The type representing a Role in the system.</typeparam>
        /// <typeparam name="TKey">The key type for Users and Roles in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
        public static IdentityBuilder<TUser, TRole, TKey> AddIdentity<TUser, TRole, TKey>(this IServiceCollection services)
            where TUser : class, IUser<TKey>
            where TRole : class, IRole<TKey>
            where TKey : IEquatable<TKey>
        {
            // Identity services
            services.TryAddScoped<UserManager<TUser, TKey>, UserManager<TUser, TKey>>();
            services.TryAddScoped<RoleManager<TRole, TKey>, RoleManager<TRole, TKey>>();

            return new IdentityBuilder<TUser, TRole, TKey>(services);
        }
    }
}
