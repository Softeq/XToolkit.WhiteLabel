// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Polly;

namespace Softeq.XToolkit.Remote.Executor
{
    /// <summary>
    ///     A builder abstraction for configuring Polly <see cref="T:Polly.IAsyncPolicy"/> instances.
    /// </summary>
    public interface IExecutorBuilder
    {
        /// <summary>
        ///      Add a timeout policy to the executor.
        /// </summary>
        /// <param name="timeout">Timeout value in seconds.</param>
        /// <returns>Modified instance of <see cref="IExecutorBuilder"/>.</returns>
        IExecutorBuilder WithTimeout(int timeout);

        /// <summary>
        ///      Add a retry policy to the executor.
        /// </summary>
        /// <param name="retryCount">The count of request retries when errors happened.</param>
        /// <param name="shouldRetry">The condition when request should be retry.</param>
        /// <returns>Modified instance of <see cref="IExecutorBuilder"/>.</returns>
        IExecutorBuilder WithRetry(int retryCount, Func<Exception, bool> shouldRetry);

        /// <summary>
        ///     Build the executor.
        /// </summary>
        /// <returns>Composition of Polly policies.</returns>
        IAsyncPolicy Build();
    }
}
