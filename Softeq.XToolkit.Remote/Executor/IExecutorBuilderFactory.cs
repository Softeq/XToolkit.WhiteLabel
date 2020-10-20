// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Remote.Executor
{
    /// <summary>
    ///     A factory abstraction for a component that can create <see cref="IExecutorBuilder"/> instances.
    /// </summary>
    public interface IExecutorBuilderFactory
    {
        /// <summary>
        ///     Creates simple <see cref="IExecutorBuilder"/> instance.
        /// </summary>
        /// <returns>Instance of <see cref="IExecutorBuilder"/>.</returns>
        IExecutorBuilder Create();
    }
}
