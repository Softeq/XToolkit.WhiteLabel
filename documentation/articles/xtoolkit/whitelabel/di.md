# Dependency Injection

WhiteLabel declares own abstraction over the DI implementation.

Out of the box by default used [DryIoc](https://github.com/dadhi/DryIoc).

## Registration

To register dependencies you should use [IContainerBuilder](xref:Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract.IContainerBuilder).

Sample:

```cs
containerBuilder.Singleton<JsonSerializer, IJsonSerializer>();
```

> When you register new instances of internal services, you need to use `IfRegistered.Replace` otherwise you will have runtime exception about duplication registration.

## Resolving

To resolving registered dependencies you should use [IContainer](xref:Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract.IContainer).

Sample:

```cs
container.Resolve<IJsonSerializer>();
```

### Lazy

Because WhiteLabel use DryIoc under the hood, our `IContainer` has the implicit ability for lazy, factory resolving ([DryIoc doc](https://bitbucket.org/dadhi/dryioc/wiki/Wrappers#markdown-header-predefined-wrappers), [Autofac doc](https://autofaccn.readthedocs.io/en/latest/resolve/relationships.html#delayed-instantiation-lazy-b))

```cs
container.Resolve<Lazy<IJsonSerializer>>();
```

### ViewModel

All ViewModels that [registered](#registration) and used with WhiteLabel can resolve dependency via the constructor.

```cs
public class MyViewModel : ViewModelBase
{
    public MyViewModel(
        Lazy<IJsonSerializer> serializer,
        IDataService dataService)
    {
    }
}
```

---
