using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Extensions.DependencyInjection;

namespace Unravel.AspNet.Identity.DependencyInjection
{
    /// <summary>
    /// Helper functions for configuring identity services.
    /// </summary>
    public class IdentityBuilder
    {
        /// <summary>
        /// Creates a new instance of <see cref="IdentityBuilder"/>.
        /// </summary>
        /// <param name="user">The <see cref="Type"/> to use for the users.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to attach to.</param>
        public IdentityBuilder(Type user, IServiceCollection services)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            Services = services ?? throw new ArgumentNullException(nameof(services));

            var userInterface = user.GetTypeInfo().ImplementedInterfaces
                .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IUser<>));

            if (userInterface == null)
                throw new ArgumentException($"User type ({user.Name}) must implement IUser<>", nameof(user));

            KeyType = userInterface.GenericTypeArguments[0];
            UserType = user;
        }

        /// <summary>
        /// Creates a new instance of <see cref="IdentityBuilder"/>.
        /// </summary>
        /// <param name="user">The <see cref="Type"/> to use for the users.</param>
        /// <param name="role">The <see cref="Type"/> to use for the roles.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to attach to.</param>
        public IdentityBuilder(Type user, Type role, IServiceCollection services) : this(user, services)
        {
            if (role == null) throw new ArgumentNullException(nameof(user));

            if (!typeof(IRole<>).MakeGenericType(KeyType).IsAssignableFrom(role))
                throw new ArgumentException($"Role type ({role.Name}) must implement IRole<>", nameof(role));

            RoleType = role;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> used for user/role keys.
        /// </summary>
        public Type KeyType { get; }

        /// <summary>
        /// Gets the <see cref="Type"/> used for users.
        /// </summary>
        public Type UserType { get; }


        /// <summary>
        /// Gets the <see cref="Type"/> used for roles.
        /// </summary>
        public Type RoleType { get; }

        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> services are attached to.
        /// </summary>
        /// <value>
        /// The <see cref="IServiceCollection"/> services are attached to.
        /// </value>
        public IServiceCollection Services { get; private set; }

        private IdentityBuilder AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="IUserStore{TUser}"/> for the <seealso cref="UserType"/>.
        /// </summary>
        /// <typeparam name="TStore">The user store type.</typeparam>
        /// <returns>The current instance.</returns>
        public virtual IdentityBuilder AddUserStore<TStore>() where TStore : class
            => AddScoped(typeof(IUserStore<,>).MakeGenericType(UserType, KeyType), typeof(TStore));

        /// <summary>
        /// Adds a <see cref="UserManager{TUser}"/> for the <seealso cref="UserType"/>.
        /// </summary>
        /// <typeparam name="TUserManager">The type of the user manager to add.</typeparam>
        /// <returns>The current instance.</returns>
        public virtual IdentityBuilder AddUserManager<TUserManager>()
        {
            var managerType = typeof(UserManager<,>).MakeGenericType(UserType, KeyType);
            var customType = typeof(TUserManager);
            if (managerType != customType)
            {
                Services.AddScoped(customType, services => services.GetRequiredService(managerType));
            }
            return AddScoped(managerType, customType);
        }

        /// <summary>
        /// Adds a <see cref="IRoleStore{TRole}"/> for the <seealso cref="RoleType"/>.
        /// </summary>
        /// <typeparam name="TStore">The role store.</typeparam>
        /// <returns>The current instance.</returns>
        public virtual IdentityBuilder AddRoleStore<TStore>() where TStore : class
        {
            return AddScoped(typeof(IRoleStore<,>).MakeGenericType(RoleType, KeyType), typeof(TStore));
        }

        /// <summary>
        /// Adds a <see cref="RoleManager{TRole}"/> for the <seealso cref="RoleType"/>.
        /// </summary>
        /// <typeparam name="TRoleManager">The type of the role manager to add.</typeparam>
        /// <returns>The current instance.</returns>
        public virtual IdentityBuilder AddRoleManager<TRoleManager>()
        {
            var managerType = typeof(RoleManager<,>).MakeGenericType(RoleType, KeyType);
            var customType = typeof(TRoleManager);
            if (managerType != customType)
            {
                Services.AddScoped(customType, services => services.GetRequiredService(managerType));
            }
            return AddScoped(managerType, customType);
        }

        /// <summary>
        /// Adds a <see cref="SignInManager{TUser, TKey}"/> for the <seealso cref="UserType"/>.
        /// </summary>
        /// <typeparam name="TSignInManager">The type of the sign in manager manager to add.</typeparam>
        /// <returns>The current instance.</returns>
        public virtual IdentityBuilder AddSignInManager<TSignInManager>()
        {
            var managerType = typeof(SignInManager<,>).MakeGenericType(UserType, KeyType);
            var customType = typeof(TSignInManager);
            if (managerType != customType)
            {
                Services.AddScoped(customType, services => services.GetRequiredService(managerType));
            }
            return AddScoped(managerType, customType);
        }
    }
}
