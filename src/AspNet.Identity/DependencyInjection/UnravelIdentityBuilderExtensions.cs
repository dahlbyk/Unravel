using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Unravel.AspNet.Identity.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IdentityBuilder{TUser, TRole, TKey}"/> for configuring identity services.
    /// </summary>
    public static class UnravelIdentityBuilderExtensions
    {
        public static IdentityBuilder<TUser, TRole, TKey> AddUserManager<TUser, TRole, TKey, TUserManager>(
            this IdentityBuilder<TUser, TRole, TKey> builder,
            Func<IdentityFactoryOptions<TUserManager>, IOwinContext, TUserManager> createCallback)
            where TUser : class, IUser<TKey>
            where TRole : class, IRole<TKey>
            where TKey : IEquatable<TKey>
            where TUserManager : UserManager<TUser, TKey>
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var userManagerType = typeof(UserManager<,>).MakeGenericType(typeof(TUser), typeof(TKey));
            var customType = typeof(TUserManager);
            if (userManagerType != customType)
            {
                builder.Services.AddScoped(customType, services => services.GetRequiredService(userManagerType));
            }
            builder.Services.AddScoped(userManagerType, p =>
            {
                var options = CreateOptions<TUserManager>(p);
                return createCallback(options, p.GetRequiredService<IOwinContext>());
            });

            return builder;
        }

        public static IdentityBuilder<TUser, TRole, TKey> AddSignInManager<TUser, TRole, TKey, TSignInManager>(
            this IdentityBuilder<TUser, TRole, TKey> builder,
            Func<IdentityFactoryOptions<TSignInManager>, IOwinContext, TSignInManager> createCallback)
            where TUser : class, IUser<TKey>
            where TRole : class, IRole<TKey>
            where TKey : IEquatable<TKey>
            where TSignInManager : SignInManager<TUser, TKey>
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var userManagerType = typeof(SignInManager<,>).MakeGenericType(typeof(TUser), typeof(TKey));
            var customType = typeof(TSignInManager);
            if (userManagerType != customType)
            {
                builder.Services.AddScoped(customType, services => services.GetRequiredService(userManagerType));
            }
            builder.Services.AddScoped(userManagerType, p =>
            {
                var options = CreateOptions<TSignInManager>(p);
                return createCallback(options, p.GetRequiredService<IOwinContext>());
            });

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
