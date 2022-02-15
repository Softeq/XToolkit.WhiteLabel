// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Services;
using Uri = Android.Net.Uri;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services
{
    /// <summary>
    ///     Android platform-specific extended implementation of <see cref="EssentialsLauncherService"/>.
    /// </summary>
    public class DroidLauncherService : EssentialsLauncherService
    {
        private readonly IContextProvider _contextProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DroidLauncherService"/> class.
        /// </summary>
        /// <param name="contextProvider">Provider of Android.Content.Context.</param>
        /// <param name="logManager">An instance of loggers factory.</param>
        public DroidLauncherService(
            IContextProvider contextProvider,
            ILogManager logManager)
            : base(logManager)
        {
            _contextProvider = contextProvider;
        }

        /// <summary>
        ///     Opens URL in the default video player or ask the user.
        /// </summary>
        /// <param name="videoUrl">Url to the video.</param>
        public override void OpenVideo(string videoUrl)
        {
            Execute.BeginOnUIThread(() =>
            {
                var intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(Uri.Parse(videoUrl), "video/*");
                _contextProvider.CurrentActivity.StartActivity(intent);
            });
        }
    }
}
