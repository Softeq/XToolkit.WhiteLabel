// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using static Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public class AsyncCommandTestsCanExecuteChanged : CommandTestsCanExecuteChangedBase
    {
        [Fact]
        public void CanExecuteChanged_ExecuteWasFinished_RisesOnce()
        {
            var func = CreateFunc();
            var command = CreateAsyncCommand(func);

            Assert_CanExecuteChanged_AfterExecute_RisesOnce(command);
        }
    }
}
