// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.BottomTabs.Second;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.BottomTabs.Second
{
    public partial class BlueViewController : ViewControllerBase<BlueViewModel>
    {
        public BlueViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Button.SetCommand(ViewModel.NavigateCommand);
        }
    }
}