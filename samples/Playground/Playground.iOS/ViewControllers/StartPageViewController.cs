// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers
{
    public class StartPageViewController : ViewControllerBase<StartPageViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View!.BackgroundColor = GetDefaultColor();
        }

        private static UIColor GetDefaultColor()
        {
            return UIDevice.CurrentDevice.CheckSystemVersion(13, 0)
                ? UIColor.SystemBackgroundColor
                : UIColor.White;
        }
    }
}

