// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public partial class AsyncCommandGenericTests
    {
        [Fact]
        public void CanExecuteChanged_ExecuteWasFinished_Fires()
        {
            var command = CreateAsyncCommand(_func);
            var wasEventRaised = false;
            command.CanExecuteChanged += (s, e) => wasEventRaised = true;

            command.Execute(null);

            Assert.True(wasEventRaised);
        }
    }
}
