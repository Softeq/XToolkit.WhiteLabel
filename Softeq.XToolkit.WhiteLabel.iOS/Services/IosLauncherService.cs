// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using AVFoundation;
using AVKit;
using Foundation;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class IosLauncherService : ILauncherService
    {
        private readonly IViewLocator _viewLocator;

        public IosLauncherService(IViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
        }

        public void OpenUrl(string urlStr)
        {
            var uri = new Uri(urlStr);
            var url = new NSUrl(uri.AbsoluteUri);
            var app = UIApplication.SharedApplication;

            if (app.CanOpenUrl(url))
            {
                app.OpenUrl(url);
            }
        }

        public void OpenAppSettings()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
        }

        public void OpenVideo(string videoUrl)
        {
            var vc = _viewLocator.GetTopViewController();
            var controller = new AVPlayerViewController();

            if (controller.Player == null)
            {
                controller.Player = new AVPlayer(NSUrl.FromString(videoUrl));
            }

            vc.PresentViewController(controller, true, null);
            controller.Player.Play();
        }

        public void OpenEmail(string email)
        {
            OpenUrl($"mailto:{email}");
        }

        public void OpenPhoneNumber(string number)
        {
            OpenUrl($"tel:{number}");
        }
    }
}
