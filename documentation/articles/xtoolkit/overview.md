# XToolkit Components Overview

Library | Description | Supported platforms
------- | ----------- | --------------------
[Softeq.XToolkit.Common](common.md) | The most common components without dependencies that can be reused in any project. | Core, Android, iOS
Softeq.XToolkit.Bindings | Bindings implementation based on INotifyPropertyChanged interface (basic MVVMLight) with extends UI collections. | Core, Android, iOS
Softeq.XToolkit.Connectivity |  Library over the [Xam.Plugin.Connectivity](https://github.com/jamesmontemagno/ConnectivityPlugin) with support iOS 12+ native API.  | Core, iOS
Softeq.XToolkit.Permissions | Extended library over the [Plugin.Permissions](https://github.com/jamesmontemagno/PermissionsPlugin) that covered the common cases of working with permissions. | Core, Android, iOS
[Softeq.XToolkit.PushNotifications](push-notifications.md) | Rich implementation of common cases for push-notifications using APNs and Firebase Cloud Messaging. | Core, Android, iOS
[Softeq.XToolkit.Remote](remote.md) | Library provides a common way to make HTTP requests. | Core
[Softeq.XToolkit.WhiteLabel](whitelabel.md) | MVVM framework (DI, Navigation, MVVM) that based on XToolkit components. | Core, Android, iOS
[Softeq.XToolkit.WhiteLabel.Essentials](whitelabel/essentials.md) | Library over the XToolkit.WhiteLabel that contains optional components for any application (like ImagePicker). | Core, Android, iOS
[Softeq.XToolkit.WhiteLabel.Forms](whitelabel/forms.md) | Integration library for using XToolkit.WhiteLabel in [Xamarin.Forms](https://github.com/xamarin/Xamarin.Forms) projects. | Core
