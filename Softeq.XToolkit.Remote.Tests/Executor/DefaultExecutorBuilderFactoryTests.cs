// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Remote.Executor;
using Xunit;

namespace Softeq.XToolkit.Remote.Tests.Executor
{
    public class DefaultExecutorBuilderFactoryTests
    {
        [Fact]
        public void Create_Default_ReturnsCorrectInstance()
        {
            var factory = new DefaultExecutorBuilderFactory();

            var result = factory.Create();

            Assert.IsAssignableFrom<IExecutorBuilder>(result);
        }
    }
}
