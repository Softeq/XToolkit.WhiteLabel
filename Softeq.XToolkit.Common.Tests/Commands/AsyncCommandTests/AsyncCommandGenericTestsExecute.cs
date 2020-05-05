// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using System.Windows.Input;
using NSubstitute;
using Softeq.XToolkit.Common.Commands;
using Xunit;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public class AsyncCommandGenericTestsExecute
    {
        [Fact]
        public void Execute_CalledOneTime_ExecutesOneTime()
        {
            var func = CreateFunc<string>();
            var command = CreateAsyncCommandGeneric(func);

            command.Execute(null);

            func.Received(1).Invoke(null);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteTrue_ExecutesOneTime(string parameter)
        {
            var func = CreateFunc<string>();
            var command = CreateAsyncCommandGeneric(func, _ => true);

            command.Execute(parameter);

            func.Received(1).Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteFalse_DoNotExecutes(string parameter)
        {
            var func = CreateFunc<string>();
            var command = CreateAsyncCommandGeneric(func, _ => false);

            command.Execute(parameter);

            func.DidNotReceive().Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_SyncCallTwoTimes_ExecutesTwoTimes(string parameter)
        {
            var func = CreateFunc<string>();
            var command = CreateAsyncCommandGeneric(func);

            command.Execute(parameter);
            command.Execute(parameter);

            func.Received(2).Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public async Task Execute_AsyncCallSeveralTimes_ExecutesOneTime(string parameter)
        {
            var func = CreateFuncWithDelay<string>();
            var command = CreateAsyncCommandGeneric(func);

            command.Execute(parameter);
            command.Execute(parameter);
            command.Execute(parameter);

            await func.Received(1).Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public async Task Execute_AsyncWithException_ExecutesWithoutException(string parameter)
        {
            var func = CreateFuncWithException<string>();
            var command = CreateAsyncCommandGeneric(func);

            command.Execute(parameter);

            await func.Received(1).Invoke(parameter);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public async Task Execute_AsICommand_Executes(string parameter)
        {
            var func = CreateFunc<string>();
            ICommand command = CreateAsyncCommandGeneric(func);

            command.Execute(parameter);

            await func.Received(1).Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.InvalidParameters), MemberType = typeof(CommandsDataProvider))]
        public async Task Execute_AsICommandWithInvalidParameter_ThrowsException(object parameter)
        {
            var func = CreateFunc<string>();
            var command = CreateAsyncCommandGeneric(func) as ICommand;

            command.Execute(parameter);

            await func.DidNotReceive().Invoke(Arg.Any<string>());
        }

        [Theory]
        [InlineData(123)]
        [InlineData(null)]
        public async Task Execute_AsICommandGenericWithNullableStruct_Executes(int? parameter)
        {
            var func = CreateFunc<int?>();
            var command = CreateAsyncCommandGeneric(func) as ICommand<int?>;

            command.Execute(parameter);

            await func.Received(1).Invoke(parameter);
        }
    }
}
