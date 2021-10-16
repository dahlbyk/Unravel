using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Unravel.AspNetCore.Mvc.Internal
{

    public class IgnoreControllersOfTypeActionInvokerProvider<TController> : IActionInvokerProvider
    {
        /// <summary>
        ///   Gets the order value for determining the order of execution of providers.
        ///   Returns -995, to execute shortly after <see cref="Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvokerProvider"/> (-1000).
        /// </summary>
        public virtual int Order => -995;

        /// <summary>
        ///   Intercepts <see cref="ControllerActionDescriptor"/> for <typeparamref name="TController"/>.
        /// </summary>
        /// <param name="context">The <see cref="ActionInvokerProviderContext"/>.</param>
        public void OnProvidersExecuting(ActionInvokerProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.ActionContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var controllerTypeInfo = controllerActionDescriptor.ControllerTypeInfo;
                if (typeof(TController).GetTypeInfo().IsAssignableFrom(controllerTypeInfo))
                {
                    context.Result = new NotFoundActionInvoker(context.ActionContext);
                }
            }
        }

        /// <inheritdoc/>
        public void OnProvidersExecuted(ActionInvokerProviderContext context)
        {
        }

        private class NotFoundActionInvoker : IActionInvoker
        {
            private readonly ActionContext actionContext;

            public NotFoundActionInvoker(ActionContext actionContext)
            {
                this.actionContext = actionContext;
            }

            public Task InvokeAsync()
            {
                actionContext.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

                return Task.CompletedTask;
            }
        }
    }
}
