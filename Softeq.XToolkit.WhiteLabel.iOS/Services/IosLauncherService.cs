// Developed by Softeq Development Corporation
// http://www.softeq.com

using AVFoundation;
using AVKit;
using Foundation;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Services;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    /// <summary>
    ///     iOS platform-specific extended implementation of <see cref="EssentialsLauncherService"/>.
    /// </summary>
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

        /// <summary>
        ///     Opens URL in the system video player.
        /// </summary>
        /// <param name="videoUrl">Url to the video.</param>
        public override void OpenVideo(string videoUrl)
        {
            Execute.BeginOnUIThread(() =>
            {
                var topViewController = _viewLocator.GetTopViewController();
                var playerViewController = new AVPlayerViewController();

                playerViewController.Player ??= new AVPlayer(NSUrl.FromString(videoUrl));
                topViewController.PresentViewController(playerViewController, true, null);
                playerViewController.Player.Play();
            });
        }
    }
}
