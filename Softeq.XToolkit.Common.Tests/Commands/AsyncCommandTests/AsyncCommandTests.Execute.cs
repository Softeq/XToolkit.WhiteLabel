// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public partial class AsyncCommandTests
    {
        [Fact]
        public void Execute_CalledOneTime_ExecutesOneTime()
        {
            var command = CreateAsyncCommand(_func);

            command.Execute(null);

            _func.Received(1).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteTrue_ExecutesOneTime(string parameter)
        {
            var command = CreateAsyncCommand(_func, () => true);

            command.Execute(parameter);

            _func.Received(1).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteFalse_DoNotExecutes(string parameter)
        {
            var command = CreateAsyncCommand(_func, () => false);

            command.Execute(parameter);

            _func.DidNotReceive().Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_SyncCallTwoTimes_ExecutesTwoTimes(string parameter)
        {
            var command = CreateAsyncCommand(_func);

            command.Execute(parameter);
            command.Execute(parameter);

            _func.Received(2).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public async Task Execute_AsyncCallSeveralTimes_ExecutesOneTime(string parameter)
        {
            var func = GetFuncWithDelay();
            var command = CreateAsyncCommand(func);

            command.Execute(parameter);
            command.Execute(parameter);
            command.Execute(parameter);

            await func.Received(1).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_SyncWithException_ExecutesWithoutException(string parameter)
        {
            var func = GetFuncWithException();
            var command = CreateAsyncCommand(func);

            command.Execute(parameter);

            func.Received(1).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public async Task Execute_AsyncWithException_ExecutesWithoutException(string parameter)
        {
            var func = GetFuncWithException();
            var command = CreateAsyncCommand(func);

            command.Execute(parameter);

            await func.Received(1).Invoke();
        }
    }
}
