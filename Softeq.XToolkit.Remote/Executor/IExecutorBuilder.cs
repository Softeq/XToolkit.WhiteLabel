using System;
using Polly;

namespace Softeq.XToolkit.Remote.Executor
{
    public interface IExecutorBuilder
    {
        IExecutorBuilder WithTimeout(int timeout);
        IExecutorBuilder WithRetry(int retryCount, Func<Exception, bool> shouldRetry);
        IAsyncPolicy Build();
    }
}
