using System;
using Polly;

namespace Softeq.XToolkit.Remote.Executor
{
    public interface IExecutorBuilder<T>
    {
        IExecutorBuilder<T> WithRetry(int retryCount, Func<Exception, bool> shouldRetry);
        IExecutorBuilder<T> WithTimeout(int timeout);
        IAsyncPolicy<T> Build();
    }
}