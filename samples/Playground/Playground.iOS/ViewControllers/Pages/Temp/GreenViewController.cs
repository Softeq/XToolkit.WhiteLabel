// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Pages.Temp;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.Bindings;

namespace Playground.iOS.ViewControllers.Pages.Temp
{
    public partial class GreenViewController : ViewControllerBase<GreenViewModel>
    {
        public GreenViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Button.SetCommand(ViewModel.IncrementCommand);
        }
    }
}