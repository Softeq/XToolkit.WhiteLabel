// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers
{
    public class StartPageViewController : ViewControllerBase<StartPageViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View!.BackgroundColor = PlaygroundStyles.DefaultBackgroundColor;
        }
    }
}
