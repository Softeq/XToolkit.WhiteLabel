// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Softeq.XToolkit.Remote.Tests
{
    public interface IStubApiInterface
    {
        [Get("/")] // required to generate Refit implementation
        Task<string> Get(CancellationToken ct);

        Task<string> GetData(CancellationToken ct);

        Task DoException();

        Task<string> DoException(CancellationToken ct);
    }
}
