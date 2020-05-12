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
    public class AsyncCommandGenericTestsExecuteAsync
    {
        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public async Task ExecuteAsync_CalledOneTime_ExecutesOneTime(string parameter)
        {
            var func = Execute.Create<string>();
            var command = Command.Create(func);

            await command.ExecuteAsync(parameter);

            await func.Received(1).Invoke(parameter);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public async Task ExecuteAsync_AsyncWithException_ThrowsException(string parameter)
        {
            var func = Execute.WithException<string>();
            var command = Command.Create(func);

            await Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync(parameter));
        }

        [Theory]
        [InlineData("test")]
        public async Task ExecuteAsync_AfterExecuteTargetGarbageCollected_DoesNotExecute<T>(T parameter)
        {
            var execute = Execute.Create<T>();
            var command = Command.WithGarbageCollectableExecuteTarget(execute);

            GC.Collect();

            await command.ExecuteAsync(parameter);

            await execute.DidNotReceive().Invoke(Arg.Any<T>());
        }

        [Theory]
        [InlineData("test")]
        public async Task ExecuteAsync_AfterCanExecuteTargetGarbageCollected_DoesNotExecute<T>(T parameter)
        {
            var execute = Execute.Create<T>();
            var command = Command.WithGarbageCollectableCanExecuteTarget(execute, _ => true);

            GC.Collect();

            await command.ExecuteAsync(parameter);

            await execute.DidNotReceive().Invoke(Arg.Any<T>());
        }
    }
}
