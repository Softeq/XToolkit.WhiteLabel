// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.PushNotifications.iOS.Abstract
{
    public interface IPushNotifficationAppDelegate
    {
        void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken);

        void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error);

        void DidReceiveRemoteNotification(
            UIApplication application,
            NSDictionary userInfo,
            Action<UIBackgroundFetchResult> completionHandler);
    }
}
