// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.Dialogs
{
    public partial class DialogPageViewController : ViewControllerBase<DialogPageViewModel>
    {
        public DialogPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CloseButton.SetCommand(ViewModel.CloseCommand);
            DoneButton.SetCommand(ViewModel.DoneCommand);
        }
    }
}
