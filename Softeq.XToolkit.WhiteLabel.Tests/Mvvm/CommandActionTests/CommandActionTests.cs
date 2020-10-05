// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Mvvm.CommandActionTests
{
    public class CommandActionTests
    {
        [Theory]
        [MemberData(
                nameof(CommandActionTestsDataProvider.CtorData),
                MemberType = typeof(CommandActionTestsDataProvider))]
        public void Ctor_InitializesProperties(
            ICommand command,
            string title,
            CommandActionStyle commandActionStyle)
        {
            var commandAction = new CommandAction(command, title, commandActionStyle);

            Assert.Equal(command, commandAction.Command);
            Assert.Equal(title, commandAction.Title);
            Assert.Equal(commandActionStyle, commandAction.CommandActionStyle);
        }
    }
}
