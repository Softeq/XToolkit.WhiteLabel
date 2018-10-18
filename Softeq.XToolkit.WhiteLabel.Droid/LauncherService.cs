// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Android.Net;
using Android.Provider;
using Plugin.CurrentActivity;
using Softeq.XToolkit.WhiteLabel.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public class LauncherService : ILauncherService
    {
        public void OpenUrl(string url)
        {
            var aUri = Uri.Parse(new System.Uri(url).ToString());
            var intent = new Intent(Intent.ActionView, aUri);
            if (intent.ResolveActivity(CrossCurrentActivity.Current.Activity.PackageManager) != null)
            {
                CrossCurrentActivity.Current.Activity.StartActivity(intent);
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
            CrossCurrentActivity.Current.Activity.StartActivity(intent);
        }

        private void OpenSettings(string settings)
        {
            var intent = new Intent(settings);
            intent.AddFlags(ActivityFlags.NewTask);

            CrossCurrentActivity.Current.Activity.StartActivity(intent);
        }
    }
}