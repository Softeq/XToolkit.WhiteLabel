# XToolkit Bindings

Implementation of Bindings based on **MVVM Light Toolkit** that supports **.NET Standard** and has a lot of functionality for collections. For more details see [www.mvvmlight.net/doc](http://www.mvvmlight.net/doc)

## Install

When you use this component separately from WhiteLabel.

You can install via NuGet: [![Softeq.XToolkit.Bindings](https://buildstats.info/nuget/Softeq.XToolkit.Bindings?includePreReleases=true)](https://www.nuget.org/packages/Softeq.XToolkit.Bindings)

```text
Install-Package Softeq.XToolkit.Bindings
```

## Getting Started

### Setup

**This section only when you use this component separately from WhiteLabel.**

To start using Bindings just add this code to your project:

#### iOS

in **AppDelegate.cs**

```csharp
public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
{
    // init factory for bindings
    BindingExtensions.Initialize(new AppleBindingFactory());

    ...

    return true;
}
```

#### Android

in **MainApplication.cs**

```cs
public abstract class MainApplicationBase : Application
{
    ...

    public override void OnCreate()
    {
        base.OnCreate();

        // init factory for bindings
        BindingExtensions.Initialize(new DroidBindingFactory());

        ...
    }

    ...
}
```

## More

TODO

---
