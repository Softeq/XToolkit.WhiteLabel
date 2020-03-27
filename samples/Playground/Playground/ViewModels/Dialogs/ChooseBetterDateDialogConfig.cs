// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Dialogs;

namespace Playground.ViewModels.Dialogs
{
    public class ChooseBetterDateDialogConfig
    {
        public string Title { get; set; } = string.Empty;
        public DateTime First { get; set; }
        public DateTime Second { get; set; }
    }
}
