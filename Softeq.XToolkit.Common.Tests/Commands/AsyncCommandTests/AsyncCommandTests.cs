// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Xunit;
using Command = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using Execute = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue", Justification = "Need for tests")]
    public class AsyncCommandTests
    {
        [Fact]
        public void Constructors_Resolved_Correctly()
        {
            var func = Execute.Create();

            _ = new[]
            {
                new AsyncCommand(func),
                new AsyncCommand(func, ex => { }),
                new AsyncCommand(func, () => true),
                new AsyncCommand(func, () => true, ex => { }),
                new AsyncCommand(func, null, ex => { })
            };
        }

        [Fact]
        public void Constructor_ExecuteIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => Command.Create(null));
        }

        [Fact]
        public void Constructor_CanExecuteAndExceptionAreNull_CreatesCorrectly()
        {
            var func = Execute.Create();

            Command.Create(func, null, null);
        }

        [Fact]
        public void Constructor_Default_ReturnsICommand()
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            Assert.IsAssignableFrom<ICommand>(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsIAsyncCommand()
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            Assert.IsAssignableFrom<IAsyncCommand>(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsIRaisableCanExecute()
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            Assert.IsAssignableFrom<IRaisableCanExecute>(command);
        }
    }
}
