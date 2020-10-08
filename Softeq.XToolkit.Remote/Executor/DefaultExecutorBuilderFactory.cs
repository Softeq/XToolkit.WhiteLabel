// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.Remote.Executor
{
    /// <summary>
    ///     Default factory for creating <see cref="IExecutorBuilder"/> instances.
    /// </summary>
    public class DefaultExecutorBuilderFactory : IExecutorBuilderFactory
    {
        /// <inheritdoc />
        public IExecutorBuilder Create()
        {
            return new DefaultExecutorBuilder();
        }
    }
}
