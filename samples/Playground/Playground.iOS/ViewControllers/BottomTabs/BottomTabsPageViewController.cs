// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.BottomTabs;
using Softeq.XToolkit.WhiteLabel.iOS.ViewControllers;
using UIKit;

namespace Playground.iOS.ViewControllers.BottomTabs
{
    public class BottomTabsPageViewController : ToolbarViewControllerBase<BottomTabsPageViewModel, string>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
        }

        protected override UINavigationController CreateRootViewController<TFirstViewModel>(TFirstViewModel viewModel)
        {
            var rootController = base.CreateRootViewController(viewModel);
            rootController.NavigationBarHidden = true;
            return rootController;
        }

        protected override UIImage GetImageFromKey(string key)
        {
            return UIImage.FromBundle(string.Concat("ic", key));
        }
    }
}
