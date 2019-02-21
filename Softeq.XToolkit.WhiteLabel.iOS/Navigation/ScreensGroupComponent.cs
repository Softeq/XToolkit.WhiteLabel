// Developed for PAWS-HALO by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    internal class ScreensGroupComponent : IViewControllerComponent
    {
        public ScreensGroupComponent(string screensGroupName)
        {
            ScreensGroupName = screensGroupName;
        }

        public string Key => string.Empty;
        public string ScreensGroupName { get; }

        public void Attach(UIViewController controller) { }
        public void Detach(UIViewController controller = null) { }
    }
}
