// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
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
            var func = CreateFunc<string>();

            _ = new[]
            {
                new AsyncCommand<string>(func),

                new AsyncCommand<string>(func, ex => { }),

                new AsyncCommand<string>(func, _ => true),
                new AsyncCommand<string>(func, _ => true, ex => { }),
                new AsyncCommand<string>(func, null, ex => { }),

                new AsyncCommand<string>(func, x => true),
                new AsyncCommand<string>(func, x => true, ex => { }),
                new AsyncCommand<string>(func, null, ex => { })
            };
        }

        [Fact]
        public void Constructor_ExecuteIsNull_TrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => CreateAsyncCommandGeneric<string>(null));
        }

        [Fact]
        public void Constructor_CanExecuteAndExceptionHandlerAreNull_CreatesCorrectly()
        {
            var func = CreateFunc<string>();

            CreateAsyncCommandGeneric(func, null, null);
        }

        [Fact]
        public void Constructor_Default_ReturnsICommand()
        {
            var func = CreateFunc<string>();
            var command = CreateAsyncCommandGeneric(func);

            Assert.IsAssignableFrom<ICommand>(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsICommandGeneric()
        {
            var func = CreateFunc<string>();
            var command = CreateAsyncCommandGeneric(func);

            Assert.IsAssignableFrom<ICommand<string>>(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsIAsyncCommandGeneric()
        {
            var func = CreateFunc<string>();
            var command = CreateAsyncCommandGeneric(func);

            Assert.IsAssignableFrom<IAsyncCommand<string>>(command);
        }

        [Fact]
        public void Constructor_Default_ReturnsIRaisableCanExecute()
        {
            var func = CreateFunc<string>();
            var command = CreateAsyncCommandGeneric(func);

            Assert.IsAssignableFrom<IRaisableCanExecute>(command);
        }
    }
}
