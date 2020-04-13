// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Commands
{
    public abstract class CommandTestsCanExecuteBase
    {
        protected static void Assert_CanExecute_WithParameter_ReturnsExpectedValue(
            ICommand command,
            object parameter,
            bool expected)
        {
            var result = command.CanExecute(parameter);

            Assert.Equal(expected, result);
        }

        protected static void Assert_CanExecute_AfterExecuteWithParameter_ReturnsExpectedValue(
            ICommand command,
            object parameter,
            bool expected)
        {
            command.Execute(parameter);

            var result = command.CanExecute(parameter);

            Assert.Equal(expected, result);
        }
    }
}
