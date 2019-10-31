// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public class CommandAction
    {
        public ICommand? Command { get; set; }

        public string? Title { get; set; }

        public CommandActionStyle CommandActionStyle { get; set; }
    }
}