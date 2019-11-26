using Playground.ViewModels.BottomTabs.First;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Playground.iOS.ViewControllers.BottomTabs.First
{
    public class FirstRootFrameNavigationController : RootFrameNavigationControllerBase<TabViewModel<RedViewModel>>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationBarHidden = true;
        }
    }
}