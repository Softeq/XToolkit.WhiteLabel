// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content;
using Android.Net;
using Android.Provider;
using Softeq.XToolkit.WhiteLabel.Interfaces;

namespace Softeq.XToolkit.WhiteLabel.Droid.Services
{
    public class LauncherService : ILauncherService
    {
        protected Android.App.Activity CurrentActivity => MainApplicationBase.CurrentActivity;

        public void OpenUrl(string url)
        {
            var aUri = Uri.Parse(new System.Uri(url).ToString());
            var intent = new Intent(Intent.ActionView, aUri);
            if (intent.ResolveActivity(CurrentActivity.PackageManager) != null)
            {
                CurrentActivity.StartActivity(intent);
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
            CurrentActivity.StartActivity(intent);
        }

        public void OpenEmail(string email)
        {
            var intent = new Intent(Intent.ActionSendto);
            intent.SetData(Uri.Parse($"mailto:{email}"));
            if (intent.ResolveActivity(CurrentActivity.PackageManager) != null)
            {
                CurrentActivity.StartActivity(Intent.CreateChooser(intent, string.Empty));
            }
        }

        public void OpenPhoneNumber(string number)
        {
            var intent = new Intent(Intent.ActionDial);
            intent.SetData(Uri.Parse($"tel:{number}"));
            if (intent.ResolveActivity(CurrentActivity.PackageManager) != null)
            {
                CurrentActivity.StartActivity(intent);
            }
        }

        private void OpenSettings(string settings)
        {
            var intent = new Intent(settings);
            intent.AddFlags(ActivityFlags.NewTask);
            CurrentActivity.StartActivity(intent);
        }
    }
}
