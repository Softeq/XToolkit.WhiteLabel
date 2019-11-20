using System;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.Remote.Primitives;

namespace Softeq.XToolkit.Remote
{
    public interface IRemoteService<out TApiService>
    {
        Task<TResult> MakeRequest<TResult>(
            Func<TApiService, CancellationToken, Task<TResult>> operation,
            RequestOptions options = null);

        Task MakeRequest(
            Func<TApiService, CancellationToken, Task> operation,
            RequestOptions options = null);
    }
}