// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Commands
{
    public abstract class CommandTestsCanExecuteChangedBase
    {
        protected static void Assert_CanExecuteChanged_AfterExecute_RisesOnce(ICommand command)
        {
            var eventRaisedCount = 0;

            command.CanExecuteChanged += (s, e) => eventRaisedCount++;

            command.Execute(null);

            Assert.Equal(2, eventRaisedCount);
        }
    }
}
