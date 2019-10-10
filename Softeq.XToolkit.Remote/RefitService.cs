using System;
using System.Collections.Generic;
using System.Net.Http;
using Refit;

namespace Softeq.XToolkit.Remote
{
    public interface IRefitService<T>
    {
        T GetByPriority(Priority priority);
    }


    public class RefitService<T> : IRefitService<T>
    {
        private readonly Lazy<T> _coreService;

        public RefitService(
            string apiBaseAddress,
            bool autoRedirectRequests,
            Func<DelegatingHandler> delegatingHandler,
            IDictionary<string, string> defaultHeaders)
        {
            if (string.IsNullOrEmpty(apiBaseAddress))
            {
                throw new ArgumentNullException(nameof(apiBaseAddress));
            }

            Func<HttpMessageHandler, T> createClient = messageHandler =>
            {
                HttpMessageHandler handler;

                if (delegatingHandler != null)
                {
                    var delegatingHandlerInstance = delegatingHandler.Invoke();
                    delegatingHandlerInstance.InnerHandler = messageHandler;
                    handler = delegatingHandlerInstance;
                }
                else
                {
                    handler = messageHandler;
                }

                if (!autoRedirectRequests)
                {
                    DisableAutoRedirects(messageHandler);
                }

                var client = handler == null ? new HttpClient() : new HttpClient(handler);

                client.BaseAddress = new Uri(apiBaseAddress);

                if (defaultHeaders != default)
                {
                    foreach (var header in defaultHeaders)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                return RestService.For<T>(client);
            };

            _coreService = new Lazy<T>(() => createClient(null));

            //_background = new Lazy<T>(() => createClient(NetCache.Background));
        }

        protected virtual void DisableAutoRedirects(HttpMessageHandler messageHandler)
        {
            if (messageHandler is DelegatingHandler internalDelegate
                && internalDelegate.InnerHandler is HttpClientHandler internalClientHandler)
            {
                internalClientHandler.AllowAutoRedirect = false;
            }
        }

        public T GetByPriority(Priority priority)
        {
            //switch (priority)
            //{
            //    case Priority.Background:
            //        return _background.Value;
            //    case Priority.Speculative:
            //        return _speculative.Value;
            //    case Priority.UserInitiated:
            //    default:
            //        return _userInitiated.Value;
            //}

            return _coreService.Value;
        }
    }
}
