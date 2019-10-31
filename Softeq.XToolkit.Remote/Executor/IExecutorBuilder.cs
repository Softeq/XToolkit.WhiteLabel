using System;
using System.Threading.Tasks;
using Polly;

namespace Softeq.XToolkit.Remote.Executor
{
    public interface IExecutorBuilder<T>
    {
        IExecutorBuilder<T> WithRetry(int retryCount, Func<Exception, bool> shouldRetry);
        IExecutorBuilder<T> WithTimeout(int timeout);
        IExecutorBuilder<T> WithRefreshToken(Func<Task> refreshToken);
        IAsyncPolicy<T> Build();
    }
}
