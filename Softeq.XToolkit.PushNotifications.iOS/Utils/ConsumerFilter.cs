// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Softeq.XToolkit.PushNotifications.iOS.Abstract;
using UserNotifications;

namespace Softeq.XToolkit.PushNotifications.iOS.Utils
{
    public sealed class ConsumerFilter : IConsumerFilter
    {
        private readonly Func<UNNotification, bool> _notificationDelegate;
        private readonly Func<NSDictionary, bool> _userInfoDelegate;

        public ConsumerFilter(
            Func<UNNotification, bool> notificationDelegate,
            Func<NSDictionary, bool> userInfoDelegate)
        {
            _notificationDelegate = notificationDelegate;
            _userInfoDelegate = userInfoDelegate;
        }

        public static ConsumerFilter FromUserInfo(Func<NSDictionary, bool> userInfoDelegate)
        {
            return new ConsumerFilter(
                notification => userInfoDelegate.Invoke(notification.Request.Content.UserInfo),
                userInfoDelegate);
        }

        public bool CanConsume(UNNotification notification) => _notificationDelegate.Invoke(notification);

        public bool CanConsume(NSDictionary userInfo) => _userInfoDelegate.Invoke(userInfo);
    }
}
