// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public partial class AsyncCommandTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public async Task ExecuteAsync_CalledOneTime_ExecutesOneTime(string parameter)
        {
            var command = CreateAsyncCommand(_func);

            await command.ExecuteAsync(parameter);

            await _func.Received(1).Invoke();
        }

        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public async Task ExecuteAsync_AsyncWithException_ThrowsException(string parameter)
        {
            var func = GetFuncWithException();
            var command = CreateAsyncCommand(func);

            await Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync(parameter));
        }
    }
}
