using System;
using Refit;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote.Client
{
    public interface IHttpClientProvider<T>
    {
        T GetByPriority(Priority priority);
    }

    public class HttpClientProvider<T> : IHttpClientProvider<T>
    {
        private readonly Lazy<T> _userInitiated;

        public HttpClientProvider(IHttpClientBuilder httpClientBuilder)
        {
            _userInitiated = new Lazy<T>(() => BuildClient(httpClientBuilder, Priority.UserInitiated));
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

            return _userInitiated.Value;
        }

        protected virtual T BuildClient(IHttpClientBuilder httpClientBuilder, Priority priority)
        {
            var httpClient = httpClientBuilder
                    // TODO YP: add custom priority handler (Fusillade)
                    //.WithDelegatingHandler(handler => new HttpDiagnosticsHandler(handler))
                    .Build();

            return RestService.For<T>(httpClient);
        }
    }
}
