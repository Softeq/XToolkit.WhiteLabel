// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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

        private readonly ILogger _logger;
        private readonly IRemoteService<IHttpBinApiService> _remoteService;

        public HttpBinRemoteService(ILogManager logManager)
        {
            _logger = logManager.GetLogger<HttpBinRemoteService>();

            var httpClient = new DefaultHttpClientFactory().CreateClient(ApiUrl, _logger);
            // httpClient.Timeout = TimeSpan.FromSeconds(2);

            _remoteService = new RemoteServiceFactory().Create<IHttpBinApiService>(httpClient);
        }

        public Task CheckStatusCodeAsync(int statusCode, CancellationToken cancellationToken)
        {
            return _remoteService.SafeRequest(
                (s, ct) => s.ResponseStatusAsync(statusCode, ct),
                cancellationToken,
                _logger);
        }

        public async Task<string> TestRequestAsync(CancellationToken cancellationToken)
        {
            await using var memoryStream = new MemoryStream();
            memoryStream.SetLength(1 * 1024 * 1024); // 1Mb

            var bodyContent = new PostContent { Name = "TestName", Age = 100500 };

            var result = await _remoteService.MakeRequest(
                (s, ct) =>
                // s.GetRequestAsync("foo1-value", "foo2-value")
                // s.PostRawRequestAsync("foo1-value", bodyContent)
                // s.PostFormRequestAsync(bodyContent)
                // s.PostMultipartFormRequestAsync(bodyContent)
                // s.PostFileRequestAsync(new StreamPart(memoryStream, "photo.jpg", "image/jpeg"))
                // s.PutRequestAsync(bodyContent)
                // s.PatchRequestAsync(bodyContent)
                // s.DeleteRequestAsync(bodyContent)
                // s.RequestHeadersAsync()
                // s.ResponseHeadersAsync()
                // s.SetCookiesAsync()
                // s.GetCookiesAsync()
                s.ResponseStatusAsync((int)HttpStatusCode.OK, cancellationToken)
                // s.ResponseStatusAsync((int)HttpStatusCode.Redirect, cancellationToken)
                // s.ResponseStatusAsync((int)HttpStatusCode.NotFound, cancellationToken)
                // s.ResponseStatusAsync((int)HttpStatusCode.InternalServerError, cancellationToken)
                // s.GetGzipAsync()
                // s.GetDeflateAsync()
                , new RequestOptions
                {
                    ShouldRetry = ex => ex is ApiException apiException && (int)apiException.StatusCode >= 500
                }
            );




            // var cts = new CancellationTokenSource();
            // cts.CancelAfter(2000);

            // var result2 = await _remoteService.MakeRequest(
            //     (s, ct) => s.GetWithDelayAsync(5, ct),
            //     new RequestOptions
            //     {
            //          // Timeout = 3,
            //          // CancellationToken = cts.Token
            //     });

            // await _remoteService.MakeRequest(
            //     async (s, ct) =>
            //     {
            //         // get stream with not structured multiline json
            //         var stream = await s.GetStreamDataAsync(99);
            //
            //         using var streamReader = new StreamReader(stream);
            //         using var jsonReader = new JsonTextReader(streamReader)
            //         {
            //             SupportMultipleContent = true
            //         };
            //
            //         // manual deserialization
            //         var items = DeserializationHelper.DeserializeAsync(jsonReader, ct);
            //
            //         // print results
            //         await foreach (var item in items.WithCancellation(ct))
            //         {
            //             Debug.WriteLine(item);
            //         }
            //     },
            //     new RequestOptions
            //     {
            //         CancellationToken = cancellationToken
            //     });

            return result;
        }
    }
}
