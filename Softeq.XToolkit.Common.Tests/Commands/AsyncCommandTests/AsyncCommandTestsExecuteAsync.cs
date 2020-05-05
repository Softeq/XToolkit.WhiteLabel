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
        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public async Task ExecuteAsync_CalledOneTime_ExecutesOneTime(string parameter)
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            await command.ExecuteAsync(parameter);

            await func.Received(1).Invoke();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public async Task ExecuteAsync_AsyncWithException_ThrowsException(string parameter)
        {
            var func = Execute.WithException();
            var command = Command.Create(func);

            await Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync(parameter));
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
