// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public class AsyncCommandTestsExecute
    {
        [Fact]
        public void Execute_CalledOneTime_ExecutesOneTime()
        {
            var func = CreateFunc();
            var command = CreateAsyncCommand(func);

            command.Execute(null);

            func.Received(1).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteTrue_ExecutesOneTime(string parameter)
        {
            var func = CreateFunc();
            var command = CreateAsyncCommand(func, () => true);

            command.Execute(parameter);

            func.Received(1).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteFalse_DoNotExecutes(string parameter)
        {
            var func = CreateFunc();
            var command = CreateAsyncCommand(func, () => false);

            command.Execute(parameter);

            func.DidNotReceive().Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_SyncCallTwoTimes_ExecutesTwoTimes(string parameter)
        {
            var func = CreateFunc();
            var command = CreateAsyncCommand(func);

            command.Execute(parameter);
            command.Execute(parameter);

            func.Received(2).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public async Task Execute_AsyncCallSeveralTimes_ExecutesOneTime(string parameter)
        {
            var func = CreateFuncWithDelay();
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
            var func = CreateFuncWithException();
            var command = CreateAsyncCommand(func);

            command.Execute(parameter);

            func.Received(1).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public async Task Execute_AsyncWithException_ExecutesWithoutException(string parameter)
        {
            var func = CreateFuncWithException();
            var command = CreateAsyncCommand(func);

            command.Execute(parameter);

            await func.Received(1).Invoke();
        }
    }
}
