# XToolkit.WhiteLabel

XToolkit.WhiteLabel is a collection of "lego" components for fast create cross-platform mobile applications with Xamarin, based on [XToolkit](https://github.com/Softeq/XToolkit).

## Installation

NuGet:

```
Install-Package Softeq.XToolkit.WhiteLabel
```

## Quick Start

1. Install NuGet package or use `XToolkit` and `XToolkit.WhiteLabel` repositories (clone/submodules).
2. To start using WhiteLabel SDK:

### iOS 

Configure SDK in `AppDelegate.cs`

```csharp
public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
{
    // init factory for bindings
    BindingExtensions.Initialize(new AppleBindingFactory());

    // init assembly sources for Activator.cs
    AssemblySourceCache.Install();
    AssemblySourceCache.ExtractTypes = assembly => assembly.GetExportedTypes()
                                                           .Where(t => typeof(UIViewController).IsAssignableFrom(t));
    AssemblySource.Instance.AddRange(SelectAssemblies());

    // init ui thread helper
    PlatformProvider.Current = new IosPlatformProvider();

    // init dependencies
    var containerBuilder = new ContainerBuilder();
    
    containerBuilder.RegisterType<StoryboardPageNavigation>().As<IPageNavigationService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<StoryboardFrameNavigationService>().As<IFrameNavigationService>().InstancePerDependency();
    
    // register other services here...

    ServiceLocator.StartScope(containerBuilder);
}
```

### Android

Add `MainApplication.cs` to your project

```csharp
[Application]
public abstract class MainApplicationBase : Application
{
    protected MainApplicationBase(IntPtr handle, JniHandleOwnership transfer)
        : base(handle, transfer)
    {
    }

    public override void OnCreate()
    {
        base.OnCreate();
        
        CrossCurrentActivity.Current.Init(this);

        //init factory for bindings
        BindingExtensions.Initialize(new DroidBindingFactory());

        //init assembly sources for Activator.cs
        AssemblySourceCache.Install();
        AssemblySourceCache.ExtractTypes = assembly =>
            assembly.GetExportedTypes()
            .Where(t => typeof(FragmentActivity).IsAssignableFrom(t)
                       || typeof(Android.Support.V4.App.Fragment).IsAssignableFrom(t)
                       || typeof(Android.Support.V4.App.DialogFragment).IsAssignableFrom(t));
        AssemblySource.Instance.AddRange(new List<Assembly> {GetType().Assembly});

        //init dependencies
        StartScopeForIoc();

        //init ui thread helper
        PlatformProvider.Current = new AndroidPlatformProvider();
    }

    private void StartScopeForIoc()
    {
        var containerBuilder = new ContainerBuilder();

        //TODO: add registration here

        ServiceLocator.StartScope(containerBuilder);
    }
    
    protected abstract void ConfigureIoc(ContainerBuilder containerBuilder);
}
```

## Contributing

We welcome any contributions.

## License

The XToolkit project is available for free use, as described by the [LICENSE](/LICENSE) (MIT).
