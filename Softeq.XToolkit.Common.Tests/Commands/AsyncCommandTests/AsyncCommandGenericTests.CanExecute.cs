// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public partial class AsyncCommandGenericTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        [InlineData(123)]
        public void CanExecute_DefaultWithParameters_ReturnsTrue(object parameter)
        {
            var command = CreateAsyncCommand(_func);

            var result = command.CanExecute(parameter);

            Assert.True(result);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData(null, false)]
        [InlineData(CommandsDataProvider.DefaultParameter, false)]
        [InlineData(123, true)]
        public void CanExecute_NotNullDelegate_ReturnsExpectedValue(object parameter, bool canExecute)
        {
            var command = CreateAsyncCommand(_func, () => canExecute);

            var result = command.CanExecute(parameter);

            Assert.Equal(canExecute, result);
        }

        [Fact]
        public void CanExecute_ExecuteWithDelayWasCalled_ReturnsFalse()
        {
            var func = GetFuncWithDelay();
            var command = CreateAsyncCommand(func);

            command.Execute(null);
            var result = command.CanExecute(null);

            Assert.False(result);
        }
    }
}
