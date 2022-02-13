using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Unravel.AspNet.Mvc.Internal
{
    /// <summary>
    /// Resolves <see cref="MvcOptions"/> to apply configuration.
    /// </summary>
    public class MvcOptionsStartupFilter : IStartupFilter
    {
        public MvcOptionsStartupFilter(IOptions<MvcOptions> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (options.Value == null)
                throw new InvalidOperationException($"{nameof(MvcOptions)} not found.");
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => next;
    }
}
