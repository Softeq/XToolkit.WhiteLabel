// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using NSubstitute;
using Softeq.XToolkit.Common.Commands;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public partial class AsyncCommandTests
    {
        private readonly Func<Task> _func;

        public AsyncCommandTests()
        {
            _func = Substitute.For<Func<Task>>();
        }

        [Fact]
        public void Constructor_ExecuteIsNull_CreatesCorrectly()
        {
            var command = CreateAsyncCommand(null);

            Assert.NotNull(command);
        }

        [Fact]
        public void Constructor_CanExecuteIsNull_CreatesCorrectly()
        {
            // ReSharper disable once RedundantArgumentDefaultValue
            var command = CreateAsyncCommand(_func, null);

            Assert.NotNull(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsICommand()
        {
            var command = CreateAsyncCommand(_func);

            Assert.IsAssignableFrom<ICommand>(command);
        }

        private static AsyncCommand CreateAsyncCommand(Func<Task> func, Func<bool> canExecute = null)
        {
            return new AsyncCommand(func, canExecute);
        }

        private static Func<Task> GetFuncWithDelay()
        {
            var func = Substitute.For<Func<Task>>();
            func.Invoke().Returns(_ => Task.Delay(10));
            return func;
        }

        private static Func<Task> GetFuncWithException()
        {
            var func = Substitute.For<Func<Task>>();
            func.Invoke().Returns(_ => throw new InvalidOperationException());
            return func;
        }
    }
}
