# Configure iOS

## Install

- [Install WhiteLabel](xtoolkit/whitelabel.md#install) via NuGet or such as submodule.

## Setup

### Configure AppDelegate

Copy and replace AppDelegate:

```cs
[Register(nameof(AppDelegate))]
public class AppDelegate : AppDelegateBase
{
    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        var result = base.FinishedLaunching(application, launchOptions);

        // Override point for customization after application launch.

        return result;
    }

    protected override IBootstrapper CreateBootstrapper()
    {
        return new CustomIosBootstrapper();
    }

    protected override void ConfigureEntryPointNavigation(IPageNavigationService navigationService)
    {
        navigationService.For<StartPageViewModel>().Navigate();
    }
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
