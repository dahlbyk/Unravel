using System;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Unravel.AspNet.Identity.DependencyInjection
{
    /// <summary>
    /// Helper functions for configuring identity services.
    /// </summary>
    public class IdentityBuilder<TUser, TRole, TKey>
        where TUser : class, IUser<TKey>
        where TRole : class, IRole<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Creates a new instance of <see cref="IdentityBuilder"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to attach to.</param>
        public IdentityBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> services are attached to.
        /// </summary>
        /// <value>
        /// The <see cref="IServiceCollection"/> services are attached to.
        /// </value>
        public IServiceCollection Services { get; private set; }

        private IdentityBuilder<TUser, TRole, TKey> AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="IUserStore{TUser}"/>.
        /// </summary>
        /// <typeparam name="TStore">The user store type.</typeparam>
        /// <returns>The current instance.</returns>
        public virtual IdentityBuilder<TUser, TRole, TKey> AddUserStore<TStore>() where TStore : class, IUserStore<TUser, TKey>
            => AddScoped(typeof(IUserStore<,>).MakeGenericType(typeof(TUser), typeof(TKey)), typeof(TStore));

        /// <summary>
        /// Adds a <see cref="UserManager{TUser}"/>.
        /// </summary>
        /// <typeparam name="TUserManager">The type of the user manager to add.</typeparam>
        /// <returns>The current instance.</returns>
        public virtual IdentityBuilder<TUser, TRole, TKey> AddUserManager<TUserManager>() where TUserManager : UserManager<TUser, TKey>
        {
            var managerType = typeof(UserManager<,>).MakeGenericType(typeof(TUser), typeof(TKey));
            var customType = typeof(TUserManager);
            if (managerType != customType)
            {
                Services.AddScoped(customType, services => services.GetRequiredService(managerType));
            }
            return AddScoped(managerType, customType);
        }

        /// <summary>
        /// Adds a <see cref="IRoleStore{TRole}"/>.
        /// </summary>
        /// <typeparam name="TStore">The role store.</typeparam>
        /// <returns>The current instance.</returns>
        public virtual IdentityBuilder<TUser, TRole, TKey> AddRoleStore<TStore>() where TStore : class, IRoleStore<TRole, TKey>
        {
            return AddScoped(typeof(IRoleStore<,>).MakeGenericType(typeof(TRole), typeof(TKey)), typeof(TStore));
        }

        /// <summary>
        /// Adds a <see cref="RoleManager{TRole}"/>.
        /// </summary>
        /// <typeparam name="TRoleManager">The type of the role manager to add.</typeparam>
        /// <returns>The current instance.</returns>
        public virtual IdentityBuilder<TUser, TRole, TKey> AddRoleManager<TRoleManager>() where TRoleManager : RoleManager<TRole, TKey>
        {
            var managerType = typeof(RoleManager<,>).MakeGenericType(typeof(TRole), typeof(TKey));
            var customType = typeof(TRoleManager);
            if (managerType != customType)
            {
                Services.AddScoped(customType, services => services.GetRequiredService(managerType));
            }
            return AddScoped(managerType, customType);
        }
    }
}
