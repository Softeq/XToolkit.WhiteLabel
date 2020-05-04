# Bindable

- [IBindingsOwner](xref:Softeq.XToolkit.Bindings.Abstract.IBindingsOwner) - a common contract for everyone who wants to use bindings (and tasty extensions for them).
- [IBindable](xref:Softeq.XToolkit.Bindings.Abstract.IBindable) - a contract for each Bindable View that provides a mechanism to control `DataContext`.
- [DataContext](xref:Softeq.XToolkit.Bindings.Abstract.IBindable#Softeq_XToolkit_Bindings_Abstract_IBindable_DataContext) - data context for an element when it participates in data binding.

`Data context` is a concept that allows elements to inherit information from their parent elements about the data source that is used for binding, as well as other characteristics of the binding, such as the path.

Each `Bindable*` view must provide a contract to add bindings safely, such as [SetBinding()](xref:Softeq.XToolkit.Bindings.BindingExtensions#Softeq_XToolkit_Bindings_BindingExtensions_SetBinding__2_System_Object_Expression_Func___0___Expression_Func___1___Softeq_XToolkit_Bindings_BindingMode___0___0_) method.

// TODO:

## Create custom View contains Bindings

Need to implement [IBindable](xref:Softeq.XToolkit.Bindings.Abstract.IBindable) interface.

Basic:

```cs
public class CustomView : AnyPlatformUIObject, IBindable
{
    public CustomView()
    {
        Bindings = new List<Binding>();
    }

    public object DataContext { get; set; }

    public List<Binding> Bindings { get; }

    public virtual void SetBindings()
    {
        // set your bindings here
    }
}
```

Add ViewModel property for typed DataContext:

```cs
protected TViewModel ViewModel => (TViewModel) DataContext;
```

Can be setup:

```cs
var view = new CustomView<My>();

```

Add using:
`using Softeq.XToolkit.Bindings.Extensions;`

Use extention methods:

```cs
view.SetDataContext(default(TViewModel));
```

---
