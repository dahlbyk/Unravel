using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Unravel.AspNet.Identity.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IdentityBuilder"/> for configuring identity services.
    /// </summary>
    public static class UnravelIdentityBuilderExtensions
    {
        public static IdentityBuilder AddUserManager<TUserManager>(
            this IdentityBuilder builder,
            Func<IdentityFactoryOptions<TUserManager>, IOwinContext, TUserManager> createCallback)
            where TUserManager : IDisposable
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var managerType = typeof(UserManager<,>).MakeGenericType(builder.UserType, builder.KeyType);
            var customType = typeof(TUserManager);
            if (managerType != customType)
            {
                builder.Services.AddScoped(customType, services => services.GetRequiredService(managerType));
            }
            builder.Services.AddScoped(managerType, p =>
            {
                var options = CreateOptions<TUserManager>(p);
                return createCallback(options, p.GetRequiredService<IOwinContext>());
            });

            builder.SetPerOwinContext<TUserManager>();

            return builder;
        }

        public static IdentityBuilder AddRoleManager<TRoleManager>(
            this IdentityBuilder builder,
            Func<IdentityFactoryOptions<TRoleManager>, IOwinContext, TRoleManager> createCallback)
            where TRoleManager : IDisposable
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var managerType = typeof(RoleManager<,>).MakeGenericType(builder.RoleType, builder.KeyType);
            var customType = typeof(TRoleManager);
            if (managerType != customType)
            {
                builder.Services.AddScoped(customType, services => services.GetRequiredService(managerType));
            }
            builder.Services.AddScoped(managerType, p =>
            {
                var options = CreateOptions<TRoleManager>(p);
                return createCallback(options, p.GetRequiredService<IOwinContext>());
            });

            return builder;
        }

        /// <summary>
        /// Adds a <see cref="SignInManager{TUser}"/> for the <seealso cref="UserType"/>.
        /// </summary>
        /// <typeparam name="TSignInManager">The type of the sign in manager manager to add.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder"/>.</param>
        /// <param name="createCallback">The callback to create a <typeparamref name="TSignInManager"/>.</param>
        /// <returns>The <see cref="IdentityBuilder"/>.</returns>
        public static IdentityBuilder AddSignInManager<TSignInManager>(
            this IdentityBuilder builder,
            Func<IdentityFactoryOptions<TSignInManager>, IOwinContext, TSignInManager> createCallback)
            where TSignInManager : IDisposable
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var managerType = typeof(SignInManager<,>).MakeGenericType(builder.UserType, builder.KeyType);
            var customType = typeof(TSignInManager);
            if (managerType != customType)
            {
                builder.Services.AddScoped(customType, services => services.GetRequiredService(managerType));
            }
            builder.Services.AddScoped(managerType, p =>
            {
                var options = CreateOptions<TSignInManager>(p);
                return createCallback(options, p.GetRequiredService<IOwinContext>());
            });

            builder.SetPerOwinContext<TSignInManager>();

            return builder;
        }

        private static IdentityFactoryOptions<T> CreateOptions<T>(IServiceProvider p) where T : IDisposable
        {
            return new IdentityFactoryOptions<T>
            {
                DataProtectionProvider = p.GetService<IDataProtectionProvider>(),
                Provider = NullIdentityFactoryProvider<T>.Instance,
            };
        }

        private class NullIdentityFactoryProvider<T> : IIdentityFactoryProvider<T> where T : IDisposable
        {
            public static IIdentityFactoryProvider<T> Instance { get; } = new NullIdentityFactoryProvider<T>();
            T IIdentityFactoryProvider<T>.Create(IdentityFactoryOptions<T> options, IOwinContext context) => throw new InvalidOperationException();
            void IIdentityFactoryProvider<T>.Dispose(IdentityFactoryOptions<T> options, T instance) { }
        }
    }
}
