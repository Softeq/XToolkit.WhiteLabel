// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public class AsyncCommandGenericTestsCanExecute : CommandTestsCanExecuteBase
    {
        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public void CanExecute_DefaultWithParameters_ReturnsTrue(object parameter)
        {
            var func = CreateFunc<string>();
            var command = CreateAsyncCommandGeneric(func);

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, parameter, true);
        }

        [Fact]
        public void CanExecute_DefaultWithParameters_WhenTypesMismatch_ReturnsFalse()
        {
            var func = CreateFunc<string>();
            var command = CreateAsyncCommandGeneric(func);

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, 123, false);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData(null, false)]
        [InlineData(CommandsDataProvider.DefaultParameter, false)]
        public void CanExecute_NotNullDelegate_ReturnsExpectedValue(object parameter, bool expected)
        {
            var func = CreateFunc<string>();
            var command = CreateAsyncCommandGeneric(func, _ => expected);

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, parameter, expected);
        }

        [Fact]
        public void CanExecute_WhileExecuting_ReturnsFalse()
        {
            var func = CreateFuncWithDelay<string>();
            var command = CreateAsyncCommandGeneric(func);

            Assert_CanExecute_AfterExecuteWithParameter_ReturnsExpectedValue(command, null, false);
        }
    }
}
