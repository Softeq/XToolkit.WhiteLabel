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

## Get Started

Before starting you have to have three projects.
1. ApplicationName(Should be .Net Standart project) which will be include your shared viewModels
2. ApplicationName.iOS
3. ApplicationName.Droid

### Configuring shared logic

#### Create ViewModel

Create class `MyCustomNamePageViewModel` in `ApplicationName.ViewModels` folder.

```csharp
public class MyCustomNamePageViewModel : ViewModelBase
{
}
```
```diff
+ Note: you have to inherit from ViewModelBase
```

## Configure iOS

#### Configure AppDelegate file
Copy and replace AppDelegate:

```csharp
[Register("AppDelegate")]
public class AppDelegate : AppDelegateBase
{
    public override UIWindow Window { get; set; }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        var result = base.FinishedLaunching(application, launchOptions);

        return result;
    }

    public override IList<Assembly> SelectAssemblies()
    {
        return new List<Assembly>
            {
                GetType().Assembly,
            };
    }

    public override void ConfigureIoc(ContainerBuilder builder)
    {
        //services InstancePerLifetimeScope
        builder.PerLifetimeScope<StoryboardPageNavigation, IPageNavigationService>();
        builder.PerLifetimeScope<StoryboardViewLocator, IViewLocator>()
        .WithParameter(new TypedParameter(typeof(Func<UIViewController, UIViewController>), GetRootViewFinder()));

        //services InstancePerDependency
        builder.PerDependency<IPermissionsDialogService, IPermissionsDialogService>();
        builder.PerDependency<StoryboardDialogsService, IDialogsService>();
        builder.PerDependency<IosConsoleLogManager, ILogManager>();

        //view models
        builder.PerDependency<MyCustomNamePage1ViewModel>();
        builder.PerDependency<MyCustomNamePage2ViewModel>();
    }

    private static Func<UIViewController, UIViewController> GetRootViewFinder()
    {
        UIViewController Func(UIViewController controller)
        {
            if (controller.PresentedViewController != null)
            {
                var presentedViewController = controller.PresentedViewController;
                return Func(presentedViewController);
            }

            switch (controller)
            {
                case UINavigationController navigationController:
                    return Func(navigationController.VisibleViewController);
                case UITabBarController tabBarController:
                    return Func(tabBarController.SelectedViewController);
            }

            return controller;
        }

        return Func;
    }
}
```

#### Create and initialize Storyboard

1. Create `MyCustomNamePage1ViewConstroller` class

```csharp
public partial class MyCustomNamePageViewController : ViewControllerBase<MyCustomNamePageViewModel>
{
    public MyCustomNamePageViewController(IntPtr handle) : base(handle)
    {
    }
}
```
```diff
+ Note: you have to inherit from ViewControllerBase<MyCustomNamePageViewModel>
```
```diff
+ Note: if you what to use storyboard you have to create constructor MyCustomNameViewConstroller(IntPtr handle) : base(handle). If you load storyboard via code you have to create constructor without parameters.
```

2. Create `MyCustomNameStoryboard` from `Pages folder-> Add-> New File-> iOS -> Storyboard`. Then open storyboard via Interface Builder(`MyCustomNameStoryboard -> Open With -> XCode Interface Builder`) select `Identity Inspector` and update next properties:
    1. `Class` to `MuCustomNamePageViewController.cs`     
    2. `Storyboard Id` to `MuCustomNameViewController`

After this steps you should be able to navigate to `MyCustomNamePageViewModel`.

## Configure Android

Install: `Softeq.XToolkit.WhiteLabel` and `Softeq.XToolkit.Common` to Android project.

#### Configure project

Remove `MainActivity.cs`.

Create `MainApplication` in root folder and paste the following code:

```csharp
[Application]
public class MainApplication : MainApplicationBase
{
    protected MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
    {
    }

    public override IList<Assembly> SelectAssemblies()
    {
        return new List<Assembly> { GetType().Assembly };
    }

    protected override void ConfigureIoc(ContainerBuilder builder)
    {
        //services InstancePerLifetimeScope
        builder.PerLifetimeScope<PageNavigationService, IPageNavigationService>();
        builder.RegisterType<ViewLocator>();
        builder.PerLifetimeScope<DroidInternalSettings, IInternalSettings>();
        builder.PerLifetimeScope<ViewModelFactoryService, IViewModelFactoryService>();
	builder.PerLifetimeScope<BackStackManager, IBackStackManager>();

        //services InstancePerDependency
        builder.PerDependency<FrameNavigationService, IFrameNavigationService>();
        builder.PerDependency<DroidFragmentDialogService, IDialogsService>();
        builder.PerDependency<DefaultAlertBuilder, IAlertBuilder>();

        //services InstancePerDependency
        //builder.PerDependency<PermissionsDialogService>().As<IPermissionsDialogService>().InstancePerDependency();
        builder.PerDependency<DroidConsoleLogManager>().As<ILogManager>().InstancePerDependency();
	
        //view models InstancePerDependency
        builder.PerDependency<DetailsPageViewModel>();
        builder.PerDependency<MyCustomNamePage1ViewModel>();
        builder.PerDependency<MyCustomNamePage2ViewModel>();

    }
}
```

#### Create Activity

1. Create `StartPageActivity.cs` in `ApplicationName.Droid.Views.Pages`

```csharp
[Activity(MainLauncher = true, Icon = "@mipmap/icon", NoHistory = true)]
public class SplashActivity : AppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        var intent = new Intent(this, typeof(StartPageActivity));

        if (Intent?.Extras != null)
        {
            intent.PutExtras(Intent.Extras);
        }

        StartActivity(intent);
        Finish();
   }
}

[Activity(Theme = "@style/SplashTheme")]
public class StartPageActivity : ActivityBase<StartPageViewModel>
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.my_custom_name_page);
    }
}
```
```diff
+ Note: you have to inherit from ActivityBase<MyCustomNamePageViewModel>
```
2. Create `my_custom_name_page.xml` in `YourProjectName.Droid.Resources.layout`

```xml
<?xml version="1.0" encoding="utf-8"?>
<RelativeLayout xmlns:android="http://schemas.android.com/apk/res/android"
                android:layout_width="match_parent"
                android:layout_height="match_parent"
                android:minWidth="25px"
                android:minHeight="25px"
                android:background="#00ff00">
		<Button android:id="@+id/button2"
			android:layout_width="match_parent"
			android:layout_height="wrap_content"
			android:text="Button" />
</RelativeLayout>
```

After that you have to be able navigate to first page.

## Contributing

We welcome any contributions.

## License

The XToolkit project is available for free use, as described by the [LICENSE](/LICENSE) (MIT).
