// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Xunit;
using Command = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using Execute = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public class AsyncCommandGenericTestsCanExecute : CommandTestsCanExecuteBase
    {
        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public void CanExecute_DefaultWithParameters_ReturnsTrue(object parameter)
        {
            var func = Execute.CreateFunc<string>();
            var command = Command.CreateAsyncCommandGeneric(func);

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, parameter, true);
        }

        [Fact]
        public void CanExecute_DefaultWithParameters_WhenTypesMismatch_ReturnsFalse()
        {
            var func = Execute.CreateFunc<string>();
            var command = Command.CreateAsyncCommandGeneric(func);

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, 123, false);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData(null, false)]
        [InlineData(CommandsDataProvider.DefaultParameter, false)]
        public void CanExecute_NotNullDelegate_ReturnsExpectedValue(object parameter, bool expected)
        {
            var func = Execute.CreateFunc<string>();
            var command = Command.CreateAsyncCommandGeneric(func, _ => expected);

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, parameter, expected);
        }

        [Fact]
        public void CanExecute_WhileExecuting_ReturnsFalse()
        {
            var func = Execute.CreateFuncWithDelay<string>();
            var command = Command.CreateAsyncCommandGeneric(func);

            Assert_CanExecute_AfterExecuteWithParameter_ReturnsExpectedValue(command, null, false);
        }

        [Theory]
        [InlineData("test")]
        public void CanExecute_AfterExecuteTargetGarbageCollected_ReturnsFalse<T>(T parameter)
        {
            var command = Command.WithGarbageCollectableExecuteTarget<T>(_ => Task.CompletedTask);

            GC.Collect();

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, parameter, false);
        }

        [Theory]
        [InlineData("test")]
        public void CanExecute_AfterCanExecuteTargetGarbageCollected_ReturnsFalse<T>(T parameter)
        {
            var command = Command.WithGarbageCollectableCanExecuteTarget<T>(_ => Task.CompletedTask, _ => true);

            GC.Collect();

            Assert_CanExecute_WithParameter_ReturnsExpectedValue(command, parameter, false);
        }

        [Theory]
        [InlineData("test")]
        public void CanExecuteGeneric_AfterExecuteTargetGarbageCollected_ReturnsFalse<T>(T parameter)
        {
            var command = Command.WithGarbageCollectableExecuteTarget<T>(_ => Task.CompletedTask);

            GC.Collect();

            var result = command.CanExecute(parameter);

            Assert.False(result);
        }

        [Theory]
        [InlineData("test")]
        public void CanExecuteGeneric_AfterCanExecuteTargetGarbageCollected_ReturnsFalse<T>(T parameter)
        {
            var command = Command.WithGarbageCollectableCanExecuteTarget<T>(_ => Task.CompletedTask, _ => true);

            GC.Collect();

            var result = command.CanExecute(parameter);

            Assert.False(result);
        }
    }
}
