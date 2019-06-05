// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;
using Playground.ViewModels.Pages.Temp;

namespace Playground.iOS
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