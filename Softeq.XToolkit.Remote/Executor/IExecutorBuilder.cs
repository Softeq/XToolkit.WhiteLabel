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

    public interface IExecutorBuilder
    {
        IExecutorBuilder WithRetry(int retryCount, Func<Exception, bool> shouldRetry);
        IExecutorBuilder WithTimeout(int timeout);
        IAsyncPolicy Build();
    }
}
