// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Dialogs
{
    public partial class SimpleDialogPageViewController : ViewControllerBase<SimpleDialogPageViewModel>
    {
        public SimpleDialogPageViewController(IntPtr handle) : base(handle)
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
