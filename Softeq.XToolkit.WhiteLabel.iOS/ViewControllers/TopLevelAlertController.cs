// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ViewControllers
{
    internal class TopLevelAlertController : UIAlertController
    {
        private UIWindow _alertWindow;

        public override UIAlertControllerStyle PreferredStyle => UIAlertControllerStyle.Alert;

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            _alertWindow.Hidden = true;
            _alertWindow = null;
        }

        public void Show()
        {
            var topWindow = UIApplication.SharedApplication.Windows.LastOrDefault(x => x.WindowLevel == UIWindowLevel.Normal);
            if (topWindow == null)
            {
                return;
            }
                
            var rootViewController = new UIViewController();
            rootViewController.View.BackgroundColor = UIColor.Clear;
                
            _alertWindow = new UIWindow(UIScreen.MainScreen.Bounds)
            {
                TintColor = topWindow.TintColor,
                WindowLevel = UIWindowLevel.Alert + 1,
                RootViewController = rootViewController
            };
            _alertWindow.MakeKeyAndVisible();
                
            rootViewController.PresentViewController(this, true, null);
        }
    }
}
