// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Logger;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Logger
{
    public class ConsoleLogManagerTests
    {
        private readonly ILogManager _logManager;

        public ConsoleLogManagerTests()
        {
            _logManager = new ConsoleLogManager();
        }

        [Fact]
        public void GetLogger_Generic_ReturnsLogger()
        {
            var result = _logManager.GetLogger<string>();

            Assert.IsAssignableFrom<ILogger>(result);
        }

        [Theory]
        [InlineData("TestName")]
        [InlineData(null)]
        public void GetLogger_WithName_ReturnsLogger(string name)
        {
            var result = _logManager.GetLogger(name);

            Assert.IsAssignableFrom<ILogger>(result);
        }
    }
}
