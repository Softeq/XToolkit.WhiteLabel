# MVVM

XToolkit.WhiteLabel is a framework based on the [Model-View-ViewModel (MVVM) pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel). The WhiteLabel helps you to separate your View from your Model which creates applications that are cleaner and easier to maintain and extend. It also creates testable applications and allows you to have a much thinner user interface layer (which is more difficult to test automatically).

XToolkit.WhiteLabel is intended to be a fully featured MVVM Framework and does include some features that other frameworks do. ViewModel-first navigation, [DI](di.md), and messaging being the most obvious ones.

XToolkit.Common can serve as a basis for developers who want to create their own MVVM implementation. By providing only the most basic of extra functionality but still following common conventions it should be the easiest option.

## Core files

### ObservableObject

[ObservableObject](xref:Softeq.XToolkit.Common.ObservableObject) contains an implementation of the [INotifyPropertyChanged](xref:System.ComponentModel.INotifyPropertyChanged) interface and is used as a base class for all ViewModels. This makes it easy to update bound properties on the View.

### ViewModelBase

The [ViewModelBase](xref:Softeq.XToolkit.WhiteLabel.Mvvm.ViewModelBase) is the base class for all page view-models in the application.

### Commands

[RelayCommand](xref:Softeq.XToolkit.Common.Commands.RelayCommand)/[AsyncCommand](xref:Softeq.XToolkit.Common.Commands.AsyncCommand) contains an implementation of the [ICommand](xref:System.Windows.Input.ICommand) interface and allows the **View** to call commands on the **ViewModel**, rather than handle UI events directly.

## Navigation

XToolkit.WhiteLabel assumes View-based navigation. This means that a ViewModel will trigger navigation to another View.

You can learn more about Navigation [here](navigation.md).

## Snippets

### Basic property

```cs
private bool _isBusy;

public bool IsBusy
{
    get => _isBusy;
    private set => Set(ref _isBusy, value);
}
```

### Constructor

Init the command via constructor:

```cs
// ctor
{
    StartCommand = new AsyncCommand(StartAsync);
}

public IAsyncCommand StartCommand { get; }
```

### Lazy

Lazy initialization the command:

```cs
private AsyncCommand _startCommand;

public IAsyncCommand StartCommand => _startCommand ??= new AsyncCommand(StartAsync);
```

---
