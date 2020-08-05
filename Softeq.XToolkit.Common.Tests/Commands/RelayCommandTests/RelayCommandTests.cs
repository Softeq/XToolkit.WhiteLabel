// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Xunit;
using Command = Softeq.XToolkit.Common.Tests.Commands.RelayCommandTests.RelayCommandsFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.RelayCommandTests
{
    public class RelayCommandTests
    {
        [Theory]
        [InlineData("test")]
        [InlineData(0)]
        public void Execute_WithNotNullParameter_IgnoresParameter(object parameter)
        {
            var func = Substitute.For<Action>();
            var command = Command.Create(func);

            command.Execute(parameter);

            func.Received(1).Invoke();
        }

        [Fact]
        public void CanExecute_AfterExecuteTargetGarbageCollected_ReturnsFalse()
        {
            var command = Command.WithGarbageCollectableExecuteTarget(() => { });

            GC.Collect();

            var result = command.CanExecute(null);

            Assert.False(result);
        }

        [Fact]
        public void Execute_AfterExecuteTargetGarbageCollected_DoesNotExecute()
        {
            var execute = Substitute.For<Action>();
            var command = Command.WithGarbageCollectableExecuteTarget(execute);

            GC.Collect();

            command.Execute(null);

            execute.DidNotReceive().Invoke();
        }

        [Fact]
        public void CanExecute_AfterCanExecuteTargetGarbageCollected_ReturnsFalse()
        {
            var command = Command.WithGarbageCollectableCanExecuteTarget(() => { }, () => true);

            GC.Collect();

            var result = command.CanExecute(null);

            Assert.False(result);
        }

        [Fact]
        public void Execute_AfterCanExecuteTargetGarbageCollected_DoesNotExecute()
        {
            var execute = Substitute.For<Action>();
            var command = Command.WithGarbageCollectableCanExecuteTarget(execute, () => true);

            GC.Collect();

            command.Execute(null);

            execute.DidNotReceive().Invoke();
        }
    }
}
