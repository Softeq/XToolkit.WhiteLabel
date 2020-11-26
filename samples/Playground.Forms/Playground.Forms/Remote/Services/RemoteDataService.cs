// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading.Tasks;
using Playground.Forms.Remote.Services.Dtos;
using Refit;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.Remote.Client;

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

        public async Task<string> TestRequestAsync()
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
                s.ResponseHeadersAsync()
            );
            return result;
        }
    }
}
