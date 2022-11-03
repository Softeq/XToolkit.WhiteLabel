// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Firebase.Messaging;

namespace Softeq.XToolkit.PushNotifications.Droid.Abstract
{
    public interface IPushNotificationsConsumer
    {
        bool TryHandleNotification(RemoteMessage message);

        void OnPushTokenRefreshed(string token);

        Task OnUnregisterFromPushNotifications();
    }
}
