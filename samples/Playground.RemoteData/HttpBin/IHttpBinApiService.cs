// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Playground.RemoteData.HttpBin.Dtos;
using Refit;

namespace Playground.RemoteData.HttpBin
{
    // API Docs: https://httpbin.org

    [Headers("Header-Interface: 1")]
    public interface IHttpBinApiService
    {
        [Get("/get")]
        Task<string> GetRequestAsync(string foo1, [AliasAs("foo2")] string arg2, CancellationToken ct);

        [Post("/post")]
        Task<string> PostRawRequestAsync(string foo1, [Body] PostContent content, CancellationToken ct);

        [Post("/post")]
        Task<string> PostFormRequestAsync([Body(BodySerializationMethod.UrlEncoded)] PostContent content, CancellationToken ct);

        [Multipart]
        [Post("/post")]
        Task<string> PostMultipartFormRequestAsync([AliasAs("fooBar")] PostContent content, CancellationToken ct);

        [Multipart]
        [Post("/post")]
        Task<string> PostFileRequestAsync([AliasAs("myPhoto")] StreamPart photoStream, CancellationToken ct);

        [Put("/put")]
        Task<string> PutRequestAsync([Body] PostContent content, CancellationToken ct);

        [Patch("/patch")]
        Task<string> PatchRequestAsync([Body] PostContent content, CancellationToken ct);

        [Delete("/delete")]
        Task<string> DeleteRequestAsync([Body] PostContent content, CancellationToken ct);

        [Get("/headers")]
        [Headers("Header-Method: 2")]
        Task<string> RequestHeadersAsync([Header("Header-Parameter")] int c, CancellationToken ct);

        [Get("/response-headers")]
        Task<string> ResponseHeadersAsync([AliasAs("freeform")] string foo, CancellationToken ct);

        [Get("/cookies/set")]
        Task<string> SetCookiesAsync(string freeform, CancellationToken ct);

        [Get("/cookies")]
        Task<string> GetCookiesAsync(CancellationToken ct);

        [Get("/status/{statusCode}")]
        Task<string> ResponseStatusAsync(int statusCode, CancellationToken ct);

        [Get("/stream/{itemsCount}")]
        Task<Stream> GetStreamDataAsync(int itemsCount, CancellationToken ct);

        [Get("/delay/{delayInSeconds}")]
        Task<string> GetWithDelayAsync(int delayInSeconds, CancellationToken ct);

        [Get("/gzip")]
        Task<string> GetGzipAsync(CancellationToken ct);

        [Get("/deflate")]
        Task<string> GetDeflateAsync(CancellationToken ct);
    }
}
