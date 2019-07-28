// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.iOS.ViewControllers
{
    public class RootFrameNavigationViewController : RootFrameNavigationControllerBase<RootFrameNavigationViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationBarHidden = true;
        }
    }
}
