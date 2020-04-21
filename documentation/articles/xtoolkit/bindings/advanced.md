# Advanced Bindings

Bindings provide a powerful and simple approach to use MVVM in your applications.

## Bindings

[Bind](xref:Softeq.XToolkit.Bindings.Extensions.BindableExtensions#Softeq_XToolkit_Bindings_Extensions_BindableExtensions_Bind__1_Softeq_XToolkit_Bindings_Abstract_IBindingsOwner_Expression_Func___0___Action___0__) - an extension method that provides for any views that implement the interface [IBindingsOwner](xref:Softeq.XToolkit.Bindings.Abstract.IBindingsOwner) (more details: [Bindable](bindable.md))

XToolkit Bindings supports three types of bindings: `OneTime`, `OneWay`, `TwoWay`

### One-Way

- This binding mode transfers values from ViewModel to View;
- Whenever the property changes within the ViewModel, the corresponding View property is automatically adjusted;
- This binding mode is useful when showing, for example, data that is arriving from a dynamic source - like from a sensor or from a network data feed.

One-way bindings used by default.

```cs
this.Bind(() => ViewModel.FirstName, () => FirstNameLabel.Text);
```

### Two-Way

- This binding mode transfers values in both directions;
- Changes in both View and ViewModel properties are monitored - if either change, then the other will be updated;
- This binding mode is useful when editing entries in an existing form;
- Have some default definitions for simple platform controls, like `Label`, `UISwitch`, `TextView`, for other cases provides a mechanism for define custom bindings (look below).

```cs
this.Bind(() => ViewModel.Count, () => CountField.Text, BindingMode.TwoWay);
```

### One-Time

- This binding mode transfers values from ViewModel to View;
- This transfer doesn’t actively monitor change messages/events from the ViewModel;
- Instead, this binding mode tries to transfer data from ViewModel to View only when the binding source is set. After this, the binding doesn’t monitor changes and doesn’t perform any updates, unless the binding source itself is reset;
- This mode is not very commonly used but can be useful for fields that are configurable but which don’t tend to change after they have initially been set.

### Extended Bind

```cs
this.Bind(() => ViewModel.Count, count => CountField.Text = count, BindingMode.TwoWay);
```

### Value Converters

A `ValueConverter` is a class that implements the [IConverter<TOut, TIn>](xref:Softeq.XToolkit.Common.Converters.IConverter`2) interface.

```cs
IConverter<string, Person> _personConverter;

this.Bind(() => ViewModel.Person, () => NameLabel.Text, _personConverter);
```

Also, you can use [ConverterBase<TOut, TIn>](xref:Softeq.XToolkit.Common.Converters.ConverterBase`2) abstract class for common cases.

ValueConverters can also be provided with a `parameter` - this can sometimes be useful to reuse a single value converter in different situations.

## Commands

Add command for default controls (UIButton, Button, ...):

```cs
OpenPageButton.SetCommand(ViewModel.OpenPageCommand);
```

Add command for any `EventHandler`:

```cs
linkSpan.SetCommand(nameof(linkSpan.Clicked), ViewModel.ClickHereCommand);
```

## Collections

More details: [iOS & Android Collections](collections.md)

- .
- .
- // TODO
- .
- .

## Rich examples

Applying two-way binding for iOS native control:

```cs
_datePicker = new UIDatePicker(CGRect.Empty);
_formatter = new NSDateFormatter();


var binding = this.SetBinding(
        () => ViewModel.PaymentDate,
        () => _datePicker.Date,
        BindingMode.TwoWay)
    // converter from NSDate to string
    .ConvertTargetToSource(date => _formatter.ToString(date))
    // observe custom event
    .ObserveTargetEvent(nameof(UIDatePicker.ValueChanged)));
```

---
