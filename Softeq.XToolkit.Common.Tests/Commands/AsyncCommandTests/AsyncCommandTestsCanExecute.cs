// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public class AsyncCommandTestsCanExecute : CommandTestsCanExecuteBase
    {
        [Theory]
        [InlineData(null)]
        [InlineData(123)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public void CanExecute_DefaultWithParameters_ReturnsTrue(object parameter)
        {
            var func = CreateFunc();
            var command = CreateAsyncCommand(func);

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, parameter, true);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData(null, false)]
        [InlineData(CommandsDataProvider.DefaultParameter, false)]
        [InlineData(123, true)]
        public void CanExecute_NotNullDelegate_ReturnsExpectedValue(object parameter, bool expected)
        {
            var func = CreateFunc();
            var command = CreateAsyncCommand(func, () => expected);

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, parameter, expected);
        }

        [Fact]
        public void CanExecute_WhileExecuting_ReturnsFalse()
        {
            var func = CreateFuncWithDelay();
            var command = CreateAsyncCommand(func);

            Assert_CanExecute_AfterExecuteWithParameter_ReturnsExpectedValue(command, null, false);
        }
    }
}
