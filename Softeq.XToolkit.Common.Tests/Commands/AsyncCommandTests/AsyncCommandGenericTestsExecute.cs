// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Windows.Input;
using NSubstitute;
using Softeq.XToolkit.Common.Commands;
using Xunit;
using Command = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using Execute = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public class AsyncCommandGenericTestsExecute
    {
        [Fact]
        public void Execute_CalledOneTime_ExecutesOneTime()
        {
            var func = Execute.CreateFunc<string>();
            var command = Command.CreateAsyncCommandGeneric(func);

            command.Execute(null);

            func.Received(1).Invoke(null);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteTrue_ExecutesOneTime(string parameter)
        {
            var func = Execute.CreateFunc<string>();
            var command = Command.CreateAsyncCommandGeneric(func, _ => true);

            command.Execute(parameter);

            func.Received(1).Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteFalse_DoNotExecutes(string parameter)
        {
            var func = Execute.CreateFunc<string>();
            var command = Command.CreateAsyncCommandGeneric(func, _ => false);

            command.Execute(parameter);

            func.DidNotReceive().Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_SyncCallTwoTimes_ExecutesTwoTimes(string parameter)
        {
            var func = Execute.CreateFunc<string>();
            var command = Command.CreateAsyncCommandGeneric(func);

            command.Execute(parameter);
            command.Execute(parameter);

            func.Received(2).Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_AsyncCallSeveralTimes_ExecutesOneTime(string parameter)
        {
            var func = Execute.CreateFuncWithDelay<string>();
            var command = Command.CreateAsyncCommandGeneric(func);

            command.Execute(parameter);
            command.Execute(parameter);
            command.Execute(parameter);

            func.Received(1).Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_AsyncWithException_ExecutesWithoutException(string parameter)
        {
            var func = Execute.CreateFuncWithException<string>();
            var command = Command.CreateAsyncCommandGeneric(func);

            command.Execute(parameter);

            func.Received(1).Invoke(parameter);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(CommandsDataProvider.DefaultParameter)]
        public void Execute_AsICommand_Executes(string parameter)
        {
            var func = Execute.CreateFunc<string>();
            var command = Command.CreateAsyncCommandGeneric(func) as ICommand;

            command.Execute(parameter);

            func.Received(1).Invoke(parameter);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.InvalidParameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_AsICommandWithInvalidParameter_ThrowsException(object parameter)
        {
            var func = Execute.CreateFunc<string>();
            var command = Command.CreateAsyncCommandGeneric(func) as ICommand;

            command.Execute(parameter);

            func.DidNotReceive().Invoke(Arg.Any<string>());
        }

        [Theory]
        [InlineData(123)]
        [InlineData(null)]
        public void Execute_AsICommandGenericWithNullableStruct_Executes(int? parameter)
        {
            var func = Execute.CreateFunc<int?>();
            var command = Command.CreateAsyncCommandGeneric(func) as ICommand<int?>;

            command.Execute(parameter);

            func.Received(1).Invoke(parameter);
        }

        [Theory]
        [InlineData("test")]
        public void Execute_AfterExecuteTargetGarbageCollected_DoesNotExecute<T>(T parameter)
        {
            var execute = Execute.CreateFunc<T>();
            var command = Command.WithGarbageCollectableExecuteTarget(execute) as ICommand;

            GC.Collect();

            command.Execute(parameter);

            execute.DidNotReceive().Invoke(Arg.Any<T>());
        }

        [Theory]
        [InlineData("test")]
        public void Execute_AfterCanExecuteTargetGarbageCollected_DoesNotExecute<T>(T parameter)
        {
            var execute = Execute.CreateFunc<T>();
            var command = Command.WithGarbageCollectableCanExecuteTarget(execute, _ => true) as ICommand;

            GC.Collect();

            command.Execute(parameter);

            execute.DidNotReceive().Invoke(Arg.Any<T>());
        }

        [Theory]
        [InlineData("test")]
        public void ExecuteGeneric_AfterExecuteTargetGarbageCollected_DoesNotExecute<T>(T parameter)
        {
            var execute = Execute.CreateFunc<T>();
            var command = Command.WithGarbageCollectableExecuteTarget(execute);

            GC.Collect();

            command.Execute(parameter);

            execute.DidNotReceive().Invoke(Arg.Any<T>());
        }

        [Theory]
        [InlineData("test")]
        public void ExecuteGeneric_AfterCanExecuteTargetGarbageCollected_DoesNotExecute<T>(T parameter)
        {
            var execute = Execute.CreateFunc<T>();
            var command = Command.WithGarbageCollectableCanExecuteTarget(execute, _ => true);

            GC.Collect();

            command.Execute(parameter);

            execute.DidNotReceive().Invoke(Arg.Any<T>());
        }
    }
}
