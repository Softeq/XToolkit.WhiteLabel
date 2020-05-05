// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;
using Command = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.AsyncCommandsFactory;
using Execute = Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests.ExecuteDelegatesFactory;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public class AsyncCommandTestsCanExecuteChanged : CommandTestsCanExecuteChangedBase
    {
        [Fact]
        public void CanExecuteChanged_ExecuteWasFinished_RisesOnce()
        {
            var func = Execute.Create();
            var command = Command.Create(func);

            Assert_CanExecuteChanged_AfterExecute_RisesOnce(command);
        }
    }
}
