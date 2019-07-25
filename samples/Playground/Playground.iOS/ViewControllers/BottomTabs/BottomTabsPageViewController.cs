// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.BottomTabs;
using Softeq.XToolkit.WhiteLabel.iOS.ViewControllers;
using UIKit;

namespace Playground.iOS.ViewControllers.BottomTabs
{
    public class BottomTabsPageViewController : ToolbarViewControllerBase<BottomTabsPageViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
        }
    }
}
