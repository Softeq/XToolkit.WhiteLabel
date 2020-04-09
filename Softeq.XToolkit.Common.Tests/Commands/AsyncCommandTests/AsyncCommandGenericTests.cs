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
    public partial class AsyncCommandGenericTests
    {
        private readonly Func<string, Task> _func;

        public AsyncCommandGenericTests()
        {
            _func = Substitute.For<Func<string, Task>>();
        }

        [Fact]
        public void Constructor_ExecuteIsNull_CreatesCorrectly()
        {
            var command = CreateAsyncCommand<string>(null);

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

        [Fact]
        public void Constructor_Default_ReturnsICommandGeneric()
        {
            var command = CreateAsyncCommand(_func);

            Assert.IsAssignableFrom<ICommand<string>>(command);
        }

        private static AsyncCommand<T> CreateAsyncCommand<T>(Func<T, Task> func, Func<bool> canExecute = null)
        {
            return new AsyncCommand<T>(func, canExecute);
        }

        private static Func<string, Task> GetFuncWithDelay()
        {
            var func = Substitute.For<Func<string, Task>>();
            func.Invoke(Arg.Any<string>()).Returns(_ => Task.Delay(10));
            return func;
        }

        private static Func<string, Task> GetFuncWithException()
        {
            var func = Substitute.For<Func<string, Task>>();
            func.Invoke(Arg.Any<string>()).Returns(_ => throw new InvalidOperationException());
            return func;
        }
    }
}
