// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Dialogs;

namespace Playground.ViewModels.Dialogs
{
    public class ChooseBetterDateDialogConfig : IDialogConfig<DateTime>
    {
        public string Title { get; set; }
        public DateTime First { get; set; }
        public DateTime Second { get; set; }
    }
}
