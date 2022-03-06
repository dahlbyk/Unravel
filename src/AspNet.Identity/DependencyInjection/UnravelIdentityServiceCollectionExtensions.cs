using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Owin.Security.DataProtection;
using Owin;
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
        public static IdentityBuilder AddIdentity<TUser, TRole, TKey>(this IServiceCollection services)
            where TUser : class, IUser<TKey>
            where TRole : class, IRole<TKey>
            where TKey : IEquatable<TKey>
        {
            // Used by default UserManager
            services.AddSingleton(sp => sp.GetRequiredService<IAppBuilder>().GetDataProtectionProvider());

            // Identity services
            if (typeof(TKey) == typeof(string))
            {
                services.TryAddScoped(typeof(UserManager<>).MakeGenericType(typeof(TUser)));
                services.TryAddScoped(typeof(RoleManager<>).MakeGenericType(typeof(TRole)));

                services.TryAddScoped(typeof(UserManager<,>).MakeGenericType(typeof(TUser), typeof(TKey)), s => s.GetRequiredService<UserManager<TUser, TKey>>());
                services.TryAddScoped(typeof(RoleManager<,>).MakeGenericType(typeof(TRole), typeof(TKey)), s => s.GetRequiredService<RoleManager<TRole, TKey>>());
            }
            else
            {
                services.TryAddScoped<UserManager<TUser, TKey>>();
                services.TryAddScoped<RoleManager<TRole, TKey>>();
            }

            services.TryAddScoped<SignInManager<TUser, TKey>>();

            return new IdentityBuilder(typeof(TUser), typeof(TRole), services);
        }
    }
}
