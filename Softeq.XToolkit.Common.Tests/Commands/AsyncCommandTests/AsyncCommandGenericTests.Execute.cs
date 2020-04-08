// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using NSubstitute;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public partial class AsyncCommandGenericTests
    {
        [Fact]
        public void Execute_CalledOneTime_ExecutesOneTime()
        {
            var command = CreateAsyncCommand(_func);

            command.Execute(null);

            _func.Received(1).Invoke(null);
        }

        [Fact]
        public void Execute_CanExecuteTrue_ExecutesOneTime()
        {
            var command = CreateAsyncCommand(_func, () => true);

            command.Execute(null);

            _func.Received(1).Invoke(null);
        }

        [Fact]
        public void Execute_CanExecuteFalse_DoNotExecutes()
        {
            var command = CreateAsyncCommand(_func, () => false);

            command.Execute(null);

            _func.DidNotReceive().Invoke(null);
        }

        [Fact]
        public void Execute_SyncCallTwoTimes_ExecutesTwoTimes()
        {
            var command = CreateAsyncCommand(_func);

            command.Execute(null);
            command.Execute(null);

            _func.Received(2).Invoke(null);
        }

        [Fact]
        public async Task Execute_AsyncCallSeveralTimes_ExecutesOneTime()
        {
            var func = GetFuncWithDelay();
            var command = CreateAsyncCommand(func);

            command.Execute(null);
            command.Execute(null);
            command.Execute(null);

            await func.Received(1).Invoke(null);
        }

        [Fact]
        public void Execute_SyncWithException_ExecutesWithoutException()
        {
            var func = GetFuncWithException();
            var command = CreateAsyncCommand(func);

            command.Execute(null);

            func.Received(1).Invoke(null);
        }

        [Fact]
        public async Task Execute_AsyncWithException_ExecutesWithoutException()
        {
            var func = GetFuncWithException();
            var command = CreateAsyncCommand(func);

            command.Execute(null);

            await func.Received(1).Invoke(null);
        }
    }
}
