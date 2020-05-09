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
        public void Execute_WithNotNullParameter_ThrowsException(object parameter)
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            Assert.Throws<ArgumentException>(() => command.Execute(parameter));
        }

        [Fact]
        public void Execute_CanExecuteTrue_ExecutesOneTime()
        {
            var func = Execute.Create();
            var command = Command.Create(func, () => true);

            command.Execute(null);

            func.Received(1).Invoke();
        }

        [Fact]
        public void Execute_CanExecuteFalse_DoesNotExecute()
        {
            var func = Execute.Create();
            var command = Command.Create(func, () => false);

            command.Execute(null);

            func.DidNotReceive().Invoke();
        }

        [Fact]
        public void Execute_SyncCallTwoTimes_ExecutesTwoTimes()
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            command.Execute(null);
            command.Execute(null);

            func.Received(2).Invoke();
        }

        [Fact]
        public void Execute_AsyncCallSeveralTimes_ExecutesOneTime()
        {
            var tcs = new TaskCompletionSource<bool>();
            var func = Execute.FromSource(tcs);
            var command = Command.Create(func);

            command.Execute(null);
            command.Execute(null);
            command.Execute(null);

            func.Received(1).Invoke();

            tcs.SetResult(true);
        }

        [Fact]
        public void Execute_AsyncWithException_ExecutesWithoutException()
        {
            var func = Execute.WithException();
            var command = Command.Create(func);

            command.Execute(null);

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
