// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Playground.Forms.Remote.Services.Dtos;
using Refit;

namespace Playground.Forms.Remote.Services
{
    // API Docs: https://docs.postman-echo.com/

    public interface IPostmanEchoRemoteService
    {
        [Get("/get")]
        Task<EchoResponse> GetRequestAsync(string foo1, [AliasAs("foo2")] string arg2);

        [Post("/post")]
        Task<string> PostRequestAsync();

        [Put("/put")]
        Task<string> PutRequestAsync();
    }
}
