﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Firebase;
using Firebase.Iid;
using Firebase.Messaging;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public class PushNotificationsServiceDroid : PushNotificationsServiceBase, IDisposable
    {
        private readonly INotificationsSettingsProvider _notificationsSettings;
        private readonly Context _appContext;

        private bool _isInitialized;
        private bool _showForegroundNotificationsInSystem;

        private bool _registrationRequired;

        public PushNotificationsServiceDroid(IRemotePushNotificationsService remotePushNotificationsService,
            IPushTokenStorageService pushTokenStorageService, IPushNotificationsHandler pushNotificationsHandler,
            IPushNotificationParser pushNotificationParser, INotificationsSettingsProvider notificationsSettings)
            : base(remotePushNotificationsService, pushTokenStorageService, pushNotificationsHandler, pushNotificationParser)
        {
            _appContext = Application.Context;
            _notificationsSettings = notificationsSettings;
        }

        public override void Initialize(bool showForegroundNotificationsInSystem)
        {
            if (_isInitialized)
            {
                Console.WriteLine("PushNotificationsServiceDroid: Already Initialized");
                return;
            }

            _isInitialized = true;
            _showForegroundNotificationsInSystem = showForegroundNotificationsInSystem;

            //TODO: + update on locale changed
            NotificationsHelper.CreateNotificationChannels(_appContext, _notificationsSettings);

            FirebaseApp.InitializeApp(_appContext);
            XFirebaseIIDService.OnTokenRefreshed += OnPushTokenRefreshed;
            XFirebaseMessagingService.OnNotificationReceived += OnMessageReceived;
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
                OnRegisteredForPushNotificaions(token);
            }
        }

        protected override void UnregisterFromPushTokenInSystem()
        {
            // TODO: possibly use topics and UnsubscribeFromTopic instead
            Task.Run(() =>
            {
                try
                {
                    FirebaseInstanceId.Instance.DeleteInstanceId(); //Throws exception if there's no Internet Connection
                    var token = FirebaseInstanceId.Instance.Token; // Value is null here. This call is needed to force new token generation
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Firebase DeleteInstance failed: {e.Message}");
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

        protected override string SimplifyToken(string token)
        {
            return token;
        }

        protected override void ShowNotification(object pushNotification, PushNotificationModel parsedPushNotification)
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
                OnRegisteredForPushNotificaions(token);
            }
        }

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                XFirebaseIIDService.OnTokenRefreshed -= OnPushTokenRefreshed;
                XFirebaseMessagingService.OnNotificationReceived -= OnMessageReceived;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PushNotificationsServiceDroid()
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
