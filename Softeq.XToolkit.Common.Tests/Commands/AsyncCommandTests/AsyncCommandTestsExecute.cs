// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Xunit;
using Command = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using Execute = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public class AsyncCommandTestsExecute
    {
        [Fact]
        public void Execute_CalledOneTime_ExecutesOneTime()
        {
            var func = Execute.CreateFunc();
            var command = Command.CreateAsyncCommand(func);

            command.Execute(null);

            func.Received(1).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteTrue_ExecutesOneTime(string parameter)
        {
            var func = Execute.CreateFunc();
            var command = Command.CreateAsyncCommand(func, () => true);

            command.Execute(parameter);

            func.Received(1).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteFalse_DoNotExecutes(string parameter)
        {
            var func = Execute.CreateFunc();
            var command = Command.CreateAsyncCommand(func, () => false);

            command.Execute(parameter);

            func.DidNotReceive().Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_SyncCallTwoTimes_ExecutesTwoTimes(string parameter)
        {
            var func = Execute.CreateFunc();
            var command = Command.CreateAsyncCommand(func);

            command.Execute(parameter);
            command.Execute(parameter);

            func.Received(2).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_AsyncCallSeveralTimes_ExecutesOneTime(string parameter)
        {
            var func = Execute.CreateFuncWithDelay();
            var command = Command.CreateAsyncCommand(func);

            command.Execute(parameter);
            command.Execute(parameter);
            command.Execute(parameter);

            func.Received(1).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_AsyncWithException_ExecutesWithoutException(string parameter)
        {
            var func = Execute.CreateFuncWithException();
            var command = Command.CreateAsyncCommand(func);

            command.Execute(parameter);

            func.Received(1).Invoke();
        }

        [Fact]
        public void Execute_AfterExecuteTargetGarbageCollected_DoesNotExecute()
        {
            var execute = Execute.CreateFunc();
            var command = Command.WithGarbageCollectableExecuteTarget(execute);

            GC.Collect();

            command.Execute(null);

            execute.DidNotReceive().Invoke();
        }

        [Fact]
        public void Execute_AfterCanExecuteTargetGarbageCollected_DoesNotExecute()
        {
            var execute = Execute.CreateFunc();
            var command = Command.WithGarbageCollectableCanExecuteTarget(execute, () => true);

            GC.Collect();

            command.Execute(null);

            execute.DidNotReceive().Invoke();
        }
    }
}
