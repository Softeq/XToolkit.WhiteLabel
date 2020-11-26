// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Playground.Forms.Remote.Services.Dtos;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;

namespace Playground.Forms.Remote.Services
{
    public class RemoteDataService
    {
        private readonly ILogger _logger;
        private readonly IRemoteService<IPostmanEchoRemoteService> _remoteService;

        public RemoteDataService(ILogManager logManager)
        {
            _logger = logManager.GetLogger<RemoteDataService>();

            var httpClient = new DefaultHttpClientFactory().CreateClient("https://postman-echo.com", _logger);

            _remoteService = new RemoteServiceFactory().Create<IPostmanEchoRemoteService>(httpClient);
        }

        public async Task<string> TestRequestAsync(CancellationToken cancellationToken)
        {
            await using var memoryStream = new MemoryStream();
            memoryStream.SetLength(1 * 1024 * 1024); // 1Mb

            var bodyContent = new PostContent { Name = "TestName", Age = 100500 };

            var result = await _remoteService.MakeRequest((s, ct) =>
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
                s.ResponseStatusAsync((int)HttpStatusCode.OK)
                // s.ResponseStatusAsync((int)HttpStatusCode.Redirect) // YP: Check retry !!
                // s.ResponseStatusAsync((int)HttpStatusCode.NotFound) // YP: Check retry !!
                // s.ResponseStatusAsync((int)HttpStatusCode.InternalServerError) // YP: Check retry !!
            );

            await _remoteService.MakeRequest(
                async (s, ct) =>
                {
                    // get stream with not structured multiline json
                    var stream = await s.GetStreamDataAsync(100000);

                    using var streamReader = new StreamReader(stream);
                    using var jsonReader = new JsonTextReader(streamReader)
                    {
                        SupportMultipleContent = true
                    };

                    // manual deserialization
                    var items = DeserializationHelper.DeserializeAsync(jsonReader, ct);

                    // print results
                    await foreach (var item in items.WithCancellation(ct))
                    {
                        Debug.WriteLine(item);
                    }
                },
                new RequestOptions
                {
                    CancellationToken = cancellationToken
                });

            return result;
        }
    }
}
