// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.Content;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    [IntentFilter(new[] { Intent.ActionLocaleChanged })]
    public class LocaleChangeBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            NotificationsHelper.CreateNotificationChannels(context);
        }
    }
}
