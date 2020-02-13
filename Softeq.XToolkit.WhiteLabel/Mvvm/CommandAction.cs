// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;

namespace Softeq.XToolkit.WhiteLabel.Mvvm
{
    public class CommandAction : DialogOption
    {
        public ICommand Command { get; set; }
    }
}