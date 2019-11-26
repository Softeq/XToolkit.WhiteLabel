using Playground.ViewModels.BottomTabs.Second;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Playground.iOS.ViewControllers.BottomTabs.Second
{
    public class SecondRootFrameNavigationController : RootFrameNavigationControllerBase<TabViewModel<BlueViewModel>>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationBarHidden = true;
        }
    }
}