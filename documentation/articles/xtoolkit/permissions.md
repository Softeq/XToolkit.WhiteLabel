# XToolkit Permissions

Extended library over the [Plugin.Permissions](https://github.com/jamesmontemagno/PermissionsPlugin) that covered common cases of working with permissions.

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
// this registration need only for WL.Forms
builder.Singleton<ICurrentActivity>(c => CrossCurrentActivity.Current);

// permissions
builder.Singleton<PermissionsService, IPermissionsService>();
builder.Singleton<PermissionsManager, IPermissionsManager>();
builder.Singleton<RequestResultHandler, IPermissionRequestHandler>();
```

#### Add Handler

<div class="IMPORTANT">
<h5>IMPORTANT</h5>
<p>This section only for <b>WhiteLabel.Forms.Droid</b> project.

WhiteLabel provides this functionality by default.</p>
</div>

Add registrations to `MainActivity` (Forms.Droid entry point):

```cs
protected override void OnCreate(Bundle savedInstanceState)
{
    base.OnCreate(savedInstanceState);

    // ...

    CrossCurrentActivity.Current.Init(this, savedInstanceState);

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

### iOS

Register dependencies in platform-specific Bootstrapper:

```cs
// permissions
builder.Singleton<PermissionsService, IPermissionsService>();
builder.Singleton<PermissionsManager, IPermissionsManager>();
```

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
await _permissionsManager.CheckWithRequestAsync<CameraPermission>();
```

### Check Permission Only

```cs
await _permissionsManager.CheckAsync<CameraPermission>();
```

## Description

### Common Contracts

Contract | Implementation
---------|----------------
[IPermissionsManager](xref:Softeq.XToolkit.Permissions.IPermissionsManager) | iOS, Android
[IPermissionsService](xref:Softeq.XToolkit.Permissions.IPermissionsService) | iOS, Android
[IPermissionRequestHandler](xref:Softeq.XToolkit.Common.Droid.Permissions.IPermissionRequestHandler) | Android
[PermissionStatus](xref:Softeq.XToolkit.Permissions.PermissionStatus) | -

### Default Platform-Specific implementation

Default implementation handles a set of predefined permissions (see below) and has additional behavior to request permissions and double-dialog check and opening application settings when the permissions are Denied.

> Default implementation depends on [Plugin.Permissions](https://www.nuget.org/packages/Plugin.Permissions) and [Xam.Plugins.Settings](https://www.nuget.org/packages/Xam.Plugins.Settings)

### Custom Permission

Core:

```cs
public class CustomPermission : BasePermission
{
    public CustomPermission()
        : base(Permission.Unknown)
    {
    }
}
```

iOS:

```cs
public partial class CustomPermission
{
    public override Task<Plugin.Permissions.Abstractions.PermissionStatus> RequestPermissionAsync()
    {
        // custom request
    }
}
```


---
