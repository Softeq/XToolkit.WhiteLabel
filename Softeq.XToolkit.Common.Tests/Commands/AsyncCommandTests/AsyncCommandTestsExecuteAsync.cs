// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public class AsyncCommandTestsExecuteAsync
    {
        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public async Task ExecuteAsync_CalledOneTime_ExecutesOneTime(string parameter)
        {
            var func = CreateFunc();
            var command = CreateAsyncCommand(func);

            await command.ExecuteAsync(parameter);

            await func.Received(1).Invoke();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public async Task ExecuteAsync_AsyncWithException_ThrowsException(string parameter)
        {
            var func = CreateFuncWithException();
            var command = CreateAsyncCommand(func);

            await Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync(parameter));
        }
    }
}
