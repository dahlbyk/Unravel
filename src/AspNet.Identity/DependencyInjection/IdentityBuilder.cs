using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;

namespace Unravel.AspNet.Identity.DependencyInjection
{
    /// <summary>
    /// Helper functions for configuring identity services.
    /// </summary>
    public class IdentityBuilder : IStartupFilter
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

            services.AddSingleton<IStartupFilter>(this);

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
        public Type? RoleType { get; }

        /// <summary>
        /// List of callbacks to invoke per OWIN request.
        /// </summary>
        public List<Action<IOwinContext>> OwinContextSetters { get; } = new List<Action<IOwinContext>>();

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
        /// Also calls <see cref="SetPerOwinContext{TService}"/> with <typeparamref name="TUserManager"/>.
        /// </summary>
        /// <typeparam name="TUserManager">The type of the user manager to add.</typeparam>
        /// <returns>The current instance.</returns>
        public virtual IdentityBuilder AddUserManager<TUserManager>()
        {
            SetPerOwinContext<TUserManager>();

            var managerType = typeof(UserManager<,>).MakeGenericType(UserType, KeyType);
            var customType = typeof(TUserManager);
            if (managerType != customType)
            {
                Services.AddScoped(customType, services => services.GetRequiredService(managerType));

                if (KeyType == typeof(string))
                {
                    var userManagerType = typeof(UserManager<>).MakeGenericType(UserType);
                    if (userManagerType.IsAssignableFrom(customType))
                    {
                        Services.AddScoped(userManagerType, s => s.GetRequiredService(managerType));
                    }
                }
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
        /// Also calls <see cref="SetPerOwinContext{TService}"/> with <typeparamref name="TRoleManager"/>.
        /// </summary>
        /// <typeparam name="TRoleManager">The type of the role manager to add.</typeparam>
        /// <returns>The current instance.</returns>
        public virtual IdentityBuilder AddRoleManager<TRoleManager>()
        {
            SetPerOwinContext<TRoleManager>();

            var managerType = typeof(RoleManager<,>).MakeGenericType(RoleType, KeyType);
            var customType = typeof(TRoleManager);
            if (managerType != customType)
            {
                Services.AddScoped(customType, services => services.GetRequiredService(managerType));

                if (KeyType == typeof(string))
                {
                    var roleManagerType = typeof(RoleManager<>).MakeGenericType(RoleType);
                    if (roleManagerType.IsAssignableFrom(customType))
                    {
                        Services.AddScoped(roleManagerType, s => s.GetRequiredService(managerType));
                    }
                }
            }
            return AddScoped(managerType, customType);
        }

        /// <summary>
        /// Adds a <see cref="SignInManager{TUser, TKey}"/> for the <seealso cref="UserType"/>.
        /// Also calls <see cref="SetPerOwinContext{TService}"/> with <typeparamref name="TSignInManager"/>.
        /// </summary>
        /// <typeparam name="TSignInManager">The type of the sign in manager manager to add.</typeparam>
        /// <returns>The current instance.</returns>
        public virtual IdentityBuilder AddSignInManager<TSignInManager>()
        {
            SetPerOwinContext<TSignInManager>();

            var managerType = typeof(SignInManager<,>).MakeGenericType(UserType, KeyType);
            var customType = typeof(TSignInManager);
            if (managerType != customType)
            {
                Services.AddScoped(customType, services => services.GetRequiredService(managerType));
            }
            return AddScoped(managerType, customType);
        }

        /// <summary>
        /// Replacement for <see cref="AppBuilderExtensions.CreatePerOwinContext{T}(IAppBuilder, Func{T})"/>.
        /// Adds a callback to <see cref="OwinContextSetters"/> that calls
        /// OWIN Identity <see cref="OwinContextExtensions.Set{T}(IOwinContext, T)"/>
        /// with the <typeparamref name="TService"/> from request services.
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <returns>The current instance.</returns>
        public IdentityBuilder SetPerOwinContext<TService>()
        {
            OwinContextSetters.Add(SetRequestService<TService>);
            return this;
        }

        /// <summary>
        /// Replacement for <see cref="AppBuilderExtensions.CreatePerOwinContext{T}(IAppBuilder, Func{T})"/>.
        /// Inserts a callback at <paramref name="index"/> in <see cref="OwinContextSetters"/> that calls
        /// OWIN Identity <see cref="OwinContextExtensions.Set{T}(IOwinContext, T)"/>
        /// with the <typeparamref name="TService"/> from request services.
        /// </summary>
        /// <param name="index">The zero-based index at which the callback should be inserted.</param>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <returns>The current instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="index"/> is less than 0 -or- index is greater than <c><see cref="OwinContextSetters"/>.Count</c>
        /// </exception>
        public IdentityBuilder SetPerOwinContext<TService>(int index)
        {
            OwinContextSetters.Insert(index, SetRequestService<TService>);
            return this;
        }

        private static void SetRequestService<TService>(IOwinContext owin) =>
            owin.Set(owin.GetRequestServices().GetService<TService>());

        Action<IApplicationBuilder> IStartupFilter.Configure(Action<IApplicationBuilder> next)
        {
            if (OwinContextSetters.Count == 0)
                return next;

            return AddMiddleware;

            void AddMiddleware(IApplicationBuilder builder)
            {
                builder.ApplicationServices.GetRequiredService<IAppBuilder>()
                    .Use(IdentityServicesMiddleware);

                next(builder);
            }
        }

        private Task IdentityServicesMiddleware(IOwinContext owin, Func<Task> next)
        {
            foreach (var setter in OwinContextSetters)
                setter(owin);

            return next();
        }
    }
}
