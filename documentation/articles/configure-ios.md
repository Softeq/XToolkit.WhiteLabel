# Configure iOS

## Install

- [Install WhiteLabel](xtoolkit/whitelabel.md#install) via NuGet or such as submodule.

## Setup

### Configure AppDelegate

Copy and replace AppDelegate:

```csharp
[Register(nameof(AppDelegate))]
public class AppDelegate : AppDelegateBase
{
    private UINavigationController _rootNavigationController;

    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        var _ = base.FinishedLaunching(app, options);

        // Init navigation
        _rootNavigationController = new UINavigationController();

        Window = new UIWindow(UIScreen.MainScreen.Bounds)
        {
            RootViewController = _rootNavigationController
        };
        Window.MakeKeyAndVisible();

        var navigationService = Dependencies.PageNavigationService;
        navigationService.Initialize(Window.RootViewController);

        // Entry point
        navigationService.For<MainPageViewModel>().Navigate();

        return true;
    }

    protected override IBootstrapper Bootstrapper => new CustomBootstrapper();
}
```

### Configure dependencies

Create custom Bootsrapper:

```cs
public class CustomBootstrapper : IosBootstrapperBase
{
    protected override IList<Assembly> SelectAssemblies()
    {
        // for auto-registration ViewModels by ViewControllers
        return base.SelectAssemblies()
            .AddItem(GetType().Assembly);
    }

    protected override void ConfigureIoc(IContainerBuilder builder)
    {
        // you can register any dependencies here
    }
}
```

### Create and initialize Storyboard

- [Create Storyboard/ViewController](create-storyboard-viewcontroller.md)

---
