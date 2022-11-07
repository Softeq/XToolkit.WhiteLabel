// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.Services;
using Playground.Services.PushNotifications;
using Playground.ViewModels.Frames;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.PushNotifications;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Services;

namespace Playground
{
    public static class CustomBootstrapper
    {
        public static void Configure(IContainerBuilder builder)
        {
            // playground services
            builder.Singleton<DataService, IDataService>();

            // push notifications
            builder.Singleton<InternalSettings, IInternalSettings>();
            builder.Singleton<PlaygroundPushNotificationsHandler, IPushNotificationsHandler>();
            builder.Singleton<PlaygroundRemotePushNotificationsService, IRemotePushNotificationsService>();
            builder.Singleton<PushTokenStorageService, IPushTokenStorageService>();

            // view models
            builder.PerDependency<TopShellViewModel>();
        }
    }
}
