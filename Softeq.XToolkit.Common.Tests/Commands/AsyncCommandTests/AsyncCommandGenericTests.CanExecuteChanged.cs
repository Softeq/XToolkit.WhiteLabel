// Developed by Softeq Development Corporation
// http://www.softeq.com

using Xunit;

namespace Softeq.XToolkit.Common.Tests.Commands.AsyncCommandTests
{
    public partial class AsyncCommandGenericTests
    {
        [Fact]
        public void CanExecuteChanged_ExecuteWasFinished_RisesOnce()
        {
            var eventRaisedCount = 0;
            var command = CreateAsyncCommand(_func);
            command.CanExecuteChanged += (s, e) => eventRaisedCount++;

            command.Execute(null);

            Assert.Equal(1, eventRaisedCount);
        }
    }
}
