// Developed by Softeq Development Corporation
// http://www.softeq.com

using AVFoundation;
using AVKit;
using Foundation;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class LauncherService : ILauncherService
    {
        private readonly IViewLocator _viewLocator;

        public LauncherService(IViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
        }
        
        public void OpenUrl(string url)
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
        }

        public void OpenDeviceSecuritySettings()
        {
            OpenPrefsUrl(":root=TOUCHID_PASSCODE");
        }

        public void OpenAppSettings()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
        }

        public void OpenVideo(string videoUrl)
        {
            var vc = _viewLocator.GetTopViewController();
            if (vc == null)
            {
                return;
            }

            var controller = new AVPlayerViewController();
            if (controller.Player == null)
            {
                controller.Player = new AVPlayer(NSUrl.FromString(videoUrl));
            }

            vc.PresentViewController(controller, true, null);
            controller.Player.Play();
        }

        private static void OpenPrefsUrl(string url)
        {
            var prefsUrl = new NSUrl($"prefs{url}");
            var nativeUrl = UIApplication.SharedApplication.CanOpenUrl(prefsUrl) ? prefsUrl : new NSUrl($"App-Prefs{url}");
            UIApplication.SharedApplication.OpenUrl(nativeUrl);
        }
    }
}