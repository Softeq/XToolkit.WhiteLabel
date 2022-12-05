// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
using Foundation;
using Playground.ViewModels;
using Softeq.XToolkit.PushNotifications.Abstract;
using Softeq.XToolkit.PushNotifications.iOS.Abstract;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.Navigation;
using UIKit;

namespace Playground.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : AppDelegateBase
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var result = base.FinishedLaunching(application, launchOptions);

            // Override point for customization after application launch.

            return result;
        }

        protected override IBootstrapper CreateBootstrapper()
        {
            return new CustomIosBootstrapper(LaunchOptions);
        }

        protected override void OnContainerInitialized(IContainer container)
        {
            container.Resolve<IPushNotificationsService>();

            base.OnContainerInitialized(container);
        }

        protected override void InitializeNavigation(IContainer container)
        {
            var navigationService = container.Resolve<IPageNavigationService>();
            navigationService.Initialize(Window!.RootViewController!);
            navigationService.For<StartPageViewModel>().Navigate();
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Dependencies.Container.Resolve<IPushNotificationAppDelegate>().RegisteredForRemoteNotifications(application, deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            Dependencies.Container.Resolve<IPushNotificationAppDelegate>().FailedToRegisterForRemoteNotifications(application, error);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Dependencies.Container.Resolve<IPushNotificationAppDelegate>().DidReceiveRemoteNotification(application, userInfo, completionHandler);
        }
    }
}
