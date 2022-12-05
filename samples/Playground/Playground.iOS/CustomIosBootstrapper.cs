// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CoreData;
using Foundation;
using Playground.Extended;
using Playground.iOS.Extended;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Connectivity;
using Softeq.XToolkit.Connectivity.iOS;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.Permissions.iOS;
using Softeq.XToolkit.PushNotifications;
using Softeq.XToolkit.PushNotifications.Abstract;
using Softeq.XToolkit.PushNotifications.iOS;
using Softeq.XToolkit.PushNotifications.iOS.Abstract;
using Softeq.XToolkit.PushNotifications.iOS.Services;
using Softeq.XToolkit.PushNotifications.iOS.Utils;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Essentials.iOS.FullScreenImage;
using Softeq.XToolkit.WhiteLabel.Essentials.iOS.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.iOS.Services;
using Softeq.XToolkit.WhiteLabel.Navigation;
using UIKit;
using UserNotifications;

namespace Playground.iOS
{
    internal class CustomIosBootstrapper : IosBootstrapperBase
    {
        private readonly NSDictionary _dictionary;

        protected override IList<Assembly> SelectAssemblies()
        {
            return base.SelectAssemblies() // Softeq.XToolkit.WhiteLabel.iOS
                .AddItem(typeof(FullScreenImageViewController).Assembly) // Softeq.XToolkit.WhiteLabel.Essentials.iOS
                .AddItem(GetType().Assembly); // Playground.iOS
        }

        public CustomIosBootstrapper(NSDictionary dictionary)
        {
            _dictionary = dictionary;
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            builder.Singleton(c => new TestService(_dictionary));

            builder.Singleton<IosAppInfoService, IAppInfoService>();

            builder.Singleton<StoryboardDialogsService, IDialogsService>();
            builder.Singleton<IosExtendedDialogsService, IExtendedDialogsService>();

            // permissions
            builder.Singleton<PermissionsService, IPermissionsService>();
            builder.Singleton<PermissionsManager, IPermissionsManager>();

            // image picker
            builder.Singleton<IosImagePickerService, IImagePickerService>();

            // connectivity
            builder.Singleton<IosConnectivityService, IConnectivityService>();

            ConfigurePushNotifications(builder);
        }

        private static void ConfigurePushNotifications(IContainerBuilder builder)
        {
            builder.Singleton<IosPushNotificationService, IPushNotificationsService>();
            builder.Singleton<IosPushNotificationAppDelegate, IPushNotificationAppDelegate>();
            builder.Singleton<IosNotificationCategoriesProvider, INotificationCategoriesProvider>();
            builder.Singleton<IosPushNotificationParser, IIosPushNotificationsParser>();

            builder.Singleton<IosPushNotificationsConsumer>();
            builder.Singleton<IIosPushNotificationsConsumer>(container =>
            {
                var logConsumer = new IosLogPushNotificationsConsumer();
                var defaultConsumer = container.Resolve<IosPushNotificationsConsumer>(ForegroundNotificationOptions.ShowWithBadge);

                return new CompositeIosPushNotificationsConsumer(logConsumer, defaultConsumer);
            });
        }
    }

    public class TestService
    {
        private readonly NSDictionary _options;

        public TestService(NSDictionary options)
        {
            _options = options;
        }
    }

    public class IosLogPushNotificationsConsumer : IIosPushNotificationsConsumer
    {
        public UNAuthorizationOptions GetRequiredAuthorizationOptions()
        {
            return UNAuthorizationOptions.Alert;
        }

        public IEnumerable<UNNotificationCategory> GetCategories()
        {
            return Enumerable.Empty<UNNotificationCategory>();
        }

        public bool TryPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            Debug.WriteLine($"XXX: {nameof(TryPresentNotification)}, notification:{notification}");
            return false;
        }

        public bool TryHandleNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            Debug.WriteLine($"XXX: {nameof(TryHandleNotificationResponse)}");
            return false;
        }

        public bool TryHandleRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Debug.WriteLine($"XXX: {nameof(TryHandleRemoteNotification)}, data: ${userInfo}");
            return false;
        }

        public void OnPushNotificationAuthorizationResult(bool isGranted)
        {
            Debug.WriteLine($"XXX: {nameof(OnPushNotificationAuthorizationResult)}=>{isGranted}");
        }

        public void OnRegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Debug.WriteLine($"XXX: PushDelegate:{nameof(OnRegisteredForRemoteNotifications)}data: ${deviceToken?.AsString()}");
        }

        public void OnFailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            Debug.WriteLine($"XXX: PushDelegate:{nameof(OnFailedToRegisterForRemoteNotifications)}, error:{error}");
        }

        public Task OnUnregisterFromPushNotifications()
        {
            Debug.WriteLine($"XXX: PushDelegate:{nameof(OnUnregisterFromPushNotifications)}");
            return Task.FromResult(false);
        }
    }
}
