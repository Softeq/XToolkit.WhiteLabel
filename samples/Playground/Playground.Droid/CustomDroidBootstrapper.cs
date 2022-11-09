// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Android.Content;
using Firebase.Messaging;
using Playground.Droid.Extended;
using Playground.Droid.Services;
using Playground.Extended;
using Softeq.XToolkit.Common.Droid.Permissions;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Connectivity;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.Permissions.Droid;
using Softeq.XToolkit.PushNotifications;
using Softeq.XToolkit.PushNotifications.Abstract;
using Softeq.XToolkit.PushNotifications.Droid;
using Softeq.XToolkit.PushNotifications.Droid.Abstract;
using Softeq.XToolkit.PushNotifications.Droid.Services;
using Softeq.XToolkit.PushNotifications.Droid.Utils;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;
using Softeq.XToolkit.WhiteLabel.Droid.Services;
using Softeq.XToolkit.WhiteLabel.Essentials.Droid.FullScreenImage;
using Softeq.XToolkit.WhiteLabel.Essentials.Droid.ImagePicker;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Navigation;
using IImagePickerService = Softeq.XToolkit.WhiteLabel.Essentials.ImagePicker.IImagePickerService;

namespace Playground.Droid
{
    internal class CustomDroidBootstrapper : DroidBootstrapperBase
    {
        protected override IList<Assembly> SelectAssemblies()
        {
            return base.SelectAssemblies() // Softeq.XToolkit.WhiteLabel.Droid
                .AddItem(typeof(FullScreenImageDialogFragment).Assembly) // Softeq.XToolkit.WhiteLabel.Essentials.Droid
                .AddItem(GetType().Assembly); // Playground.Droid
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
            // core
            CustomBootstrapper.Configure(builder);

            builder.Singleton<DroidAppInfoService, IAppInfoService>();

            builder.Singleton<DroidFragmentDialogService, IDialogsService>();
            builder.Singleton<DroidExtendedDialogsService, IExtendedDialogsService>();

            // permissions
            builder.Singleton<PermissionsService, IPermissionsService>();
            builder.Singleton<PermissionsManager, IPermissionsManager>();
            builder.Singleton<RequestResultHandler, IPermissionRequestHandler>();

            // image picker
            builder.Singleton<DroidImagePickerService, IImagePickerService>();
            builder.Singleton<ImagePickerActivityResultHandler, IImagePickerActivityResultHandler>();

            // connectivity
            builder.Singleton<ConnectivityService, IConnectivityService>();

            // push notifications
            ConfigurePushNotifications(builder);
        }

        private static void ConfigurePushNotifications(IContainerBuilder builder)
        {
            builder.Singleton<DroidPushNotificationsService, IPushNotificationsService>();
            builder.Singleton<IActivityLauncherDelegate>(container => container.Resolve<DroidPushNotificationsService>());
            builder.Singleton<DroidPushNotificationParser, IPushNotificationsParser>();
            builder.Singleton<PlaygroundDroidNotificationsSettingsProvider, INotificationsSettingsProvider>();

            builder.Singleton<DroidPushNotificationsConsumer>();
            builder.Singleton<IPushNotificationsConsumer>(container =>
            {
                var logConsumer = new DroidLogPushNotificationsConsumer();
                var defaultConsumer = container.Resolve<DroidPushNotificationsConsumer>(ForegroundNotificationOptions.ShowWithBadge);

                return new CompositePushNotificationsConsumer(logConsumer, defaultConsumer);
            });
        }
    }

    public class DroidLogPushNotificationsConsumer : IPushNotificationsConsumer
    {
        public bool TryHandleNotification(RemoteMessage message)
        {
            System.Diagnostics.Debug.WriteLine($"XXX: TryHandle:RemoteMessage");
            return false;
        }

        public bool TryHandlePushNotificationIntent(Intent intent)
        {
            System.Diagnostics.Debug.WriteLine($"XXX: TryHandle:Intent");
            return false;
        }

        public void OnPushTokenRefreshed(string token)
        {
            System.Diagnostics.Debug.WriteLine($"XXX: TOKEN: ${token}");
        }

        public async Task OnUnregisterFromPushNotifications()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
