// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Xunit;
using Command = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using Execute = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public class AsyncCommandTestsCanExecute : CommandTestsCanExecuteBase
    {
        [Fact]
        public void CanExecute_WithNullParameter_ReturnsTrue()
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, null, true);
        }

        [Theory]
        [InlineData(123)]
        [InlineData("test")]
        public void CanExecute_WithUnsupportedParameters_IgnoresParameter(object parameter)
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, parameter, true);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CanExecute_NotNullDelegate_ReturnsExpectedValue(bool expected)
        {
            var func = Execute.Create();
            var command = Command.Create(func, () => expected);

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, null, expected);
        }

        [Fact]
        public void CanExecute_WhileExecuting_ReturnsFalse()
        {
            var tcs = new TaskCompletionSource<bool>();
            var func = Execute.FromSource(tcs);
            var command = Command.Create(func);

            Assert_CanExecute_AfterExecuteWithParameter_ReturnsExpectedValue(command, null, false);

            tcs.SetResult(true);
        }

        [Fact]
        public void CanExecute_AfterExecuteTargetGarbageCollected_ReturnsFalse()
        {
            var command = Command.WithGarbageCollectableExecuteTarget(() => Task.CompletedTask);

            GC.Collect();

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, null, false);
        }

        [Fact]
        public void CanExecute_AfterCanExecuteTargetGarbageCollected_ReturnsFalse()
        {
            var command = Command.WithGarbageCollectableCanExecuteTarget(() => Task.CompletedTask, () => true);

            GC.Collect();

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, null, false);
        }
    }
}
