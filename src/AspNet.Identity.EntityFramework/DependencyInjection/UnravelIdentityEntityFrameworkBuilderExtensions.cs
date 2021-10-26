using System;
using System.Data.Entity;
using System.Reflection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Unravel.AspNet.Identity.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IdentityBuilder"/> for adding entity framework stores.
    /// </summary>
    public static class UnravelIdentityEntityFrameworkBuilderExtensions
    {
        /// <summary>
        /// Adds an Entity Framework implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TContext">The Entity Framework database context to use.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddEntityFrameworkStores<TContext>(this IdentityBuilder builder)
            where TContext : DbContext
        {
            builder.Services.TryAddScoped<TContext>();

            AddStores<TContext>(builder);
            return builder;
        }

        /// <summary>
        /// Adds an Entity Framework implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TContext">The Entity Framework database context to use.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
        /// <param name="createCallback">Invoked to create an instance of <typeparamref name="TContext"/>.</param></param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddEntityFrameworkStores<TContext>(this IdentityBuilder builder, Func<TContext> createCallback)
            where TContext : DbContext
        {
            builder.Services.TryAddScoped(s => createCallback());

            AddStores<TContext>(builder);
            return builder;
        }

        private static void AddStores<TContext>(IdentityBuilder builder)
        {
            // Insert at 0 to ensure Set before services that depend on it
            builder.SetPerOwinContext<TContext>(0);

            var contextType = typeof(TContext);
            var services = builder.Services;
            var userType = builder.UserType;
            var roleType = builder.RoleType;
            var keyType = builder.KeyType;

            var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<,,,>));
            if (identityUserType == null)
                throw new InvalidOperationException($"{nameof(AddEntityFrameworkStores)} can only be called with a user that derives from IdentityUser<,,,>.");

            var userLoginType = identityUserType.GenericTypeArguments[1];
            var userRoleType = identityUserType.GenericTypeArguments[2];
            var userClaimType = identityUserType.GenericTypeArguments[3];

            var identityRoleType = FindGenericBaseType(roleType, typeof(IdentityRole<,>));
            if (identityRoleType == null)
                throw new InvalidOperationException($"{nameof(AddEntityFrameworkStores)} can only be called with a role that derives from IdentityRole<,>.");

            if (userRoleType != identityRoleType.GenericTypeArguments[1])
                throw new InvalidOperationException($"IdentityUser<,,TRole,> must match IdentityRole<,TUserRole>.");

            var identityContext = FindGenericBaseType(contextType, typeof(IdentityDbContext<,,,,,>));
            if (identityContext != null)
            {
                if (identityContext.GenericTypeArguments[0] != userType)
                    throw new InvalidOperationException("IdentityDbContext<TUser,,,,,> must match user type.");

                if (identityContext.GenericTypeArguments[1] != roleType)
                    throw new InvalidOperationException("IdentityDbContext<,TRole,,,,> must match role type.");

                if (identityContext.GenericTypeArguments[2] != keyType)
                    throw new InvalidOperationException("IdentityDbContext<,,TKey,,,> must match IdentityUser<TKey,,,> and IdentityRole<TKey,>.");

                if (identityContext.GenericTypeArguments[3] != userLoginType)
                    throw new InvalidOperationException("IdentityDbContext<,,,TUserLogin,,> must match IdentityUser<,TLogin,,>.");

                if (identityContext.GenericTypeArguments[4] != userRoleType)
                    throw new InvalidOperationException("IdentityDbContext<,,,,TUserRole,> must match IdentityUser<,,TRole,> and IdentityRole<,TUserRole>.");

                if (identityContext.GenericTypeArguments[5] != userClaimType)
                    throw new InvalidOperationException("IdentityDbContext<,,,,,TUserClaim> must match IdentityUser<,,,TClaim>.");

                services.TryAddScoped(identityContext, s => s.GetRequiredService(contextType));

                var userIdentityContext = FindGenericBaseType(contextType, typeof(IdentityDbContext<>));
                if (userIdentityContext != null)
                {
                    services.TryAddScoped(userIdentityContext, s => s.GetRequiredService(contextType));
                }
                else if (typeof(IdentityDbContext).IsAssignableFrom(contextType))
                {
                    services.TryAddScoped(typeof(IdentityDbContext), s => s.GetRequiredService(contextType));
                }
            }

            var baseUserStoreType = typeof(UserStore<,,,,,>).MakeGenericType(userType, roleType, keyType, userLoginType, userRoleType, userClaimType);
            var baseRoleStoreType = typeof(RoleStore<,,>).MakeGenericType(roleType, keyType, userRoleType);

            var userStoreType = baseUserStoreType;
            var roleStoreType = baseRoleStoreType;

            if (keyType == typeof(string))
            {
                if (typeof(IdentityUser).IsAssignableFrom(userType)
                    && roleType == typeof(IdentityRole)
                    && userLoginType == typeof(IdentityUserLogin)
                    && userRoleType == typeof(IdentityUserRole)
                    && userClaimType == typeof(IdentityUserClaim))
                {
                    userStoreType = typeof(UserStore<>).MakeGenericType(userType);

                    services.TryAddScoped(baseUserStoreType, s => s.GetRequiredService(userStoreType));
                    services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), s => s.GetRequiredService(userStoreType));
                }

                if (typeof(IdentityRole).IsAssignableFrom(roleType)
                    && userRoleType == typeof(IdentityUserRole))
                {
                    roleStoreType = typeof(RoleStore<>).MakeGenericType(roleType);

                    services.TryAddScoped(baseRoleStoreType, s => s.GetRequiredService(roleStoreType));
                    services.TryAddScoped(typeof(IQueryableRoleStore<>).MakeGenericType(roleType), s => s.GetRequiredService(roleStoreType));
                    services.TryAddScoped(typeof(IRoleStore<>).MakeGenericType(roleType), s => s.GetRequiredService(roleStoreType));
                }
            }

            services.TryAddScoped(userStoreType, s => Activator.CreateInstance(userStoreType, s.GetRequiredService(identityContext)));
            services.TryAddScoped(typeof(IUserLoginStore<,>).MakeGenericType(userType, keyType), s => s.GetRequiredService(userStoreType));
            services.TryAddScoped(typeof(IUserStore<,>).MakeGenericType(userType, keyType), s => s.GetRequiredService(userStoreType));
            services.TryAddScoped(typeof(IUserClaimStore<,>).MakeGenericType(userType, keyType), s => s.GetRequiredService(userStoreType));
            services.TryAddScoped(typeof(IUserRoleStore<,>).MakeGenericType(userType, keyType), s => s.GetRequiredService(userStoreType));
            services.TryAddScoped(typeof(IUserPasswordStore<,>).MakeGenericType(userType, keyType), s => s.GetRequiredService(userStoreType));
            services.TryAddScoped(typeof(IUserSecurityStampStore<,>).MakeGenericType(userType, keyType), s => s.GetRequiredService(userStoreType));
            services.TryAddScoped(typeof(IQueryableUserStore<,>).MakeGenericType(userType, keyType), s => s.GetRequiredService(userStoreType));
            services.TryAddScoped(typeof(IUserEmailStore<,>).MakeGenericType(userType, keyType), s => s.GetRequiredService(userStoreType));
            services.TryAddScoped(typeof(IUserPhoneNumberStore<,>).MakeGenericType(userType, keyType), s => s.GetRequiredService(userStoreType));
            services.TryAddScoped(typeof(IUserTwoFactorStore<,>).MakeGenericType(userType, keyType), s => s.GetRequiredService(userStoreType));
            services.TryAddScoped(typeof(IUserLockoutStore<,>).MakeGenericType(userType, keyType), s => s.GetRequiredService(userStoreType));

            services.TryAddScoped(roleStoreType, s => Activator.CreateInstance(roleStoreType, s.GetRequiredService(identityContext)));
            services.TryAddScoped(typeof(IQueryableRoleStore<,>).MakeGenericType(roleType, keyType), s => s.GetRequiredService(roleStoreType));
            services.TryAddScoped(typeof(IRoleStore<,>).MakeGenericType(roleType, keyType), s => s.GetRequiredService(roleStoreType));
        }

        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}
