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
            var func = CreateFuncWithArg();
            var command = CreateAsyncCommandGeneric(func);

            command.Execute(null);

            func.Received(1).Invoke(null);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteTrue_ExecutesOneTime(string parameter)
        {
            var func = CreateFuncWithArg();
            var command = CreateAsyncCommandGeneric(func, () => true);

            command.Execute(parameter);

            func.Received(1).Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteFalse_DoNotExecutes(string parameter)
        {
            var func = CreateFuncWithArg();
            var command = CreateAsyncCommandGeneric(func, () => false);

            command.Execute(parameter);

            func.DidNotReceive().Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_SyncCallTwoTimes_ExecutesTwoTimes(string parameter)
        {
            var func = CreateFuncWithArg();
            var command = CreateAsyncCommandGeneric(func);

            command.Execute(parameter);
            command.Execute(parameter);

            func.Received(2).Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public async Task Execute_AsyncCallSeveralTimes_ExecutesOneTime(string parameter)
        {
            var func = CreateFuncWithArgAndDelay();
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
            var func = CreateFuncWithArgAndException();
            var command = CreateAsyncCommandGeneric(func);

            command.Execute(parameter);

            await func.Received(1).Invoke(parameter);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public async Task Execute_AsICommand_Executes(string parameter)
        {
            var func = CreateFuncWithArg();
            ICommand command = CreateAsyncCommandGeneric(func);

            command.Execute(parameter);

            await func.Received(1).Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.InvalidParameters), MemberType = typeof(CommandsDataProvider))]
        public async Task Execute_AsICommandWithInvalidParameter_ThrowsException(object parameter)
        {
            var func = CreateFuncWithArg();
            ICommand command = CreateAsyncCommandGeneric(func);

            Assert.Throws<InvalidCommandParameterException>(() =>
            {
                command.Execute(parameter);
            });

            await func.DidNotReceive().Invoke(Arg.Any<string>());
        }

        [Theory]
        [InlineData(null)]
        public async Task Execute_AsICommandWithInvalidNullableStruct_ThrowsException(int? parameter)
        {
            var func = CreateFuncWithArg<int?>();
            ICommand command = CreateAsyncCommandGeneric(func);

            Assert.Throws<InvalidCommandParameterException>(() =>
            {
                command.Execute(parameter);
            });

            await func.DidNotReceive().Invoke(Arg.Any<int?>());
        }

        [Theory]
        [InlineData(123)]
        public async Task Execute_AsICommandGenericWithNullableStruct_Executes(int? parameter)
        {
            var func = CreateFuncWithArg<int?>();
            ICommand<int?> command = CreateAsyncCommandGeneric(func);

            command.Execute(parameter);

            await func.Received(1).Invoke(parameter);
        }

        [Theory]
        [InlineData(null)]
        public async Task Execute_AsICommandGenericWithInvalidNullableStruct_ThrowsException(int? parameter)
        {
            var func = CreateFuncWithArg<int?>();
            ICommand<int?> command = CreateAsyncCommandGeneric(func);

            Assert.Throws<InvalidCommandParameterException>(() =>
            {
                command.Execute(parameter);
            });

            await func.DidNotReceive().Invoke(Arg.Any<int?>());
        }
    }
}
