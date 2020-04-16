// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Xunit;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    [SuppressMessage("ReSharper", "RedundantArgumentDefaultValue", Justification = "Need for tests")]
    public class AsyncCommandTests
    {
        [Fact]
        public void Constructors_Resolved_Correctly()
        {
            var func = CreateFunc();

            new AsyncCommand(func);

            new AsyncCommand(func, ex => { });

            new AsyncCommand(func, () => true);
            new AsyncCommand(func, () => true, ex => { });

            var funcGeneric = CreateFuncWithArg<object>();

            new AsyncCommand(funcGeneric, x => true);
            new AsyncCommand(funcGeneric, x => true, ex => { });
        }

        [Fact]
        public void Constructor_ExecuteIsNull_CreatesCorrectly()
        {
            CreateAsyncCommand(null);
        }

        [Fact]
        public void Constructor_CanExecuteIsNull_CreatesCorrectly()
        {
            var func = CreateFunc();

            CreateAsyncCommand(func, null);
        }

        [Fact]
        public void Constructor_AllArgsNull_CreatesCorrectly()
        {
            CreateAsyncCommandWithParam(null, null, null);
        }

        [Fact]
        public void Constructor_Default_ReturnsICommand()
        {
            var func = CreateFunc();
            var command = CreateAsyncCommand(func);

            Assert.IsAssignableFrom<ICommand>(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsIAsyncCommand()
        {
            var func = CreateFunc();
            var command = CreateAsyncCommand(func);

            Assert.IsAssignableFrom<IAsyncCommand>(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsIRaisableCanExecute()
        {
            var func = CreateFunc();
            var command = CreateAsyncCommand(func);

            Assert.IsAssignableFrom<IRaisableCanExecute>(command);
        }
    }
}
