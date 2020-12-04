﻿// Developed by Softeq Development Corporation
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
        Task<string> GetRequestAsync(string foo1, [AliasAs("foo2")] string arg2);

        [Post("/post")]
        Task<string> PostRawRequestAsync(string foo1, [Body] PostContent content);

        [Post("/post")]
        Task<string> PostFormRequestAsync([Body(BodySerializationMethod.UrlEncoded)] PostContent content);

        [Multipart]
        [Post("/post")]
        Task<string> PostMultipartFormRequestAsync([AliasAs("fooBar")] PostContent content);

        [Multipart]
        [Post("/post")]
        Task<string> PostFileRequestAsync([AliasAs("myPhoto")] StreamPart photoStream);

        [Put("/put")]
        Task<string> PutRequestAsync([Body] PostContent content);

        [Patch("/patch")]
        Task<string> PatchRequestAsync([Body] PostContent content);

        [Delete("/delete")]
        Task<string> DeleteRequestAsync([Body] PostContent content);

        [Get("/headers")]
        [Headers("Header-Method: 2")]
        Task<string> RequestHeadersAsync([Header("Header-Parameter")] int c = 100500);

        [Get("/response-headers")]
        Task<string> ResponseHeadersAsync([AliasAs("freeform")] string foo = "foo-value");

        [Get("/cookies/set")]
        Task<string> SetCookiesAsync(string freeform = "foo-value");

        [Get("/cookies")]
        Task<string> GetCookiesAsync();

        [Get("/status/{statusCode}")]
        Task<string> ResponseStatusAsync(int statusCode, CancellationToken ct);

        [Get("/stream/{itemsCount}")]
        Task<Stream> GetStreamDataAsync(int itemsCount = 10);

        [Get("/delay/{delayInSeconds}")]
        Task<string> GetWithDelayAsync(int delayInSeconds, CancellationToken ct);

        [Get("/gzip")]
        Task<string> GetGzipAsync();

        [Get("/deflate")]
        Task<string> GetDeflateAsync();
    }
}
