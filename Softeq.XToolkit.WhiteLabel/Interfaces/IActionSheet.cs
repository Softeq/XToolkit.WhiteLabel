// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Windows.Input;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Interfaces
{
    public interface IActionSheet
    {
        string ActionHeader { get; }

        IList<CommandAction> Actions { get; }

        ICommand OpenCommand { get; set; }
    }
}