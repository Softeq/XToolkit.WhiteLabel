# XToolkit Permissions

Extended library over the [Xamarin.Essentials Permissions](https://github.com/xamarin/Essentials) that covered common cases of working with permissions.

## Install

When you use this component separately from WhiteLabel.

You can install via NuGet: [![Softeq.XToolkit.Permissions](https://buildstats.info/nuget/Softeq.XToolkit.Permissions?includePreReleases=true)](https://www.nuget.org/packages/Softeq.XToolkit.Permissions)

```text
Install-Package Softeq.XToolkit.Permissions
```

## Setup

### Android

Register dependencies in platform-specific Bootstrapper:

```cs
// permissions
builder.Singleton<PermissionsService, IPermissionsService>();
builder.Singleton<PermissionsManager, IPermissionsManager>();
builder.Singleton<RequestResultHandler, IPermissionRequestHandler>();
```

#### Add Handler

<div class="IMPORTANT">
<h5>IMPORTANT</h5>
<p>This section only for WhiteLabel.<b>Forms.Droid</b> project.

WhiteLabel provides this functionality by default.</p>
</div>

Add registrations to `MainActivity` (Forms.Droid entry point):

```cs
protected override void OnCreate(Bundle savedInstanceState)
{
    base.OnCreate(savedInstanceState);

    // ...

    Xamarin.Essentials.Platform.Init(this, savedInstanceState);

    // ...
}

public override void OnRequestPermissionsResult(
    int requestCode,
    string[] permissions,
    [GeneratedEnum] Permission[] grantResults)
{
    Dependencies.Container.Resolve<IPermissionRequestHandler>()?.Handle(requestCode, permissions, grantResults);

    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
}
```

---

### iOS

Register dependencies in platform-specific Bootstrapper:

```cs
// permissions
builder.Singleton<PermissionsService, IPermissionsService>();
builder.Singleton<PermissionsManager, IPermissionsManager>();
```

---

## Declare permissions

Don't forget to add permission declarations:

- iOS: Info.plist
- Android: AndroidManifest.xml

## Using

Get `IPermissionsManager` from the constructor:

```cs
public class NewPageViewModel : ViewModelBase
{
    public NewPageViewModel(IPermissionsManager permissionsManager)
    {
        // ...
    }
}
```

### Check Permission with Request

```cs
await _permissionsManager.CheckWithRequestAsync<Xamarin.Essentials.Permissions.Camera>();
```

### Check Permission Only

```cs
await _permissionsManager.CheckAsync<Xamarin.Essentials.Permissions.Camera>();
```

## Description

### Common Contracts

Contract | Implementation
---------|----------------
[IPermissionsManager](xref:Softeq.XToolkit.Permissions.IPermissionsManager) | iOS, Android
[IPermissionsService](xref:Softeq.XToolkit.Permissions.IPermissionsService) | iOS, Android
[IPermissionRequestHandler](xref:Softeq.XToolkit.Common.Droid.Permissions.IPermissionRequestHandler) | Android
[PermissionStatus](xref:Softeq.XToolkit.Permissions.PermissionStatus) | Core

### Default Platform-Specific implementation

Default implementation handles a set of predefined permissions (see below) and has additional behavior to request permissions and double-dialog check and opening application settings when the permissions are Denied.

> Default implementation depends on [Xamarin.Essentials.Permissions](https://github.com/xamarin/Essentials)

## About Xamarin.Essentials Permissions

- [Available Permissions](https://docs.microsoft.com/en-us/xamarin/essentials/permissions#available-permissions)
- [Custom Permissions](https://docs.microsoft.com/en-us/xamarin/essentials/permissions#extending-permissionss#available-permissions)

---
