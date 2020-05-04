# Bootstrapper

Responsibility:

- Setup WhiteLabel
- Setup dependencies

## Description

- WhiteLabel Core has [BootstrapperBase](xref:Softeq.XToolkit.WhiteLabel.Bootstrapper.BootstrapperBase) where declared DI container and setup WhiteLabel dependencies.
- WhiteLabel platforms have [IosBootstrapperBase](xref:Softeq.XToolkit.WhiteLabel.iOS.IosBootstrapperBase), [DroidBootstrapperBase](xref:Softeq.XToolkit.WhiteLabel.Droid.DroidBootstrapperBase), [FormsBootstrapper](xref:Softeq.XToolkit.WhiteLabel.Forms.FormsBootstrapper) classes extended by BootstrapperBase where declared platform-specific dependencies.

*IosBootstrapperBase* and *DroidBootstrapperBase* inheritance of  [BootstrapperWithViewModelLookup](xref:Softeq.XToolkit.WhiteLabel.Bootstrapper.BootstrapperWithViewModelLookup) that provide ability for auto-registering ViewModels for each platform-specific view.

Each application must use WhiteLabel platform-specific bootstrappers for extending.

Playground project contains an actual demo.

### AssemblySource

What is `AssemblySource.Instance`? This is the place that WhiteLabel looks for Views. You can add assemblies to AssemblySource any time during your application lifecycle to make them available to the framework, but there is also a special place to do it in the Bootstrapper. Simply override SelectAssemblies like this:

```cs
protected override IList<Assembly> SelectAssemblies()
{
    return base.SelectAssemblies()     // base WL assembly
        .AddItem(GetType().Assembly);  // app assembly
}
```

All you have to do is return a list of searchable assemblies. By default, the base class returns the WL platform base assembly. If you have multiple referenced assemblies that contain views, this is an extension point you need to remember. Also, if you are dynamically loading modules, you’ll need to make sure they get registered with your IoC container and the AssemblySource.Instance when they are loaded.

## Next

- [Dependency Injection](di.md)

---
