using System;
using System.Collections.Generic;
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

        public bool AssumeBodyModified { get; private set; }

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
            get { AssumeBodyModified = true; return response.Body; }
            set { AssumeBodyModified = true; response.Body = value; }
        }

        public bool HasStarted => response.HasStarted;

        public void OnStarting(Func<object, Task> callback, object state)
        {
            logger.LogDebug("OnStarting {0}.{1}(state)", callback?.Method.DeclaringType.ToString(), callback?.Method.Name);

            response.OnStarting(callback, state);
        }

        private readonly object _onCompletedSync = new object();
        private Stack<(Func<object, Task>, object)> _onCompleted;

        // From: https://github.com/dotnet/aspnetcore/blob/c2cfc5f140cd2743ecc33eeeb49c5a2dd35b017f/src/Servers/Kestrel/Core/src/Internal/Http/HttpProtocol.cs#L658-L668
        public void OnCompleted(Func<object, Task> callback, object state)
        {
            logger.LogDebug("OnCompleted {0}.{1}(state)", callback?.Method.DeclaringType.ToString(), callback?.Method.Name);

            lock (_onCompletedSync)
            {
                if (_onCompleted == null)
                {
                    _onCompleted = new Stack<(Func<object, Task>, object)>();
                }
                _onCompleted.Push((callback, state));
            }
        }

        // From: https://github.com/dotnet/aspnetcore/blob/c2cfc5f140cd2743ecc33eeeb49c5a2dd35b017f/src/Servers/Kestrel/Core/src/Internal/Http/HttpProtocol.cs#L732-L762
        public Task FireOnCompletedAsync()
        {
            Stack<(Func<object, Task>, object)> onCompleted;
            lock (_onCompletedSync)
            {
                onCompleted = _onCompleted;
                _onCompleted = null;
            }

            if (onCompleted == null)
            {
                return Task.CompletedTask;
            }

            return FireOnCompletedAwaited(onCompleted);
        }

        private async Task FireOnCompletedAwaited(Stack<(Func<object, Task>, object)> onCompleted)
        {
            foreach (var (callback, state) in onCompleted)
            {
                try
                {
                    await callback(state);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An unhandled exception was thrown by the application.");
                }
            }
        }
    }
}
