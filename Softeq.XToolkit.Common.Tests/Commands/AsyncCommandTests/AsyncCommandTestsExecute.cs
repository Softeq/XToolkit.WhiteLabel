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
    public class AsyncCommandTestsExecute
    {
        [Fact]
        public void Execute_CalledOneTime_ExecutesOneTime()
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            command.Execute(null);

            func.Received(1).Invoke();
        }

        [Theory]
        [InlineData("test")]
        [InlineData(0)]
        public void Execute_WithNotNullParameter_IgnoresParameter(object parameter)
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            command.Execute(parameter);

            func.Received(1).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteTrue_ExecutesOneTime(string parameter)
        {
            var func = Execute.Create();
            var command = Command.Create(func, () => true);

            command.Execute(parameter);

            func.Received(1).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_CanExecuteFalse_DoNotExecutes(string parameter)
        {
            var func = Execute.Create();
            var command = Command.Create(func, () => false);

            command.Execute(parameter);

            func.DidNotReceive().Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_SyncCallTwoTimes_ExecutesTwoTimes(string parameter)
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            command.Execute(parameter);
            command.Execute(parameter);

            func.Received(2).Invoke();
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_AsyncCallSeveralTimes_ExecutesOneTime(string parameter)
        {
            var tcs = new TaskCompletionSource<bool>();
            var func = Execute.FromSource(tcs);
            var command = Command.Create(func);

            command.Execute(parameter);
            command.Execute(parameter);
            command.Execute(parameter);

            func.Received(1).Invoke();

            tcs.SetResult(true);
        }

        [Theory]
        [MemberData(nameof(CommandsDataProvider.Parameters), MemberType = typeof(CommandsDataProvider))]
        public void Execute_AsyncWithException_ExecutesWithoutException(string parameter)
        {
            var func = Execute.WithException();
            var command = Command.Create(func);

            command.Execute(parameter);

            func.Received(1).Invoke();
        }

        [Fact]
        public void Execute_AfterExecuteTargetGarbageCollected_DoesNotExecute()
        {
            var execute = Execute.Create();
            var command = Command.WithGarbageCollectableExecuteTarget(execute);

            GC.Collect();

            command.Execute(null);

            execute.DidNotReceive().Invoke();
        }

        [Fact]
        public void Execute_AfterCanExecuteTargetGarbageCollected_DoesNotExecute()
        {
            var execute = Execute.Create();
            var command = Command.WithGarbageCollectableCanExecuteTarget(execute, () => true);

            GC.Collect();

            command.Execute(null);

            execute.DidNotReceive().Invoke();
        }
    }
}
