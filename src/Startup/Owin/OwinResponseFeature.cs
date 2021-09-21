using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace Unravel.Owin
{
    public class OwinResponseFeature : IHttpResponseFeature
    {
        private readonly IHttpResponseFeature response;
        private readonly ILogger logger;

        public OwinResponseFeature(IHttpResponseFeature response, ILogger logger)
        {
            this.response = response;
            this.logger = logger;
        }

        public int StatusCode
        {
            get => response.StatusCode;
            set => response.StatusCode = value;
        }

        public string ReasonPhrase
        {
            get => response.ReasonPhrase;
            set => response.ReasonPhrase = value;
        }

        public IHeaderDictionary Headers
        {
            get => response.Headers;
            set => response.Headers = value;
        }

        public Stream Body
        {
            get => response.Body;
            set => response.Body = value;
        }

        public bool HasStarted => response.HasStarted;

        public void OnStarting(Func<object, Task> callback, object state)
        {
            logger.LogDebug("OnStarting {0}.{1}(state)", callback?.Method.DeclaringType.FullName, callback?.Method.Name);

            response.OnStarting(callback, state);
        }

        public void OnCompleted(Func<object, Task> callback, object state)
        {
            logger.LogDebug("OnCompleted {0}.{1}(state)", callback?.Method.DeclaringType.FullName, callback?.Method.Name);

            // TODO: Save/call these
        }
    }
}
