// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Android.Net;
using Android.Provider;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services
{
    // TODO YP: Rework to Xamarin.Essentials
    public class LauncherService : ILauncherService
    {
        private readonly IActivityProvider _activityProvider;

        public LauncherService(
            IActivityProvider activityProvider)
        {
            _activityProvider = activityProvider;
        }

        public void OpenUrl(string url)
        {
            var currentActivity = _activityProvider.Current;
            var aUri = Uri.Parse(new System.Uri(url).ToString());
            var intent = new Intent(Intent.ActionView, aUri);
            if (intent.ResolveActivity(currentActivity.PackageManager) != null)
            {
                currentActivity.StartActivity(intent);
            }
        }

        public void OpenDeviceSecuritySettings()
        {
            OpenSettings(Settings.ActionSecuritySettings);
        }

        public void OpenAppSettings()
        {
            OpenSettings(Settings.ActionApplicationSettings);
        }

        public void OpenVideo(string videoUrl)
        {
            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(Uri.Parse(videoUrl), "video/*");
            _activityProvider.Current.StartActivity(intent);
        }

        public void OpenEmail(string email)
        {
            var currentActivity = _activityProvider.Current;
            var intent = new Intent(Intent.ActionSendto);
            intent.SetData(Uri.Parse($"mailto:{email}"));
            if (intent.ResolveActivity(currentActivity.PackageManager) != null)
            {
                currentActivity.StartActivity(Intent.CreateChooser(intent, string.Empty));
            }
        }

        public void OpenPhoneNumber(string number)
        {
            var currentActivity = _activityProvider.Current;
            var intent = new Intent(Intent.ActionDial);
            intent.SetData(Uri.Parse($"tel:{number}"));
            if (intent.ResolveActivity(currentActivity.PackageManager) != null)
            {
                currentActivity.StartActivity(intent);
            }
        }

        private void OpenSettings(string settings)
        {
            var intent = new Intent(settings);
            intent.AddFlags(ActivityFlags.NewTask);
            _activityProvider.Current.StartActivity(intent);
        }
    }
}
