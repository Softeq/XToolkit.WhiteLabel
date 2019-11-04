// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public class CommandAction
    {
        public CommandAction(ICommand command, string title, CommandActionStyle commandActionStyle = default)
        {
            Command = command;
            Title = title;
            CommandActionStyle = commandActionStyle;
        }

        public ICommand Command { get; set; }

        public string? Title { get; set; }

        public CommandActionStyle CommandActionStyle { get; set; }
    }
}