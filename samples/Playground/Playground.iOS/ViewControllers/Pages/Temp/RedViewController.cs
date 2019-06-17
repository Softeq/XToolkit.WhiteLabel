// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Pages.Temp;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.Pages.Temp
{
    public partial class RedViewController : ViewControllerBase<RedViewModel>
    {
        public RedViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Button.SetCommand(ViewModel.NavigateCommand);
        }
    }
}