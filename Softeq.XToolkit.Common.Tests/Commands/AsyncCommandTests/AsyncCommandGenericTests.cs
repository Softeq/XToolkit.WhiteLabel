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
    public class AsyncCommandGenericTests
    {
        [Fact]
        public void Constructors_Resolved_Correctly()
        {
            var func = CreateFuncWithArg<string>();

            new AsyncCommand<string>(func);

            new AsyncCommand<string>(func, ex => { });

            new AsyncCommand<string>(func, () => true);
            new AsyncCommand<string>(func, () => true, ex => { });

            new AsyncCommand<string>(func, x => true);
            new AsyncCommand<string>(func, x => true, ex => { });
        }

        [Fact]
        public void Constructor_ExecuteIsNull_CreatesCorrectly()
        {
            CreateAsyncCommandGeneric<string>(null);
        }

        [Fact]
        public void Constructor_CanExecuteIsNull_CreatesCorrectly()
        {
            var func = CreateFuncWithArg();

            CreateAsyncCommandGeneric(func, null);
        }

        [Fact]
        public void Constructor_AllArgsNull_CreatesCorrectly()
        {
            CreateAsyncCommandGenericWithParam<string>(null, null, null);
        }

        [Fact]
        public void Constructor_Default_ReturnsICommand()
        {
            var func = CreateFuncWithArg();
            var command = CreateAsyncCommandGeneric(func);

            Assert.IsAssignableFrom<ICommand>(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsICommandGeneric()
        {
            var func = CreateFuncWithArg();
            var command = CreateAsyncCommandGeneric(func);

            Assert.IsAssignableFrom<ICommand<string>>(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsIAsyncCommandGeneric()
        {
            var func = CreateFuncWithArg();
            var command = CreateAsyncCommandGeneric(func);

            Assert.IsAssignableFrom<IAsyncCommand<string>>(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsIRaisableCanExecute()
        {
            var func = CreateFuncWithArg();
            var command = CreateAsyncCommandGeneric(func);

            Assert.IsAssignableFrom<IRaisableCanExecute>(command);
        }
    }
}
