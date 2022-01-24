// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Services;
using Uri = Android.Net.Uri;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services
{
    public class DroidLauncherService : EssentialsLauncherService
    {
        private readonly IContextProvider _contextProvider;

        public DroidLauncherService(
            IContextProvider contextProvider,
            ILogManager logManager)
            : base(logManager)
        {
            _contextProvider = contextProvider;
        }

        public override void OpenVideo(string videoUrl)
        {
            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(Uri.Parse(videoUrl), "video/*");
            _contextProvider.CurrentActivity.StartActivity(intent);
        }
    }
}
