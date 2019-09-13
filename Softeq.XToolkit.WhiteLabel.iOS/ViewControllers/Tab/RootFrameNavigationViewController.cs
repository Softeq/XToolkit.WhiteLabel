// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.iOS.ViewControllers.Tab
{
    public class RootFrameNavigationViewController : RootFrameNavigationControllerBase<TabViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationBarHidden = true;
        }
    }
}
