// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.Pages;
using Softeq.XToolkit.WhiteLabel.iOS.ViewControllers;
using UIKit;

namespace Playground.iOS.ViewControllers.Pages
{
    public class ToolbarPageViewController : ToolbarViewControllerBase<ToolbarPageViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;
        }
    }
}
