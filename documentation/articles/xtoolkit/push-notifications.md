# XToolkit Push Notifications

Rich implementation of common cases for push-notifications using APNs and Firebase Cloud Messaging.

## Install

When you use this component separately from WhiteLabel.

You can install via NuGet: [![Softeq.XToolkit.PushNotifications](https://buildstats.info/nuget/Softeq.XToolkit.PushNotifications?includePreReleases=true)](https://www.nuget.org/packages/Softeq.XToolkit.PushNotifications)

```text
Install-Package Softeq.XToolkit.PushNotifications
```

## Setup

### Core

1. Add the following WhiteLabel projects to references:
  * `Softeq.XToolkit.PushNotifications`

2. Add implementation for `IPushNotificationsHandler`, do whatever you want in:
  - `HandlePushNotificationReceived`
  - `HandlePushNotificationTapped`
  - `HandleSilentPushNotification`
> all of these methods receive `PushNotificationModel`.
  - `OnPushRegistrationCompleted` has parameters showing if registration was completed in system and on server, you can handle it additionally if needed
  - `HandleInvalidPushNotification` allows you to handle errors during notification parsing, it contains an Exception and a raw notification object
  - `OnPushPermissionsRequestCompleted ` is relevant for iOS platform only and allows you to analyze user reaction to notifications permission request (registration will be performed in any case)

3. Use default implementation(`PushTokenStorageService` which uses `IInternalSettings`) or add your own implementation for `IPushTokenStorageService` which:
  - implements `PushToken` getter and setter to store the token in internal settings, for instance
  - implements storing `IsTokenRegisteredInSystem` and `IsTokenSavedOnServer` flags

4. Add implementation for `IRemotePushNotificationsService`:
  - implement methods to send push token to the server and remove it from server

5. Register your implementations in `Bootstrapper` (IPushNotificationsHandler, IRemotePushNotificationsService), also register an implementation of `ILogManager`

6. Register `PushTokenStorageService` or your implementation as `IPushTokenStorageService` in `Bootstrapper`; if you use default PushTokenStorageService then also register `IInternalSettings` implementation (possibly in platform projects)

#### Optionally

- Add enum for push types and a method to convert string type to enum value

#### Usage

- When you want to subscribe to notifications call `IPushNotificationsService` method `RegisterForPushNotifications`

- When you want to unsubscribe from notifications call `IPushNotificationsService` method `UnregisterFromPushNotifications`, with true parameter if you want to unsubscribe in system as well (default parameter value is false)

- When you want to clear notifications in notification center call `IPushNotificationsService` method `ClearAllNotifications`

- When you want to manually set a value for app badge call `SetBadgeNumber` with required value (NOTE: on Android https://github.com/wcoder/ShortcutBadger is used and not all devices are supported)

### iOS

1. Add the following WhiteLabel projects to references:
  * `Softeq.XToolkit.PushNotifications`
  * `Softeq.XToolkit.PushNotifications.iOS`

2. Perform standard setup: Add Provisioning profile with Push Notifications capability and pass APNs certificates to the server-side

3. Add implementation for `INotificationsPermissionsService`:

  * you can use default implementation `IosNotificationsPermissionsService`
  * you can override `IosNotificationsPermissionsService` and provide custom authentication options as `RequiredAuthOptions`
  * you can provide your own implementation of `INotificationsPermissionsService` with `RequestNotificationsPermissions` method using toolkit `PermissionManager` for instance

4. If needed add custom subclass of `IosPushNotificationParser` where you can:

  * specify custom key for additional data part of the notification (stored as a peer for apps) - you will be able to handle it as you wish later, default key is `Data`;
  * specify custom way to obtain string type from notification, without it the type will always be an empty string, to do this you have the whole notification, the aps part and the additional data part

5. If you want to register some categories for your notifications (with or without actions), add an implementation for `INotificationCategoriesProvider`. Default implementation `IosNotificationCategoriesProvider` does not contain any categories but provides helper methods for simplified actions and categories setup so you can override it to set up your categories.

5. Register your implementations in `Bootstrapper` (INotificationsPermissionsService, IosPushNotificationParser or your custom subclass as IPushNotificationParser, IosNotificationCategoriesProvider if you do not need to work with categories or your custom subclass if you do need to specify some categories as INotificationCategoriesProvider)

6. Register `IosPushNotificationsService` as `IPushNotificationsService` in `Bootstrapper`

7. In your `AppDelegate`

  * In `FinishedLaunching` method call `IPushNotificationsService` method `Initialize` with `ForegroundNotificationOptions.ShowWithBadge` value if you want to display foreground notifications in system notification center and update badge value from notification as well, `ForegroundNotificationOptions.Show` value if you want to display foreground notifications in system notification center but not to update badge value from notification, `ForegroundNotificationOptions.DoNotShow` if you do not want to display foreground notifications in system notification center
  * Override `RegisteredForRemoteNotifications` method and call `IPushNotificationsService` method `OnRegisteredForPushNotifications` with `deviceToken.AsString()`
  * Override `FailedToRegisterForRemoteNotifications` method and call `IPushNotificationsService` method `OnFailedToRegisterForPushNotifications` with `error.Description`
  * If you want to receive silent push notifications override `DidReceiveRemoteNotification(with completionHandler)` method and call `IPushNotificationsService` method `OnMessageReceived` with `userInfo` and any value as second parameter. Do not forget to invoke completionHandler. (Make sure that `aps` in notification payload contains `content-available:1` and does not contain `alert`, `sound` or `badge` keys)

8. Additionally if needed (for silent push notifications) you can add to your `Info.plist` UIBackgroundModes `remote-notification`

### Android

1. Add the following WhiteLabel projects to references:
  * `Softeq.XToolkit.PushNotifications`
  * `Softeq.XToolkit.PushNotifications.Droid`

2. Perform standard Firebase setup
  * Create application project in Firebase Console and pass Server Key to the server-side
  * Add `Xamarin.Firebase.Messaging` to packages
  * Add `google-services.json` to root Droid project with GoogleServicesJson Build Action
  * Add the following to your **AndroidManifet.xml** inside application tag

```xml
<receiver android:name="com.google.firebase.iid.FirebaseInstanceIdInternalReceiver" android:exported="false" />
<receiver android:name="com.google.firebase.iid.FirebaseInstanceIdReceiver" android:exported="true" android:permission="com.google.android.c2dm.permission.SEND">
    <intent-filter>
        <action android:name="com.google.android.c2dm.intent.RECEIVE" />
        <action android:name="com.google.android.c2dm.intent.REGISTRATION" />
        <category android:name="${applicationId}" />
    </intent-filter>
</receiver>
```

  * Optionally add also the following to specify icon and color
```xml
<meta-data android:name="com.google.firebase.messaging.default_notification_icon" android:resource="@drawable/ic_notification" />
<meta-data android:name="com.google.firebase.messaging.default_notification_color" android:resource="@color/notification_color" />
```

3. Add `Mono.Android.Export` to references (required to be able to identify if notifications are received in foreground)

4. Add implementation for `INotificationsSettingsProvider` (they will be applied for manually shown notifications - all data 'notifications' and foreground 'notification' notifications):
  * Provide a dictionary of Notification Channels ids and names for Android 8+, the importance for each channel id, the default channel id and a method that returns channel id for specific notification
  * If needed provide additional configuration for each notification channel before it's registered (for instance, create and set a group) in `ConfigureNotificationChannel `
  * Provide styles for manually shown notifications (return PushNotificationStyles object, where you have to specify NotificationCompat.Style and may customize other properties like an icon, etc.)
  * If needed add more customization of notification (like adding actions) in `CustomizeNotificationBuilder `
  * Provide the type of the Activity which will be opened after tapping on the notification that was shown manually and if parent stack needs to be created for it
  * Additionally you can use helper methods from `NotificationActionsHelper` to work with actions and from `NotificationChannelsHelper` if you need to manage channel groups or notification channels that reflect user choices

5. If needed add custom subclass of `DroidPushNotificationParser` where you can:

  * specify custom key for additional data part of the notification (stored inside RemoteMessage.Data) - you will be able to handle it as you wish later, default key is Data;
  * specify custom keys for title and body if you have them inside RemoteMessage.Data instead of RemoteMessage's Notification part
  * specify custom way to obtain string type from notification, without it the type will always be an empty string, you will have to override two methods - one gives you IDictionary which is RemoteMessage.Data, the other gives you Bundle which stores everything that was in RemoteMessage.Data

6. Register your implementations in `Bootstrapper` (INotificationsSettingsProvider, DroidPushNotificationParser or your custom subclass as IPushNotificationParser)

7. Register `DroidPushNotificationsService` as `IPushNotificationsService` in Bootstrapper

8. In your **MainApplication** in `OnCreate` method call _IPushNotificationsService_ method _Initialize_ with `ForegroundNotificationOptions.Show` value if you want to display foreground notifications in system notification center, `ForegroundNotificationOptions.DoNotShow` otherwise

9. In your starting **activity** `OnCreate` check if _Intent?.Extras_ are not null and contain push notifications data (you can check your custom data key for instance) and call _IPushNotificationsService_ method `OnMessageTapped(extras)` in this case (might require a delay or passing forward and calling OnMessageTapped later)

---
