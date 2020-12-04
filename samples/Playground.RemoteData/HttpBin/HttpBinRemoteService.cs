// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Playground.RemoteData.HttpBin.Dtos;
using Refit;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace Playground.RemoteData.HttpBin
{
    public class HttpBinRemoteService
    {
        private const string ApiUrl = "https://httpbin.org";

        private readonly IRemoteService<IHttpBinApiService> _remoteService;

        public HttpBinRemoteService(
            IHttpClientFactory httpClientFactory,
            IRemoteServiceFactory remoteServiceFactory,
            ILogManager logManager)
        {
            var logger = logManager.GetLogger<HttpBinRemoteService>();

            var httpClient = httpClientFactory.CreateClient(ApiUrl, logger);
            httpClient.Timeout = TimeSpan.FromSeconds(5); // custom timeout on http handler level

            _remoteService = remoteServiceFactory.Create<IHttpBinApiService>(httpClient);
        }

        private PostContent MockBody => new PostContent { Name = "TestName", Age = 100500 };

        public Task<string> UploadAsync(Stream stream, CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.PostFileRequestAsync(new StreamPart(stream, "photo.jpg", "image/jpeg"), ct),
                cancellationToken);
        }

        public Task<string> GetAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.GetRequestAsync("foo1-value", "foo2-value", ct),
                cancellationToken);
        }

        public Task<string> PostRawAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.PostRawRequestAsync("foo1-value", MockBody, ct),
                cancellationToken);
        }

        public Task<string> PostFormAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.PostFormRequestAsync(MockBody, ct),
                cancellationToken);
        }

        public Task<string> PostMultipartFormAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.PostMultipartFormRequestAsync(MockBody, ct),
                cancellationToken);
        }

        public Task<string> PutAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.PutRequestAsync(MockBody, ct),
                cancellationToken);
        }

        public Task<string> PatchAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.PatchRequestAsync(MockBody, ct),
                cancellationToken);
        }

        public Task<string> DeleteAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.DeleteRequestAsync(MockBody, ct),
                cancellationToken);
        }

        public Task<string> RequestHeadersAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.RequestHeadersAsync(100500, ct),
                cancellationToken);
        }

        public Task<string> ResponseHeadersAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.ResponseHeadersAsync("foo-value", ct),
                cancellationToken);
        }

        public Task<string> SetCookiesAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.SetCookiesAsync("foo-value", ct),
                cancellationToken);
        }

        public Task<string> GetCookiesAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.GetCookiesAsync(ct),
                cancellationToken);
        }

        public Task<string> GetGzipAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.GetGzipAsync(ct),
                cancellationToken);
        }

        public Task<string> GetDeflateAsync(CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.GetDeflateAsync(ct),
                cancellationToken);
        }

        public Task<string> StatusCodeAsync(int statusCode, CancellationToken cancellationToken)
        {
            return _remoteService.MakeRequest(
                (s, ct) => s.ResponseStatusAsync(statusCode, ct),
                new RequestOptions
                {
                    CancellationToken = cancellationToken,

                    // YP: ignore some API errors
                    ShouldRetry = ex => ex is ApiException apiException
                                        && (int)apiException.StatusCode >= 500
                });
        }

        public Task<string> DelayWithTimeoutAsync(int delay, int timeout, CancellationToken cancellationToken)
        {
            return _remoteService.MakeRequest(
                (s, ct) => s.GetWithDelayAsync(delay, ct),
                new RequestOptions
                {
                     Timeout = timeout,
                     CancellationToken = cancellationToken
                });
        }

        public Task<IAsyncEnumerable<EchoResponse>> GetStreamAsync(int itemsCount, CancellationToken cancellationToken)
        {
            return _remoteService.MakeRequest(
                async (s, ct) =>
                {
                    // get stream with multiline json
                    var stream = await s.GetStreamDataAsync(itemsCount, ct);

                    var streamReader = new StreamReader(stream);
                    var jsonReader = new JsonTextReader(streamReader)
                    {
                        SupportMultipleContent = true
                    };

                    return DeserializationHelper.DeserializeAsync<EchoResponse>(jsonReader, ct);
                },
                new RequestOptions
                {
                    CancellationToken = cancellationToken
                });
        }
    }
}
