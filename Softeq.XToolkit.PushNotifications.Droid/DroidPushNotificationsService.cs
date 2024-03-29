﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using AndroidX.Core.App;
using AndroidX.Lifecycle;
using Firebase;
using Firebase.Messaging;
using Java.Interop;
using Softeq.XToolkit.Common.Logger;
using XamarinShortcutBadger;
using Object = Java.Lang.Object;

namespace Softeq.XToolkit.PushNotifications.Droid
{
    public class DroidPushNotificationsService : PushNotificationsServiceBase, IDisposable
    {
        private readonly Context _appContext;
        private readonly AppLifecycleObserver _lifecycleObserver;

        private bool _isInitialized;

        private bool _registrationRequired;
        private ForegroundNotificationOptions _showForegroundNotificationsInSystemOptions;

        public DroidPushNotificationsService(
            IRemotePushNotificationsService remotePushNotificationsService,
            IPushTokenStorageService pushTokenStorageService,
            IPushNotificationsHandler pushNotificationsHandler,
            IPushNotificationParser pushNotificationParser,
            INotificationsSettingsProvider notificationsSettings,
            ILogManager logManager)
            : base(
                  remotePushNotificationsService,
                  pushTokenStorageService,
                  pushNotificationsHandler,
                  pushNotificationParser,
                  logManager)
        {
            _appContext = Application.Context;

            NotificationsHelper.Init(notificationsSettings);

            _lifecycleObserver = new AppLifecycleObserver();
            ProcessLifecycleOwner.Get().Lifecycle.AddObserver(_lifecycleObserver);
        }

        public override void Initialize(ForegroundNotificationOptions showForegroundNotificationsInSystemOptions)
        {
            if (_isInitialized)
            {
                throw new ArgumentException($"{nameof(DroidPushNotificationsService)}: Already Initialized");
            }

            _isInitialized = true;
            _showForegroundNotificationsInSystemOptions = showForegroundNotificationsInSystemOptions;

            NotificationsHelper.CreateNotificationChannels(_appContext);

            FirebaseApp.InitializeApp(_appContext);
            XFirebaseMessagingService.OnTokenRefreshed += OnPushTokenRefreshed;
            XFirebaseMessagingService.OnNotificationReceived += OnNotificationReceived;
        }

        public override void RegisterForPushNotifications()
        {
            Task.Run(async () =>
            {
                var token = await GetTokenAsync();
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
                NotificationManagerCompat.From(_appContext).CancelAll();
            }
        }

        protected override void SetBadgeNumberInternal(int badgeNumber)
        {
            if (_appContext != null)
            {
                var result = ShortcutBadger.ApplyCount(_appContext, badgeNumber);
                Logger.Debug($"Badge count {badgeNumber} was" + (!result ? "NOT" : string.Empty) + "set");
            }
        }

        protected override async Task<bool> UnregisterFromPushTokenInSystem()
        {
            // TODO: possibly use topics and UnsubscribeFromTopic instead
            try
            {
                // Must be called on background thread
                await Task.Run(async () =>
                {
                    // Throws Java.IOException if there's no Internet Connection
                    await FirebaseMessaging.Instance.DeleteToken().AsAsync().ConfigureAwait(false);
                }).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Firebase DeleteInstance failed: {ex?.Message}");
                return false;
            }
        }

        protected override void OnMessageCustomActionInvokedInternal(PushNotificationModel parsedNotification, string actionId, string textInput)
        {
            // Not implemented for now
        }

        protected override void OnMessageReceivedInternal(object pushNotification, PushNotificationModel parsedNotification, bool inForeground)
        {
            base.OnMessageReceivedInternal(pushNotification, parsedNotification, inForeground);

            if (!parsedNotification.IsSilent)
            {
                ShowNotification(pushNotification, parsedNotification, inForeground);
            }
        }

        private void ShowNotification(object pushNotification, PushNotificationModel parsedPushNotification, bool inForeground)
        {
            if (_showForegroundNotificationsInSystemOptions.ShouldShow() || !inForeground)
            {
                var remoteMessage = pushNotification as RemoteMessage;
                var notificationData = remoteMessage?.Data ?? new Dictionary<string, string>();

                NotificationsHelper.CreateNotification(_appContext, parsedPushNotification, notificationData);
            }
        }

        private void OnPushTokenRefreshed(string token)
        {
            // To avoid not needed requests only handle this if we were previously registered (have a push token saved)
            // and token changed or if token was not ready at the moment of registration.

            var currentToken = PushTokenStorageService.PushToken;
            var hasNewToken = !string.IsNullOrEmpty(currentToken) && currentToken != token;

            if (_registrationRequired || hasNewToken)
            {
                _registrationRequired = false;
                OnRegisteredForPushNotifications(token);
            }
        }

        private void OnNotificationReceived(object pushNotification)
        {
            OnMessageReceived(pushNotification, _lifecycleObserver.IsForegrounded);
        }

        private async Task<string?> GetTokenAsync()
        {
            var getTokenNativeTask = FirebaseMessaging.Instance.GetToken();
            Java.Lang.String? token = await getTokenNativeTask.AsAsync<Java.Lang.String?>().ConfigureAwait(false);
            return token?.ToString();
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

    public class AppLifecycleObserver : Object, ILifecycleObserver
    {
        public bool IsForegrounded { get; private set; }

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

    [Service(Exported = true)]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class XFirebaseMessagingService : FirebaseMessagingService
    {
        public static Action<string>? OnTokenRefreshed;
        public static Action<RemoteMessage>? OnNotificationReceived;

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
}
