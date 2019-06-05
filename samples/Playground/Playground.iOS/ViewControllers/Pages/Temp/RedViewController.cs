// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using Playground.ViewModels.Pages.Temp;
using Softeq.XToolkit.WhiteLabel.iOS;
using System;
using UIKit;
using Softeq.XToolkit.Bindings;

namespace Playground.iOS
{
    public partial class RedViewController : ViewControllerBase<RedViewModel>
    {
        public RedViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Button.SetCommand(ViewModel.NavigateCommand);
        }
    }
}