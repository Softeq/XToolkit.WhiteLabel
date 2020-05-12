// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using Command = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using Execute = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public class AsyncCommandTestsExecuteAsync
    {
        [Fact]
        public async Task ExecuteAsync_CalledOneTime_ExecutesOneTime()
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            await command.ExecuteAsync(null);

            await func.Received(1).Invoke();
        }

        [Theory]
        [InlineData(0)]
        [InlineData("test")]
        public async Task ExecuteAsync_WithUnsupportedParameter_IgnoresParameter(object parameter)
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            await command.ExecuteAsync(parameter);

            await func.Received(1).Invoke();
        }

        [Fact]
        public async Task ExecuteAsync_AsyncWithException_ThrowsException()
        {
            var func = Execute.WithException();
            var command = Command.Create(func);

            await Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync(null));
        }

        [Fact]
        public async Task ExecuteAsync_AfterExecuteTargetGarbageCollected_DoesNotExecute()
        {
            var execute = Execute.Create();
            var command = Command.WithGarbageCollectableExecuteTarget(execute);

            GC.Collect();

            await command.ExecuteAsync(null);

            await execute.DidNotReceive().Invoke();
        }

        [Fact]
        public async Task ExecuteAsync_AfterCanExecuteTargetGarbageCollected_DoesNotExecute()
        {
            var execute = Execute.Create();
            var command = Command.WithGarbageCollectableCanExecuteTarget(execute, () => true);

            GC.Collect();

            await command.ExecuteAsync(null);

            await execute.DidNotReceive().Invoke();
        }
    }
}
