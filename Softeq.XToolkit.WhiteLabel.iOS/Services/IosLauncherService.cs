// Developed by Softeq Development Corporation
// http://www.softeq.com

using AVFoundation;
using AVKit;
using Foundation;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Services;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class IosLauncherService : EssentialsLauncherService
    {
        private readonly IViewLocator _viewLocator;

        public IosLauncherService(
            IViewLocator viewLocator,
            ILogManager logManager)
            : base(logManager)
        {
            _viewLocator = viewLocator;
        }

        public override void OpenVideo(string videoUrl)
        {
            var topViewController = _viewLocator.GetTopViewController();
            var playerViewController = new AVPlayerViewController();

            playerViewController.Player ??= new AVPlayer(NSUrl.FromString(videoUrl));
            topViewController.PresentViewController(playerViewController, true, null);
            playerViewController.Player.Play();
        }
    }
}
