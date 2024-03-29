# Configure Android

## Install

- [Install WhiteLabel](xtoolkit/whitelabel.md#install) via NuGet or such as submodule.

## Setup

### Configure project

1. Remove `MainActivity.cs`.

2. Create `MainApplication` in the root folder and paste the following code:

```cs
[Application]
public class MainApplication : MainApplicationBase
{
    protected MainApplication(IntPtr handle, JniHandleOwnership transfer)
        : base(handle, transfer)
    {
    }

    protected override IBootstrapper CreateBootstrapper()
    {
        return new CustomBootstrapper();
    }
}
```

3. Create `SplashActivity`:

```cs
[Activity(
    MainLauncher = true,
    NoHistory = true)]
public class SplashActivity : AppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Entry point
        Dependencies.PageNavigationService
            .For<MainPageViewModel>()
            .Navigate();

        Finish();
    }
}
```

### Configure dependencies

Create custom Bootsrapper:

```cs
public class CustomBootstrapper : DroidBootstrapperBase
{
    protected override void ConfigureIoc(IContainerBuilder builder)
    {
        builder.Singleton<JsonSerializer, IJsonSerializer>(); // for saving states

        // you can register any dependencies here
    }

    protected override IList<Assembly> SelectAssemblies()
    {
        // for auto-registering ViewModels by Activities
        return base.SelectAssemblies()
            .AddItem(GetType().Assembly);
    }
}
```

### Create and initialize Activity

- [Create Activity](create-activity.md)

---
