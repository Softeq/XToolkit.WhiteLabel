// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Firebase;
using Firebase.Iid;
using Firebase.Messaging;
using Java.IO;
using Softeq.XToolkit.Common.Interfaces;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public class DroidPushNotificationsService : PushNotificationsServiceBase, IDisposable
    {
        private readonly INotificationsSettingsProvider _notificationsSettings;
        private readonly Context _appContext;

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
            : base(remotePushNotificationsService, pushTokenStorageService, pushNotificationsHandler, pushNotificationParser, logManager)
        {
            _appContext = Application.Context;
            _notificationsSettings = notificationsSettings;
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
            XFirebaseIIDService.OnTokenRefreshed += OnPushTokenRefreshed;
            XFirebaseMessagingService.OnNotificationReceived += OnNotificationReceived;
        }

        public override void RegisterForPushNotifications()
        {
            var token = FirebaseInstanceId.Instance.Token;
            if (token == null)
            {
                _registrationRequired = true;
            }
            else
            {
                _registrationRequired = false;

                OnRegisteredForPushNotifications(token);
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
                    FirebaseInstanceId.Instance.DeleteInstanceId(); //Throws Java.IOException if there's no Internet Connection
                    var token = FirebaseInstanceId.Instance.Token; // Value is null here. This call is needed to force new token generation
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

        public override void ClearAllNotifications()
        {
            if (_appContext != null)
            {
                var notificationManager = NotificationManager.FromContext(_appContext);
                notificationManager.CancelAll();
            }
        }

        protected override PushNotificationModel OnMessageReceivedInternal(object pushNotification)
        {
            var parsedNotification = base.OnMessageReceivedInternal(pushNotification);
            if (!parsedNotification.IsSilent)
            {
                ShowNotification(pushNotification, parsedNotification);
            }
            return parsedNotification;
        }

        private void ShowNotification(object pushNotification, PushNotificationModel parsedPushNotification)
        {
            if (_showForegroundNotificationsInSystem || !parsedPushNotification.HandledBySystem)
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
            OnMessageReceived(pushNotification);
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                XFirebaseIIDService.OnTokenRefreshed -= OnPushTokenRefreshed;
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

    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class XFirebaseIIDService : FirebaseInstanceIdService
    {
        public static Action<string> OnTokenRefreshed; // TODO: replace with service resolution?

        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            OnTokenRefreshed?.Invoke(refreshedToken);
        }
    }

    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class XFirebaseMessagingService : FirebaseMessagingService
    {
        public static Action<RemoteMessage> OnNotificationReceived;

        public override void OnMessageReceived(RemoteMessage message)
        {
            OnNotificationReceived?.Invoke(message);
        }
    }
}
