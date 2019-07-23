// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Android.App;
using Android.Arch.Lifecycle;
using Android.Content;
using Android.Gms.Extensions;
using Firebase;
using Firebase.Iid;
using Firebase.Messaging;
using Java.Interop;
using Java.IO;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public class DroidPushNotificationsService : PushNotificationsServiceBase, IDisposable
    {
        private readonly INotificationsSettingsProvider _notificationsSettings;
        private readonly Context _appContext;
        private readonly AppLifecycleObserver _lifecycleObserver;

        private bool _isInitialized;
        private bool _showForegroundNotificationsInSystem;

        private bool _registrationRequired;

        public DroidPushNotificationsService(
            IRemotePushNotificationsService remotePushNotificationsService,
            IPushTokenStorageService pushTokenStorageService,
            IPushNotificationsHandler pushNotificationsHandler,
            IPushNotificationParser pushNotificationParser,
            INotificationsSettingsProvider notificationsSettings,
            ILogManager logManager)
            : base(remotePushNotificationsService, pushTokenStorageService, pushNotificationsHandler, pushNotificationParser,
                logManager)
        {
            _notificationsSettings = notificationsSettings;
            _appContext = Application.Context;

            _lifecycleObserver = new AppLifecycleObserver();
            ProcessLifecycleOwner.Get().Lifecycle.AddObserver(_lifecycleObserver);
        }

        public override void Initialize(bool showForegroundNotificationsInSystem)
        {
            if (_isInitialized)
            {
                throw new ArgumentException($"{nameof(DroidPushNotificationsService)}: Already Initialized");
            }

            _isInitialized = true;
            _showForegroundNotificationsInSystem = showForegroundNotificationsInSystem;

            //TODO: + update on locale changed
            NotificationsHelper.CreateNotificationChannels(_appContext, _notificationsSettings);

            FirebaseApp.InitializeApp(_appContext);
            XFirebaseMessagingService.OnTokenRefreshed += OnPushTokenRefreshed;
            XFirebaseMessagingService.OnNotificationReceived += OnNotificationReceived;
        }

        public override void RegisterForPushNotifications()
        {
            Task.Run(async () =>
            {
                var token = await FirebaseInstanceId.Instance.GetInstanceId().GetTokenAsync();
                if (token == null)
                {
                    _registrationRequired = true;
                }
                else
                {
                    _registrationRequired = false;

                    OnRegisteredForPushNotifications(token);
                }
            });
        }

        public override void ClearAllNotifications()
        {
            if (_appContext != null)
            {
                var notificationManager = NotificationManager.FromContext(_appContext);
                notificationManager.CancelAll();
            }
        }

        protected override Task<bool> UnregisterFromPushTokenInSystem()
        {
            var tcs = new TaskCompletionSource<bool>();

            // TODO: possibly use topics and UnsubscribeFromTopic instead
            Task.Run(() =>
            {
                var result = false;
                try
                {
                    // Must be called on background thread
                    FirebaseInstanceId.Instance.DeleteInstanceId(); // Throws Java.IOException if there's no Internet Connection

                    result = true;
                }
                catch (IOException e)
                {
                    Logger.Warn($"Firebase DeleteInstance failed: {e.Message}");
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
                finally
                {
                    tcs.TrySetResult(result);
                }
            });

            return tcs.Task;
        }

        protected override PushNotificationModel OnMessageReceivedInternal(object pushNotification, bool inForeground)
        {
            var parsedNotification = base.OnMessageReceivedInternal(pushNotification, inForeground);
            if (!parsedNotification.IsSilent)
            {
                ShowNotification(pushNotification, parsedNotification, inForeground);
            }
            return parsedNotification;
        }

        private void ShowNotification(object pushNotification, PushNotificationModel parsedPushNotification, bool inForeground)
        {
            if (_showForegroundNotificationsInSystem || !inForeground)
            {
                var notificationData = (pushNotification as RemoteMessage)?.Data;
                NotificationsHelper.CreateNotification(_appContext, parsedPushNotification, notificationData, _notificationsSettings);
            }
        }

        private void OnPushTokenRefreshed(string token)
        {
            // To avoid not needed requests only handle this if we were previously registered (have a push token saved) and token changed or if token was not ready at the moment of registration
            if (_registrationRequired || (!string.IsNullOrEmpty(PushTokenStorageService.PushToken) && PushTokenStorageService.PushToken != token))
            {
                _registrationRequired = false;
                OnRegisteredForPushNotifications(token);
            }
        }

        private void OnNotificationReceived(object pushNotification)
        {
            OnMessageReceived(pushNotification, _lifecycleObserver.IsForegrounded);
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                XFirebaseMessagingService.OnTokenRefreshed -= OnPushTokenRefreshed;
                XFirebaseMessagingService.OnNotificationReceived -= OnNotificationReceived;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DroidPushNotificationsService()
        {
            Dispose(false);
        }
        #endregion
    }

    public class AppLifecycleObserver : Java.Lang.Object, ILifecycleObserver
    {
        public bool IsForegrounded { get; private set; } = false;

        [Lifecycle.Event.OnStop]
        [Export]
        public void Stopped()
        {
            IsForegrounded = false;
        }

        [Lifecycle.Event.OnStart]
        [Export]
        public void Started()
        {
            IsForegrounded = true;
        }
    }

    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class XFirebaseMessagingService : FirebaseMessagingService
    {
        public static Action<string> OnTokenRefreshed;
        public static Action<RemoteMessage> OnNotificationReceived;

#pragma warning disable RECS0133 // Parameter name differs in base declaration
        public override void OnNewToken(string token)
#pragma warning restore RECS0133 // Parameter name differs in base declaration
        {
            OnTokenRefreshed?.Invoke(token);
        }

#pragma warning disable RECS0133 // Parameter name differs in base declaration
        public override void OnMessageReceived(RemoteMessage message)
#pragma warning restore RECS0133 // Parameter name differs in base declaration
        {
            OnNotificationReceived?.Invoke(message);
        }
    }

    internal static class GmsTasksExtensions
    {
        internal static async Task<string> GetTokenAsync(this Android.Gms.Tasks.Task getInstanceIdTask)
        {
            var result = await getInstanceIdTask.AsAsync<Java.Lang.Object>();
            return result.Class.GetMethod("getToken").Invoke(result).ToString();
        }
    }
}
